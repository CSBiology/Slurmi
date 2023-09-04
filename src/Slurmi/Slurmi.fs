namespace Slurmi 

open DynamicObj
open System.Runtime.InteropServices
open Fli
open Workflow
open Job


module Slurmi =

    let findDependencyName (name:string) (jobArray:Job array )= 
        let id =
            jobArray
            |> Array.findIndex (fun x -> x.Name = name)
        let name = $"$Job{id}"
        name


    let getDependency (job:Job)= 
        (match (job |> Job.tryGetDependency) with
        | (Some value) ->  "--dependency="+(DependencyType.concat (fst (value:?>(TypeOfDep*DependencyType[]))) (snd (value:?>(TypeOfDep*DependencyType[]))))
        | (None) -> "")
    
    let createNamesForWF (jobs:Job array)= 
        jobs
        |> Array.mapi (fun i x -> ($"Job{i}",x))

    let formatWF (wfJob:(string*Job)) = 
        let jId = fst wfJob
        let job = snd wfJob
        let parsable = 
                (match (job |> Job.tryGetParsable) with
                | (Some value) -> (sprintf "--parsable")
                | (None) -> "")
        let dep: string =
            getDependency job 
        let fullString = 
            $"{jId}=$(sbatch {parsable} {dep} {job.Name})"
        fullString

    let createWorkflowFromJobArray (jobs:Job array) = 
        let jobNames = createNamesForWF jobs
    
        let parsableJobs = jobNames |> Array.map (fun x -> fst x,x |> snd |> Job.SetParsable true )

        let jobString = parsableJobs |> Array.map (fun x -> formatWF x)
        jobString

    let createWorkflowFromWFType (wf:Workflow) = 
        let jobs = wf.Jobs
        let jobNames = createNamesForWF jobs
    
        let parsableJobs = jobNames |> Array.map (fun x -> fst x,x |> snd |> Job.SetParsable true )

        let jobString = parsableJobs |> Array.map (fun x -> formatWF x)
    
        let textToDisplay = 
            [|
                    "#!/bin/bash"
                ;
                    (match (wf |> Workflow.tryGetTime) with
                    | (Some value) -> (sprintf "#SBATCH --time=%s" (formatTime (value:?>(int*int*int*int))))
                    | (None) -> "")
                ;
                    (match (wf |> Workflow.tryGetPartition) with
                    | (Some value) -> (sprintf "#SBATCH -p %s" (value:?>string))
                    | (None) -> "")

            |]
            |> Array.filter (fun x -> x <> "" || x <> " ")

        jobString |> Array.append textToDisplay
