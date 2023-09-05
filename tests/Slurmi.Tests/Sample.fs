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
            myJob2 |> Job.SetAccount "MyAccount" |> ignore
            myJob2 |> Job.SetBatch ["This";"Is";"A";"Test";"&";"Batch"] |> ignore 
            myJob2 |> Job.SetBurstBufferSpecification ["This";"|";"Is";"A";"Test"] |> ignore
            myJob2 |> Job.SetBurstBufferSpecificationFilePath "BBF" |> ignore
            myJob2 |> Job.SetWorkingDirectory "wdir" |> ignore
            myJob2 |> Job.SetClusters ["cluster1";"cluster2"] |> ignore
            myJob2 |> Job.SetComment "new comment $§" |> ignore
            myJob2 |> Job.SetContainer "MyContainer" |> ignore
            myJob2 |> Job.SetContainerID "MyContainerID" |> ignore
            myJob2 |> Job.SetContiguous true |> ignore
            myJob2 |> Job.SetSpezializedCores 3 |> ignore
            myJob2 |> Job.SetCoresPerSocket 2 |> ignore
            myJob2 |> Job.SetCPUsPerGPU 7 |> ignore
            myJob2 |> Job.SetDelayBoot 8 |> ignore
            myJob2 |> Job.SetExclude ["Hey";"Exclude";"Me"] |> ignore
            myJob2 |> Job.SetExtra "Hey I'm Extra" |> ignore
            myJob2 |> Job.SetGroupID "CSB" |> ignore
            myJob2 |> Job.SetHold true |> ignore
            myJob2 |> Job.SetIgnorePBS true |> ignore
            myJob2 |> Job.SetInput "MyInput" |> ignore
            myJob2 |> Job.SetKillOnInvalidDep true |> ignore
            myJob2 |> Job.SetMailUser "example@type.de" |> ignore
            myJob2 |> Job.SetMemoryPerGPU "30gb" |> ignore 
            myJob2 |> Job.SetMemoryPerCPU "20gb" |> ignore 
            myJob2 |> Job.SetMinCPUs 9 |> ignore
            myJob2 |> Job.SetNoRequeue true |> ignore
            myJob2 |> Job.SetNodeFile "MyNodeFile" |> ignore
            myJob2 |> Job.SetNTasksPerCore 10 |> ignore
            myJob2 |> Job.SetNTasksPerGPU 20 |> ignore
            myJob2 |> Job.SetNTasksPerNode 23 |> ignore
            myJob2 |> Job.SetNTasksPerSocket 1337 |> ignore
            myJob2 |> Job.SetOvercommit true |> ignore
            myJob2 |> Job.SetOversubscribe true |> ignore
            myJob2 |> Job.SetPrefer ["ThisCPU";"ThisNode"] |> ignore
            myJob2 |> Job.SetQuiet true |> ignore 
            myJob2 |> Job.SetReboot true |> ignore
            myJob2 |> Job.SetRequeue true |> ignore
            myJob2 |> Job.SetReservation ["Here";"There"] |> ignore
            myJob2 |> Job.SetSocketsPerNode 69 |> ignore
            myJob2 |> Job.SetSpreadJob true |> ignore
            myJob2 |> Job.SetTestOnly true |> ignore
            myJob2 |> Job.SetThreadSpec 42 |> ignore
            myJob2 |> Job.SetThreadsPerCore 2323 |> ignore
            myJob2 |> Job.SetMinTime ((1,2,3,4)) |> ignore
            myJob2 |> Job.SetUserID "MyUserID" |> ignore
            myJob2 |> Job.SetUseMinNodes true |> ignore
            myJob2 |> Job.SetVerbose true |> ignore
            myJob2 |> Job.SetWait true |> ignore
            myJob2 |> Job.SetWaitAllNodes true |> ignore
            myJob2 |> Job.SetWrap ["Wrap";"Burrito";"Taco"] |> ignore

            



            //let jobscript = Job.createJobscript myJob2
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
            Expect.equal (myJob2 |> Job.tryGetAccount) (Some "MyAccount") "Acces value Account Job check"
            Expect.equal (myJob2 |> Job.tryGetBatch) (Some ["ThisIsATest&Batch"]) "Acces value Batch Job check"
            Expect.equal (myJob2 |> Job.tryGetBurstBufferSpecification) (Some ["This|IsATest"]) "Acces value BBS Job check"
            Expect.equal (myJob2 |> Job.tryGetBurstBufferSpecificationFilePath) (Some "BBF")  "Acces value BBS Job check"
            Expect.equal (myJob2 |> Job.tryGetWorkingDirectory) (Some "wdir") "Acces value workingdirectory Job check"
            Expect.equal (myJob2 |> Job.tryGetClusters) (Some ["cluster1,cluster2"]) "Acces value clusters Job check"
            Expect.equal (myJob2 |> Job.tryGetComment) (Some "new comment $§") "Acces value comment Job check"
            Expect.equal (myJob2 |> Job.tryGetContainer) (Some "MyContainer") "Acces value container Job check"
            Expect.equal (myJob2 |> Job.tryGetContainerID) (Some "MyContainerID") "Acces value containerID Job check"
            Expect.equal (myJob2 |> Job.tryGetContiguous) (Some true) "Acces value contiguous Job check"
            Expect.equal (myJob2 |> Job.tryGetSpezializedCores) (Some 3) "Acces value spezializedCores Job check"
            Expect.equal (myJob2 |> Job.tryGetCoresPerSocket) (Some 2) "Acces value coresPerSocket Job check"
            Expect.equal (myJob2 |> Job.tryGetCPUsPerGPU) (Some 7) "Acces value CPUsPerGPU Job check"
            Expect.equal (myJob2 |> Job.tryGetDelayBoot) (Some 8) "Acces value delayBoot Job check"
            Expect.equal (myJob2 |> Job.tryGetExclude) (Some ["Hey Exclude Me"]) "Acces value exclude Job check"
            Expect.equal (myJob2 |> Job.tryGetExtra) (Some "Hey I'm Extra") "Acces value extra Job check"
            Expect.equal (myJob2 |> Job.tryGetGroupID) (Some "CSB") "Acces value groupID Job check"
            Expect.equal (myJob2 |> Job.tryGetHold) (Some true) "Acces value hold Job check"
            Expect.equal (myJob2 |> Job.tryGetIgnorePBS) (Some true) "Acces value ignorePBS Job check"
            Expect.equal (myJob2 |> Job.tryGetInput) (Some "MyInput") "Acces value input Job check"
            Expect.equal (myJob2 |> Job.tryGetKillOnInvalidDep) (Some true) "Acces value killOnInvalidDep Job check"
            Expect.equal (myJob2 |> Job.tryGetMailUser) (Some "example@type.de") "Acces value mailUser Job check"
            Expect.equal (myJob2 |> Job.tryGetMemoryPerGPU) (Some "30gb") "Acces value memoryPerGPU Job check"
            Expect.equal (myJob2 |> Job.tryGetMemoryPerCPU) (Some "20gb") "Acces value memoryPerCPU Job check"
            Expect.equal (myJob2 |> Job.tryGetMinCPUs) (Some 9) "Acces value minCPUs Job check"
            Expect.equal (myJob2 |> Job.tryGetNoRequeue) (Some true) "Acces value noRequeue Job check"
            Expect.equal (myJob2 |> Job.tryGetNodeFile) (Some "MyNodeFile") "Acces value nodeFile Job check"
            Expect.equal (myJob2 |> Job.tryGetNTasksPerCore) (Some 10) "Acces value nTasksPerCore Job check"
            Expect.equal (myJob2 |> Job.tryGetNTasksPerGPU) (Some 20) "Acces value nTasksPerGPU Job check"
            Expect.equal (myJob2 |> Job.tryGetNTasksPerNode) (Some 23) "Acces value nTasksPerNode Job check"
            Expect.equal (myJob2 |> Job.tryGetNTasksPerSocket) (Some 1337) "Acces value nTasksPerSocket Job check"
            Expect.equal (myJob2 |> Job.tryGetOvercommit) (Some true) "Acces value overcommit Job check"
            Expect.equal (myJob2 |> Job.tryGetOversubscribe) (Some true) "Acces value oversubscribe Job check"
            Expect.equal (myJob2 |> Job.tryGetPrefer) (Some "ThisCPU ThisNode") "Acces value prefer Job check"
            Expect.equal (myJob2 |> Job.tryGetQuiet) (Some true) "Acces value quiet Job check"
            Expect.equal (myJob2 |> Job.tryGetReboot) (Some true) "Acces value reboot Job check"
            Expect.equal (myJob2 |> Job.tryGetRequeue) (Some true) "Acces value requeue Job check"
            Expect.equal (myJob2 |> Job.tryGetReservation) (Some "Here There") "Acces value reservation Job check"
            Expect.equal (myJob2 |> Job.tryGetSocketsPerNode) (Some 69) "Acces value socketsPerNode Job check"
            Expect.equal (myJob2 |> Job.tryGetTestOnly) (Some true) "Acces value testOnly Job check"
            Expect.equal (myJob2 |> Job.tryGetThreadSpec) (Some 42) "Acces value threadSpec Job check"
            Expect.equal (myJob2 |> Job.tryGetThreadsPerCore) (Some 2323) "Acces value threadSpec Job check"
            Expect.equal (myJob2 |> Job.tryGetMinTime) (Some (1,2,3,4) ) "Acces value minTime Job check"
            Expect.equal (myJob2 |> Job.tryGetUserID) (Some "MyUserID") "Acces value userID Job check"
            Expect.equal (myJob2 |> Job.tryGetUseMinNodes) (Some true) "Acces value useMinNodes Job check"
            Expect.equal (myJob2 |> Job.tryGetVerbose) (Some true) "Acces value verbose Job check"
            Expect.equal (myJob2 |> Job.tryGetWait) (Some true) "Acces value wait Job check"
            Expect.equal (myJob2 |> Job.tryGetWaitAllNodes) (Some true) "Acces value waitAllNodes Job check"
            Expect.equal (myJob2 |> Job.tryGetWrap) (Some "Wrap Burrito Taco") "Acces value wrap Job check"

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

  
