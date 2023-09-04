namespace Slurmi

open DynamicObj
open System.Runtime.InteropServices
open Fli
open Process
open Environment



module Job =
    let ifOnlyOneDigit (digit:int) =
        if digit < 10 then
            "0" + digit.ToString()
        else
            digit.ToString()

    let formatTime (digits:(int*int*int*int))=
        // let (a,b,c,d) = digits
        let (days,hours,minutes,seconds) = digits
        let po = $"{ifOnlyOneDigit days}-{ifOnlyOneDigit hours}:{ifOnlyOneDigit minutes}:{ifOnlyOneDigit seconds}"
        po

    type Job(jobName: string,processList:(string*string list)list) =
        inherit DynamicObj()

        /// Job Name
        member val Name = jobName with get, set
    
        member val Processes = processList with get,set 

        static member SetJobID
            ([<Optional; DefaultParameterValue(null)>]?JobID: int) =
                (fun (job: Job) ->

                    JobID |> DynObj.setValueOpt job "jobid"
                    job
                )

        static member tryGetJobID (job: Job) =
            job.TryGetValue "jobid"
        /// Node count required for the job.
        static member SetNode
            ([<Optional; DefaultParameterValue(null)>]?Node: string) =
                (fun (job: Job) ->

                    Node |> DynObj.setValueOpt job "node"
                    job
                )

        static member tryGetNode (job: Job) =
            job.TryGetValue "node"
        /// File in which to store job output.
        static member SetOutput
            ([<Optional; DefaultParameterValue(null)>]?Output: string) =
                (fun (job: Job) ->
                    Output |> DynObj.setValueOpt job "output"
                    job
                )

        static member tryGetOutput (job: Job) =
            job.TryGetValue "output"

        /// File in which to store job error messages.
        static member SetError
            ([<Optional; DefaultParameterValue(null)>]?Error: string) =
                (fun (job: Job) ->
                    Error |> DynObj.setValueOpt job "error"
                    job
                )

        static member tryGetError (job: Job) =
            job.TryGetValue "error"
        /// Wall clock time limit.
        static member SetTime
            ([<Optional; DefaultParameterValue(null)>]?Time:(int*int*int*int)) =
                (fun (job: Job) ->
                    Time |> DynObj.setValueOpt job "time"
                    job
                )

        static member tryGetTime (job: Job) =
            job.TryGetValue "time"
        /// Number of tasks to be launched.
        static member SetNTasks
            ([<Optional; DefaultParameterValue(null)>]?NTasks:int) =
                (fun (job: Job) ->
                    NTasks |> DynObj.setValueOpt job "nTasks"
                    job
                )

        static member tryGetNTasks (job: Job) =
            job.TryGetValue "nTasks"

        /// Number of CPUs required per task.
        static member SetCPUsPerTask
            ([<Optional; DefaultParameterValue(null)>]?CPUsPerTask:int) =
                (fun (job: Job) ->
                    CPUsPerTask |> DynObj.setValueOpt job "cpusPerTask"
                    job
                )

        static member tryGetCPUsPerTask (job: Job) =
            job.TryGetValue "cpusPerTask"

        /// Memory required per node (with unit, e.g. "30gb")
        static member SetMemory
            ([<Optional; DefaultParameterValue(null)>]?Memory:string) =
                (fun (job: Job) ->
                    Memory |> DynObj.setValueOpt job "memory"
                    job
                )

        static member tryGetMemory (job: Job) =
            job.TryGetValue "memory"

        /// Partition/queue in which to run the job
        static member SetPartition
            ([<Optional; DefaultParameterValue(null)>]?Partition:string) =
                (fun (job: Job) ->
                    Partition |> DynObj.setValueOpt job "partition"
                    job
                )

        static member tryGetPartition (job: Job) =
            job.TryGetValue "partition"

        static member SetFileName
            ([<Optional; DefaultParameterValue(null)>]?FileName:string) =
                (fun (job: Job) ->
                    FileName |> DynObj.setValueOpt job "fileName"
                    job
                )
        static member tryGetFileName (job: Job) =
            job.TryGetValue "fileName"

        static member SetOutputFile
            ([<Optional; DefaultParameterValue(null)>]?OutputFile:string) =
                (fun (job: Job) ->
                    OutputFile |> DynObj.setValueOpt job "outputFile"
                    job
                )
        static member tryGetOutputFile (job: Job) =
            job.TryGetValue "outputFile"

        static member SetEnvironment
            ([<Optional; DefaultParameterValue(null)>]?Environment:EnvironmentSLURM) =
                (fun (job: Job) ->
                    Environment  |> DynObj.setValueOpt job "environment"
                    job
                )
        static member tryGetEnvironment (job: Job) =
            job.TryGetValue "environment"

        static member SetParsable
            ([<Optional; DefaultParameterValue(null)>]?Parsable:bool) =
                (fun (job: Job) ->
                    Parsable  |> DynObj.setValueOpt job "parsable"
                    job
                )
        static member tryGetParsable (job: Job) =
            job.TryGetValue "parsable"

        static member SetDependency
            ([<Optional; DefaultParameterValue(null)>]?Dependency:TypeOfDep*DependencyType[]) =
                (fun (job: Job) ->
                    Dependency  |> DynObj.setValueOpt job "dependency"
                    job
                )
        static member tryGetDependency (job: Job) =
            job.TryGetValue "dependency"

    let commands (job:Job)=
        [|
            "#!/bin/bash"
            ;
                sprintf "#SBATCH -J %s" job.Name
            ;
                (match (job |> Job.tryGetNode) with
                | (Some value) -> (sprintf "#SBATCH -N %s" (value|> string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetOutput) with
                | (Some value) -> (sprintf "#SBATCH -o %s.out" (value|> string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetError) with
                | (Some value) -> (sprintf "#SBATCH -e %s.err" (value|> string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetTime) with
                | (Some value) -> (sprintf "#SBATCH --time=%s" (formatTime (value:?>(int*int*int*int))))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetNTasks) with
                | (Some value) -> (sprintf "#SBATCH --ntasks=%s" (value|> string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetCPUsPerTask) with
                | (Some value) -> (sprintf "#SBATCH --cpus-per-task=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetMemory) with
                | (Some value) -> (sprintf "#SBATCH --mem=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetPartition) with
                | (Some value) -> (sprintf "#SBATCH -p %s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetEnvironment) with
                | (Some value) -> ((value:?>EnvironmentSLURM).GetEnvironment()) |> List.map (fun x -> String.concat " " [(fst x);(snd x)] ) |> String.concat "\n"
                | (None) -> "")

            ;
                (job.Processes  |> List.map (fun x -> createProcessCall x)) |> String.concat "\n"

        |]
        |> Array.filter (fun x -> x <> "")

    let createJobscript (jobToDo:Job) =
        commands jobToDo
        |> Array.map (fun x ->
            sprintf "%s" x
        )
