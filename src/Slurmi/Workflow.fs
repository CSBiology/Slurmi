namespace Slurmi 

open DynamicObj
open System.Runtime.InteropServices
open Fli 
open Graphoscope
open System.Collections.Generic

module Workflow = 

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

    let matchOutput x = 
        match x with 
        | Some value -> value 
        | None -> failwith "No output"

    let getResultFromCall (command:string) = 
        (callToTerminalCMD (command)).Text
        |> matchOutput

    let getResultFromCallBASH (command:string) = 
        (callToTerminalBash (command)).Text
        |> matchOutput


    type JobWithDep =

        {
            JobInfo : Job
            AllOrAny : TypeOfDep
            DependingOn : (Job*KindOfDependency) list

        }


    let createGraphFromJobList (input: JobWithDep list)= 
        let g = FGraph.empty<string,JobWithDep,KindOfDependency>
        input
        |> List.iter (
            fun jobWithDep ->
                //FGraph.addElement jobWithDep.JobInfo.Name jobWithDep.DependingOn jobWithDep.DependingOn |> ignore
                jobWithDep.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
                jobWithDep.DependingOn 
                |> List.iter (fun (job,dep) -> FGraph.addElement job.Name (input|> List.find (fun x -> x.JobInfo.Name = job.Name)) jobWithDep.JobInfo.Name jobWithDep  dep g |> ignore))
    
        g

    let sendToTerminalAndReceiveJobID (job:JobWithDep)= 
        // job 
        // set parsable 
        job.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
        let res = getResultFromCallBASH (job.JobInfo.formatProcess)
        // submit 
        // get return 
        job.JobInfo |> Job.SetJobID res |> ignore 
        // set as Job ID 


    let sendToTerminalAndReceiveJobIDCMD (job:JobWithDep)= 
        // job 
        // set parsable 
        job.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
        let res = getResultFromCall (job.JobInfo.formatProcess)
        // submit 
        // get return 
        job.JobInfo |> Job.SetJobID res |> ignore 
        // set as Job ID 


    let getNodesWithoutDependencies (graph:FGraph<string,JobWithDep,KindOfDependency>) = 
        graph.Keys
        |> Seq.map (fun x -> ((graph.Item x) |> fun (a,b,c)  -> b), graph.Item x |> (FContext.predecessors) |> Seq.length)
        |> Seq.filter (fun x -> (snd x = 0 ) ) 
        |> Seq.map fst 

    let getPredecessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
        (graph.Item (jobToSearch)) |> FContext.predecessors



    let hasPredecessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
        if (getPredecessors graph jobToSearch |> Seq.length > 0) then true else false


    let getSuccessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
        (graph.Item (jobToSearch)) |> FContext.successors
    let hasSuccessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
        if (getSuccessors graph jobToSearch |> Seq.length > 0) then true else false


    let arePredecessorsWorkedAlready  (graph:FGraph<string,JobWithDep,KindOfDependency>)(jobToLook:string) (workedOn:List<string>) = 
        let predecessors = getPredecessors graph jobToLook
        let predecessorsWorked = predecessors |> Seq.toArray  |> Array.forall (fun x -> workedOn.Contains  (fst x))
        predecessorsWorked

    let getJob (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
        let name,job,kind = graph.[job]
        job

    let getKind (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
        let name,job,kind = graph.[job]
        kind

    let getDependencies (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
        let name,job,kind = graph.[job]
        name

    let getIdFromJobWithDep (jwd:JobWithDep) = 
        jwd.JobInfo |> Job.tryGetJobID

    let getJobId (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string)= 
        let job = getJob graph job
        let jid = job.JobInfo |> Job.tryGetJobID
        jid.Value |> string


    let formatDep (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string) = 
        let deps = (getDependencies graph job) |> Seq.map (fun x ->(KindOfDependency.toString x.Value)+(getJobId graph x.Key)) |> Seq.toArray
        let jobInfo = getJob graph job
        let command = deps |> String.concat (jobInfo.AllOrAny |> TypeOfDep.toString)
        printfn "%A" command
        jobInfo.JobInfo.TwoDashes |> LongCommand.SetDependency command |> ignore

    let checkForWorked (graph:FGraph<string,JobWithDep,KindOfDependency>)(jobToLook:string) (workedOn:List<string>)= 
        if (arePredecessorsWorkedAlready graph jobToLook workedOn) then
        
            if workedOn.Contains jobToLook 
                then printfn "already worked on %A "jobToLook
            else
                printfn "added %A" jobToLook
                let jtwo = getJob graph jobToLook

                formatDep graph jobToLook
                jtwo.JobInfo |> Job.SetJobID (getResultFromCallBASH (jtwo.JobInfo.produceCall)) |> ignore
                
                workedOn.Add jobToLook
        else
            printfn "false"

    let rec checkAllForWorked (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobs:string ) (workedOn:List<string>) = 
        printfn "working on %A rn " jobs
        checkForWorked graph jobs workedOn 
        if (hasSuccessors graph jobs) then 
            let successors = getSuccessors graph jobs
        
            successors |> Seq.iter (fun x -> checkAllForWorked graph (fst x) workedOn)
        else 
            printfn "no successors"






    let formatCallWithDep (job:JobWithDep)= 
            let jobInfo = job.JobInfo
            let localStr = new System.Text.StringBuilder()
            localStr.Append ("#!/bin/bash \n") |> ignore
            localStr.Append ("#SBATCH ") |> ignore
            localStr.AppendFormat (sprintf "-J %s " jobInfo.Name) |> ignore
            localStr.Append (jobInfo.formatOneDash)       |> ignore
            localStr.Append (jobInfo.formatTwoDashes)     |> ignore
            localStr.Append (jobInfo.formatOnlyKey)       |> ignore
            localStr.Append ("\n")           |> ignore
            if jobInfo |> Job.tryGetEnvironment |> Option.isSome then
                localStr.Append (jobInfo.formatEnv)            |> ignore
            localStr.Append jobInfo.formatProcess    |> ignore
            localStr.ToString()


    let submitAll (graph:FGraph<string,JobWithDep,KindOfDependency>) (workedOn:List<string>)=
        let firstNodes = 
            getNodesWithoutDependencies graph
            |> Seq.toArray
        firstNodes |> Array.map (fun x -> checkAllForWorked graph x.JobInfo.Name workedOn) |> ignore