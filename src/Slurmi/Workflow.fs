namespace Slurmi 

open Fli 
open Graphoscope
open System.Collections.Generic
open SshNet
open Renci.SshNet
open Renci.SshNet.Common
//open Connection 

module Workflow = 
    type JobWithDep =

        {
            JobInfo : Job
            AllOrAny : TypeOfDep
            DependingOn : (Job*KindOfDependency) list

        }

    type WFGraph = 
        {
            Graph : FGraph<string,JobWithDep,KindOfDependency>
        }
        static member  createGraphFromJobList (input: JobWithDep list)= 
            let g = FGraph.empty<string,JobWithDep,KindOfDependency>
            input
            |> List.iter (
                fun jobWithDep ->
                    //FGraph.addElement jobWithDep.JobInfo.Name jobWithDep.DependingOn jobWithDep.DependingOn |> ignore
                    //jobWithDep.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
                    jobWithDep.DependingOn 
                    |> List.iter (fun (job,dep) -> FGraph.addElement job.Name (input|> List.find (fun x -> x.JobInfo.Name = job.Name)) jobWithDep.JobInfo.Name jobWithDep  dep g |> ignore))
    
            g
        static member getNodesWithoutDependencies (graph:FGraph<string,JobWithDep,KindOfDependency>) = 
            graph.Keys
            |> Seq.map (fun x -> ((graph.Item x) |> fun (a,b,c)  -> b), graph.Item x |> (FContext.predecessors) |> Seq.length)
            |> Seq.filter (fun x -> (snd x = 0 ) ) 
            |> Seq.map fst 

        static member getPredecessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            (graph.Item (jobToSearch)) |> FContext.predecessors

        static member hasPredecessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            if (WFGraph.getPredecessors graph jobToSearch |> Seq.length > 0) then true else false


        static member getSuccessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            (graph.Item (jobToSearch)) |> FContext.successors

        static member hasSuccessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            if (WFGraph.getSuccessors graph jobToSearch |> Seq.length > 0) then true else false


        static member arePredecessorsWorkedAlready  (graph:FGraph<string,JobWithDep,KindOfDependency>)(jobToLook:string) (workedOn:List<string>) = 
            let predecessors = WFGraph.getPredecessors graph jobToLook
            let predecessorsWorked = predecessors |> Seq.toArray  |> Array.forall (fun x -> workedOn.Contains  (fst x))
            predecessorsWorked

        static member getJob (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
            let name,job,kind = graph.[job]
            job

        static member getKind (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
            let name,job,kind = graph.[job]
            kind

        static member getDependencies (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
            let name,job,kind = graph.[job]
            name

        static member getIdFromJobWithDep (jwd:JobWithDep) = 
            jwd.JobInfo |> Job.tryGetJobID

        static member getJobId (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string)= 
            let job = WFGraph.getJob graph job
            let jid = job.JobInfo |> Job.tryGetJobID
            jid.Value |> string


        static member formatDep (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string) = 
            let deps = (WFGraph.getDependencies graph job) |> Seq.map (fun x ->(KindOfDependency.toString x.Value)+(WFGraph.getJobId graph x.Key)) |> Seq.toArray
            let jobInfo = WFGraph.getJob graph job
            let command = deps |> String.concat (jobInfo.AllOrAny |> TypeOfDep.toString) |> String.filter (fun x -> x <> ' ' ) |> String.filter (fun x -> x <> '\n')
            printfn "%A" command
            if command = "" then
                ()
            else 
                jobInfo.JobInfo.CommandWithArgument |> Command.SetDependency command |> ignore



    type Workflow = 
        {
            Jobs :  JobWithDep list
            Graph : WFGraph
        }

    //    member private WFGraph.callToTerminalCMD (command:string) = 
    //        let processResponse = 
    //            cli {
    //                Shell CMD
    //                Command command
    //            }
    //            |> Command.execute
    //        processResponse
    //    member private WFGraph.callToTerminalBash (command:string) = 
    //        let processResponse = 
    //            cli {
    //                Shell BASH
    //                Command command
    //            }
    //            |> Command.execute
    //        processResponse

    //    member private WFGraph. matchOutput x = 
    //        match x with 
    //        | Some value -> value 
    //        | None -> failwith "No output"

    //    member WFGraph.getResultFromCallCMD (command:string) = 
    //        (WFGraph.callToTerminalCMD (command)).Text
    //        |> WFGraph.matchOutput

    //    member WFGraph.getResultFromCallBash (command:string) = 
    //        (WFGraph.callToTerminalBash (command)).Text
    //        |> WFGraph.matchOutput


    //    member WFGraph.sendToTerminalAndReceiveJobIDBash (job:JobWithDep)= 
    //        // job 
    //        // set parsable 
    //        //job.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
    //        let res = WFGraph.getResultFromCallBash (job.JobInfo.formatProcess)
    //        // submit 
    //        // get return 
    //        job.JobInfo |> Job.SetJobID res |> ignore 
    //        // set as Job ID 


    //    member WFGraph.sendToTerminalAndReceiveJobIDCMD (job:JobWithDep)= 
    //        // job 
    //        // set parsable 
    //        //job.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
    //        let res = WFGraph.getResultFromCallCMD (job.JobInfo.formatProcess)
    //        // submit 
    //        // get return 
    //        job.JobInfo |> Job.SetJobID res |> ignore 
    //        // set as Job ID 

    let createWorkflow (jobList: JobWithDep list) = 
        {
            Jobs = jobList
            //Graph =  WFGraph.createGraphFromJobList jobList
            Graph =  
                {
                    Graph = WFGraph.createGraphFromJobList jobList
                }
        }
