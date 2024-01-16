namespace Slurmi 

module Runner = 
    open Fli 
    open Graphoscope
    open System.Collections.Generic
    open SshNet
    open Renci.SshNet
    open Renci.SshNet.Common
    open Workflow 
    //open Connection 
    // do something 


    let matchOutput (output) = 
        match output with 
        | Some value -> value 
        | None -> failwith "No output"

    let callToTerminalCMD (command:string) = 
        let processResponse = 
            cli {
                Shell CMD
                Command command
            }
            |> Command.execute
        // processResponse.Text
        processResponse
    let callToTerminalBash (command:string) = 
        let processResponse = 
            cli {
                Shell BASH
                Command command
            }
            |> Command.execute
        // processResponse.Text
        processResponse

    let getResultFromCallCMD (command:string) = 
        (callToTerminalCMD (command)).Text
        |> matchOutput

    let getResultFromCallBash (command:string) = 
        (callToTerminalBash (command)).Text
        |> matchOutput

    let sendToTerminalAndReceiveJobIDBash (job:Job)= 
        //job.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
        let res = getResultFromCallBash (job.formatProcess)
        // submit 
        // get return 
        // set as Job ID 
        job |> Job.SetJobID res |> ignore 
        

    let sendToTerminalAndReceiveJobIDCMD (job:Job)= 
        // job 
        // set parsable 
        //job.OnlyKey |> OnlyKey.SetParsable true |> ignore

        let res = getResultFromCallCMD (job.formatProcess)
        // submit 
        // get return 
        // set as Job ID 
        job |> Job.SetJobID res |> ignore 
        
    let checkForWorkedBash (graph:FGraph<string,JobWithDep,KindOfDependency>)(jobToLook:string) (workedOn:List<string>)= 
        if (WFGraph.arePredecessorsWorkedAlready graph jobToLook workedOn) then
        
            if workedOn.Contains jobToLook then
                //then printfn "already worked on %A "jobToLook
                ()
            else
                //printfn "added %A" jobToLook
                let jtwo = WFGraph.getJob graph jobToLook

                WFGraph.formatDep graph jobToLook
                jtwo.JobInfo |> Job.SetJobID (getResultFromCallBash (jtwo.JobInfo.produceCall)) |> ignore
                
                workedOn.Add jobToLook
        else
            //printfn "false"
            ()
        
    let sshToTerminal (client:SshClient) (job:Job)= 
        client.RunCommand(job.produceCall).Result

    let checkForWorkedBashSSH (graph:FGraph<string,JobWithDep,KindOfDependency>)(jobToLook:string) (workedOn:List<string>) (client: SshClient)= 
        if (WFGraph.arePredecessorsWorkedAlready graph jobToLook workedOn) then
        
            if workedOn.Contains jobToLook then
                //then printfn "already worked on %A "jobToLook
                ()
            else
                //printfn "added %A" jobToLook
                let jtwo = WFGraph.getJob graph jobToLook

                WFGraph.formatDep graph jobToLook
                jtwo.JobInfo |> Job.SetJobID (sshToTerminal client (jtwo.JobInfo)) |> ignore
                
                workedOn.Add jobToLook
        else
            //printfn "false"
            ()


    let rec checkAllForWorkedSSH (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobs:string ) (workedOn:List<string>)  (client: SshClient)= 
        checkForWorkedBashSSH graph jobs workedOn client
        if (WFGraph.hasSuccessors graph jobs) then 
            let successors = WFGraph.getSuccessors graph jobs
        
            successors |> Seq.iter (fun x -> checkAllForWorkedSSH graph (fst x) workedOn client)
        else 
            ()

    let rec checkAllForWorked (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobs:string ) (workedOn:List<string>)  = 
        checkForWorkedBash graph jobs workedOn 
        if (WFGraph.hasSuccessors graph jobs) then 
            let successors = WFGraph.getSuccessors graph jobs
        
            successors |> Seq.iter (fun x -> checkAllForWorked graph (fst x) workedOn)
        else 
            ()


    let submitAll (graph:FGraph<string,JobWithDep,KindOfDependency>) (workedOn:List<string>)=
        let firstNodes = 
            WFGraph.getNodesWithoutDependencies graph
            |> Seq.toArray
        firstNodes |> Array.map (fun x -> checkAllForWorked graph x.JobInfo.Name workedOn) |> ignore
    
    let submitAllSSH (graph:FGraph<string,JobWithDep,KindOfDependency>) (workedOn:List<string>) (client:SshClient)=
        let firstNodes = 
            WFGraph.getNodesWithoutDependencies graph
            |> Seq.toArray
        firstNodes |> Array.map (fun x ->  checkAllForWorkedSSH graph x.JobInfo.Name workedOn client) |> ignore