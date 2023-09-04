module Tests

open Expecto
open DynamicObj
open Fli
open Slurmi
open Process
open helperFunctions


[<Tests>]
let tests =
    testList "job" [
        testCase "creating Job" <| fun _ ->
            let myJob =
                Job("MyJob",[("MyProgram",["MyArgument1";"MyArgument2"]);("test1",["arg1";"arg2"])])
            Expect.equal myJob.Name "MyJob" "JobName check"
            Expect.equal myJob.Processes [("MyProgram",["MyArgument1";"MyArgument2"]);("test1",["arg1";"arg2"])] "JobCommands check"
        
        testCase "adding values to Job" <| fun _ ->
            let myJob2 =
                Job("MyJob2",[("MyProgram",["MyArgument1";"MyArgument2"])])
            myJob2 |> Job.SetNode "MyNode" |> ignore 
            myJob2 |> Job.SetOutput "MyOutput"|> ignore 
            myJob2 |> Job.SetTime ((1,2,3,4))|> ignore 
            myJob2 |> Job.SetNTasks 5|> ignore 
            myJob2 |> Job.SetCPUsPerTask 5|> ignore 
            myJob2 |> Job.SetMemory "MyMemory"|> ignore 
            myJob2 |> Job.SetPartition "MyPartition"|> ignore 
            myJob2 |> Job.SetFileName "MyFileName"|> ignore 
            myJob2 |> Job.SetOutputFile "MyOutputFile"|> ignore 
            myJob2 |> Job.SetError "MyError"|> ignore 
            myJob2 |> Job.SetJobID(1234)|> ignore 
            myJob2 |> Job.SetParsable(true)|> ignore 
            let newenv = new EnvironmentSLURM() 
            newenv.AddCommandAndArgument "module load" "proteomiqon" |> ignore
            newenv.AddCommandAndArgument "hello" "panda" |> ignore
            myJob2 |> Job.SetEnvironment newenv |> ignore

            let jobscript = Job.createJobscript myJob2
            Expect.equal myJob2.Name "MyJob2" "JobName check"
            Expect.equal myJob2.Processes [("MyProgram",["MyArgument1";"MyArgument2"])] "JobCommands2 check"
            Expect.equal (myJob2 |> Job.tryGetNode) (Some "MyNode") "Acces value Node Job check"
            Expect.equal (myJob2 |> Job.tryGetOutput) (Some "MyOutput") "Acces value Output Job check"
            Expect.equal (myJob2 |> Job.tryGetTime) (Some (1,2,3,4)) "Acces value Time Job check"
            Expect.equal (myJob2 |> Job.tryGetNTasks) (Some 5) "Acces value NTasks Job check"
            Expect.equal (myJob2 |> Job.tryGetCPUsPerTask) (Some 5) "Acces value CPUs Job check"
            Expect.equal (myJob2 |> Job.tryGetMemory) (Some "MyMemory") "Acces value Memory Job check"
            Expect.equal (myJob2 |> Job.tryGetPartition) (Some "MyPartition") "Acces value Partition Job check"
            Expect.equal (myJob2 |> Job.tryGetFileName) (Some "MyFileName") "Acces value Filename Job check"
            Expect.equal (myJob2 |> Job.tryGetOutputFile) (Some "MyOutputFile") "Acces value OutputFile Job check"
            Expect.equal (myJob2 |> Job.tryGetError) (Some "MyError") "Acces value Error Job check"
            Expect.equal (myJob2 |> Job.tryGetJobID) (Some 1234) "Acces value JodId Job check"
            Expect.equal (myJob2 |> Job.tryGetParsable) (Some true) "Acces value Parsable Job check"
            
            //Expect.equal jobscript 
            //    [|"#!/bin/bash"; "#SBATCH -J MyJob2"; "#SBATCH -N MyNode";
            //      "#SBATCH -o MyOutput.out"; "#SBATCH -e MyError.err";
            //      "#SBATCH --time=01-02:03:04"; "#SBATCH --ntasks=5";
            //      "#SBATCH --cpus-per-task=5"; "#SBATCH --mem=MyMemory";
            //      "#SBATCH -p MyPartition"; "hello panda \n module load proteomiqon";
            //      "MyProgram MyArgument1 MyArgument2"|]
            //        "Jobscript check"


        testCase "environment" <| fun _ ->
            let newenv = new EnvironmentSLURM()
            newenv.AddCommandAndArgument "module load" "proteomiqon" |> ignore
            newenv.AddCommandAndArgument "echo" "panda" |> ignore
            let myJob3 =
                Job("MyJob3",[("MyProgram",["MyArgument1";"MyArgument2"]);("test1",["arg1";"arg2"])])
            myJob3 |> Job.SetEnvironment newenv |> ignore
            Expect.equal myJob3.Name "MyJob3" "JobName check"

        testCase "Joblist" <| fun _ -> 
            let myJoblist =
                [|
                    Job ("MyJob",[("MyProgram",["MyArgument1";"MyArgument2"])]);
                    Job ("MyJob2",[("MyProgram2",["MyArgument12";"MyArgument22"])]);
                    Job ("MyJob3",[("MyProgram3",["MyArgument13";"MyArgument23"])]);
                |]
            myJoblist |> Array.mapi (fun i x -> x |> Job.SetCPUsPerTask(i)) |> ignore
            
            Expect.equal (myJoblist|> Array.map (fun x -> x |> Job.tryGetCPUsPerTask)) [|Some (0); Some (1); Some (2)|] "iterating Joblist check"
            
            myJoblist.[1] |> Job.SetDependency((All,[|Afterany [(findDependencyName "MyJob3" myJoblist);"noJobHere";"justAnExample"];Afternotok [(findDependencyName "MyJob3" myJoblist);"noJobHere2";"justAnExample2"]|])) |> ignore
            myJoblist.[2] |> Job.SetDependency((Any,[|After [(findDependencyName "MyJob" myJoblist,Some 2);("alsoJustAnExample",None)]|])) |> ignore
            
            Expect.equal (myJoblist.[0] |> Job.tryGetDependency) (None) "Get Dependency check"
            Expect.equal (myJoblist.[1] |> Job.tryGetDependency) 
                (Some (All,[|Afterany ["$Job2"; "noJobHere"; "justAnExample"];Afternotok ["$Job2"; "noJobHere2"; "justAnExample2"]|])) 
                        "Get Dependency check"
            Expect.equal (myJoblist.[2] |> Job.tryGetDependency) (Some (Any, [|After [("$Job0", Some 2); ("alsoJustAnExample", None)]|])) "Dependency check2"
            let myWorkflow = new Workflow(myJoblist)
            myWorkflow |> Workflow.SetTime((2,3,4,1)) |> ignore 
            myWorkflow |> Workflow.SetPartition("csb") |> ignore 
            let testWF = createWorkflowFromWFType myWorkflow
            let expectedWF = [| "#!/bin/bash"; "#SBATCH --time=02-03:04:01"; "#SBATCH -p csb"; "Job0=$(sbatch --parsable  MyJob)"; "Job1=$(sbatch --parsable --dependency=afterany:$Job2:noJobHere:justAnExample,afternotok:$Job2:noJobHere2:justAnExample2 MyJob2)"; "Job2=$(sbatch --parsable --dependency=after:$Job0+2:alsoJustAnExample MyJob3)" |]
            Expect.equal testWF expectedWF "Workflow check"

        testCase "Dependencies" <| fun _ -> 
            let dep1 = DependencyType.toString (DependencyType.After [("Job1",None);("Job2",Some 3)])
            let dep2 = DependencyType.toString (DependencyType.Afterany ["Job1";"Job2"])
            let dep3 = DependencyType.toString (DependencyType.Afternotok ["Job1";"Job2"])
            let dep4 = DependencyType.toString (DependencyType.Afterok ["Job1";"Job2"])
            let dep5 = DependencyType.toString (DependencyType.Afterburstbuffer ["Job1";"Job2"])
            let dep6 = DependencyType.toString (DependencyType.Aftercorr ["Job1";"Job2"])
            let dep7 = DependencyType.toString (DependencyType.Singleton ["Job1";"Job2"])
            Expect.equal dep1 "after:Job1:Job2+3" "Dependency 1 check"
            Expect.equal dep2 "afterany:Job1:Job2" "Dependency 2 check"
            Expect.equal dep3 "afternotok:Job1:Job2" "Dependency 3 check"
            Expect.equal dep4 "afterok:Job1:Job2" "Dependency 4 check"
            Expect.equal dep5 "afterburstbuffer:Job1:Job2" "Dependency 5 check"
            Expect.equal dep6 "aftercorr:Job1:Job2" "Dependency 6 check"
            Expect.equal dep7 "singleton" "Dependency 7 check"
            
            let dependencyToTestAll = DependencyType.concat All [|Afterany ["Job1";"Job2"];Afternotok ["Job1";"Job2"]|]
            let dependencyToTestAny = DependencyType.concat Any [|After ["Job1",Some 2]; Afterok[ "Job2"]|]
            
            Expect.equal dependencyToTestAll "afterany:Job1:Job2,afternotok:Job1:Job2" "Concat dependencies All check"
            Expect.equal dependencyToTestAny "after:Job1+2?afterok:Job2" "Concat dependencies Any check"


        testCase "JobFunctions" <| fun _ -> 
            Expect.equal (Job.ifOnlyOneDigit 2) "02" "ifOnlyOneDigit 02 check"
            Expect.equal (Job.ifOnlyOneDigit 12) "12" "ifOnlyOneDigit 12 check"
            Expect.equal (Job.formatTime (1,2,3,4)) "01-02:03:04" "formatTime 1 check"
            Expect.equal (Job.formatTime (0,0,0,2)) "00-00:00:02" "formatTime 2 check"

        testCase "general functions" <| fun _ -> 
            let jobArray = 
                [|
                    Job ("MyJob",[("MyProgram",["MyArgument1";"MyArgument2"])]);
                    Job ("MyJob2",[("MyProWgram2",["MyArgument12";"MyArgument22"])]);
                    Job ("MyJob3",[("MyProgram3",["MyArgument13";"MyArgument23"])]);
                |]
            Expect.equal (findDependencyName "MyJob" jobArray) "$Job0" "findDependencyName 1 check"
            Expect.equal (findDependencyName "MyJob2" jobArray) "$Job1" "findDependencyName 1 check"
            Expect.equal (findDependencyName "MyJob3" jobArray) "$Job2" "findDependencyName 1 check"

            let a = jobArray.[0] |> Job.SetDependency((Any,[|Afternotok [("Welcome to the Black Parade");("MCR")]|]))
            Expect.equal (getDependency jobArray.[0]) "--dependency=afternotok:Welcome to the Black Parade:MCR" "getDependency check"
            Expect.equal (createProcessCall("hey",["Hey";"Yeah";"You"])) "hey Hey Yeah You" "createProcessCall check"

        ]

  
