namespace Slurmi 

open Fli 
open Graphoscope
open System.Collections.Generic

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
                    jobWithDep.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
                    jobWithDep.DependingOn 
                    |> List.iter (fun (job,dep) -> FGraph.addElement job.Name (input|> List.find (fun x -> x.JobInfo.Name = job.Name)) jobWithDep.JobInfo.Name jobWithDep  dep g |> ignore))
    
            g
        static member getNodesWithoutDependencies (graph:FGraph<string,JobWithDep,KindOfDependency>) = 
            graph.Keys
            |> Seq.map (fun x -> ((graph.Item x) |> fun (a,b,c)  -> b), graph.Item x |> (FContext.predecessors) |> Seq.length)
            |> Seq.filter (fun x -> (snd x = 0 ) ) 
            |> Seq.map fst 

        member this.getPredecessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            (graph.Item (jobToSearch)) |> FContext.predecessors

        member this.hasPredecessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            if (this.getPredecessors graph jobToSearch |> Seq.length > 0) then true else false


        member this.getSuccessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            (graph.Item (jobToSearch)) |> FContext.successors

        member this.hasSuccessors (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobToSearch:string) = 
            if (this.getSuccessors graph jobToSearch |> Seq.length > 0) then true else false


        member this.arePredecessorsWorkedAlready  (graph:FGraph<string,JobWithDep,KindOfDependency>)(jobToLook:string) (workedOn:List<string>) = 
            let predecessors = this.getPredecessors graph jobToLook
            let predecessorsWorked = predecessors |> Seq.toArray  |> Array.forall (fun x -> workedOn.Contains  (fst x))
            predecessorsWorked

        member this.getJob (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
            let name,job,kind = graph.[job]
            job

        member this.getKind (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
            let name,job,kind = graph.[job]
            kind

        member this.getDependencies (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string )= 
            let name,job,kind = graph.[job]
            name

        member this.getIdFromJobWithDep (jwd:JobWithDep) = 
            jwd.JobInfo |> Job.tryGetJobID

        member this.getJobId (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string)= 
            let job = this.getJob graph job
            let jid = job.JobInfo |> Job.tryGetJobID
            jid.Value |> string


        member this.formatDep (graph:FGraph<string,JobWithDep,KindOfDependency>) (job:string) = 
            let deps = (this.getDependencies graph job) |> Seq.map (fun x ->(KindOfDependency.toString x.Value)+(this.getJobId graph x.Key)) |> Seq.toArray
            let jobInfo = this.getJob graph job
            let command = deps |> String.concat (jobInfo.AllOrAny |> TypeOfDep.toString)
            //printfn "%A" command
            jobInfo.JobInfo.TwoDashes |> LongCommand.SetDependency command |> ignore

        member this.checkForWorkedBash (graph:FGraph<string,JobWithDep,KindOfDependency>)(jobToLook:string) (workedOn:List<string>)= 
            if (this.arePredecessorsWorkedAlready graph jobToLook workedOn) then
        
                if workedOn.Contains jobToLook then
                    //then printfn "already worked on %A "jobToLook
                    ()
                else
                    //printfn "added %A" jobToLook
                    let jtwo = this.getJob graph jobToLook

                    this.formatDep graph jobToLook
                    jtwo.JobInfo |> Job.SetJobID (jtwo.JobInfo.getResultFromCallBash (jtwo.JobInfo.produceCall)) |> ignore
                
                    workedOn.Add jobToLook
            else
                //printfn "false"
                ()

        member this.checkAllForWorked (graph:FGraph<string,JobWithDep,KindOfDependency>) (jobs:string ) (workedOn:List<string>) = 
            this.checkForWorkedBash graph jobs workedOn 
            if (this.hasSuccessors graph jobs) then 
                let successors = this.getSuccessors graph jobs
        
                successors |> Seq.iter (fun x -> this.checkAllForWorked graph (fst x) workedOn)
            else 
                ()


        member this.submitAll (graph:FGraph<string,JobWithDep,KindOfDependency>) (workedOn:List<string>)=
            let firstNodes = 
                WFGraph.getNodesWithoutDependencies graph
                |> Seq.toArray
            firstNodes |> Array.map (fun x -> this.checkAllForWorked graph x.JobInfo.Name workedOn) |> ignore
    
    type Workflow = 
        {
            Jobs :  JobWithDep list
            Graph : WFGraph
        }

        member private this.callToTerminalCMD (command:string) = 
            let processResponse = 
                cli {
                    Shell CMD
                    Command command
                }
                |> Command.execute
            processResponse
        member private this.callToTerminalBash (command:string) = 
            let processResponse = 
                cli {
                    Shell BASH
                    Command command
                }
                |> Command.execute
            processResponse

        member private this. matchOutput x = 
            match x with 
            | Some value -> value 
            | None -> failwith "No output"

        member this.getResultFromCallCMD (command:string) = 
            (this.callToTerminalCMD (command)).Text
            |> this.matchOutput

        member this.getResultFromCallBash (command:string) = 
            (this.callToTerminalBash (command)).Text
            |> this.matchOutput


        member this.sendToTerminalAndReceiveJobIDBash (job:JobWithDep)= 
            // job 
            // set parsable 
            job.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
            let res = this.getResultFromCallBash (job.JobInfo.formatProcess)
            // submit 
            // get return 
            job.JobInfo |> Job.SetJobID res |> ignore 
            // set as Job ID 


        member this.sendToTerminalAndReceiveJobIDCMD (job:JobWithDep)= 
            // job 
            // set parsable 
            job.JobInfo.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
            let res = this.getResultFromCallCMD (job.JobInfo.formatProcess)
            // submit 
            // get return 
            job.JobInfo |> Job.SetJobID res |> ignore 
            // set as Job ID 

    let createWorkflow (jobList: JobWithDep list) = 
        {
            Jobs = jobList
            //Graph =  WFGraph.createGraphFromJobList jobList
            Graph =  
                {
                    Graph = WFGraph.createGraphFromJobList jobList
                }
        }
