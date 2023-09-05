namespace Slurmi

open DynamicObj
open System.Runtime.InteropServices
open Fli
open Process


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

    static member SetAccount
        ([<Optional; DefaultParameterValue(null)>]?Account:string) =
            (fun (job: Job) ->
                Account  |> DynObj.setValueOpt job "account"
                job
            )
    static member tryGetAccount (job: Job) =
        job.TryGetValue "account"

    static member SetBatch
        ([<Optional; DefaultParameterValue(null)>]?Batch:string list) =
            (fun (job: Job) ->
                Batch  |> DynObj.setValueOpt job "batch"
                job
            )
    static member tryGetBatch (job: Job) =
        job.TryGetValue "batch"

    static member SetBurstBufferSpecification
        ([<Optional; DefaultParameterValue(null)>]?BurstBufferSpecification:string list) =
            (fun (job: Job) ->
                BurstBufferSpecification  |> DynObj.setValueOpt job "bb"
                job
            )
    static member tryGetBurstBufferSpecification (job: Job) =
        job.TryGetValue "bb"
    static member SetBurstBufferSpecificationFilePath
        ([<Optional; DefaultParameterValue(null)>]?BurstBufferSpecificationFilePath:string) =
            (fun (job: Job) ->
                BurstBufferSpecificationFilePath  |> DynObj.setValueOpt job "bbf"
                job
            )
    static member tryGetBurstBufferSpecificationFilePath (job: Job) =
        job.TryGetValue "bbf"

    static member SetWorkingDirectory
        ([<Optional; DefaultParameterValue(null)>]?WorkingDirectory:string) =
            (fun (job: Job) ->
                WorkingDirectory  |> DynObj.setValueOpt job "workingdirectory"
                job
            )
    static member tryGetWorkingDirectory (job: Job) =
        job.TryGetValue "workingdirectory"

    static member SetClusters
        ([<Optional; DefaultParameterValue(null)>]?Clusters:string list) =
            (fun (job: Job) ->
                Clusters  |> DynObj.setValueOpt job "clusters"
                job
            )
    static member tryGetClusters (job: Job) =
        job.TryGetValue "clusters"

    static member SetComment
        ([<Optional; DefaultParameterValue(null)>]?Comment:string) =
            (fun (job: Job) ->
                Comment  |> DynObj.setValueOpt job "comment"
                job
            )
    static member tryGetComment (job: Job) =
        job.TryGetValue "comment"

    static member SetContainer
        ([<Optional; DefaultParameterValue(null)>]?Container:string) =
            (fun (job: Job) ->
                Container  |> DynObj.setValueOpt job "container"
                job
            )
    static member tryGetContainer (job: Job) =
        job.TryGetValue "container"

    static member SetContainerID
        ([<Optional; DefaultParameterValue(null)>]?ContainerID:string) =
            (fun (job: Job) ->
                ContainerID  |> DynObj.setValueOpt job "containerID"
                job
            )
    static member tryGetContainerID (job: Job) =
        job.TryGetValue "containerID"

    static member SetContiguous
        ([<Optional; DefaultParameterValue(null)>]?Contiguous:bool) =
            (fun (job: Job) ->
                Contiguous  |> DynObj.setValueOpt job "contiguous"
                job
            )
    static member tryGetContiguous (job: Job) =
        job.TryGetValue "contiguous"

    static member SetSpezializedCores
        ([<Optional; DefaultParameterValue(null)>]?SpezializedCores:int) =
            (fun (job: Job) ->
                SpezializedCores  |> DynObj.setValueOpt job "spezCore"
                job
            )
    static member tryGetSpezializedCores (job: Job) =
        job.TryGetValue "spezCore"

    static member SetCoresPerSocket
        ([<Optional; DefaultParameterValue(null)>]?CoresPerSocket:int) =
            (fun (job: Job) ->
                CoresPerSocket  |> DynObj.setValueOpt job "coresPerSocket"
                job
            )
    static member tryGetCoresPerSocket (job: Job) =
        job.TryGetValue "coresPerSocket"

    static member SetCPUsPerGPU
        ([<Optional; DefaultParameterValue(null)>]?CPUsPerGPU:int) =
            (fun (job: Job) ->
                CPUsPerGPU  |> DynObj.setValueOpt job "cpuPerGPU"
                job
            )
    static member tryGetCPUsPerGPU (job: Job) =
        job.TryGetValue "cpuPerGPU"

    static member SetDelayBoot
        ([<Optional; DefaultParameterValue(null)>]?DelayBoot:int) =
            (fun (job: Job) ->
                DelayBoot  |> DynObj.setValueOpt job "delayBoot"
                job
            )
    static member tryGetDelayBoot (job: Job) =
        job.TryGetValue "delayBoot"


    static member SetExclude
        ([<Optional; DefaultParameterValue(null)>]?Exclude:string list) =
            (fun (job: Job) ->
                Exclude  |> DynObj.setValueOpt job "exclude"
                job
            )
    static member tryGetExclude (job: Job) =
        job.TryGetValue "exclude"

    static member SetExtra
        ([<Optional; DefaultParameterValue(null)>]?Extra:string) =
            (fun (job: Job) ->
                Extra  |> DynObj.setValueOpt job "extra"
                job
            )
    static member tryGetExtra (job: Job) =
        job.TryGetValue "extra"

    static member SetGroupID
        ([<Optional; DefaultParameterValue(null)>]?GroupID:string) =
            (fun (job: Job) ->
                GroupID  |> DynObj.setValueOpt job "gid"
                job
            )
    static member tryGetGroupID (job: Job) =
        job.TryGetValue "gid"

    static member SetHold
        ([<Optional; DefaultParameterValue(null)>]?Hold:bool) =
            (fun (job: Job) ->
                Hold  |> DynObj.setValueOpt job "hold"
                job
            )
    static member tryGetHold (job: Job) =
        job.TryGetValue "hold"

    static member SetIgnorePBS
        ([<Optional; DefaultParameterValue(null)>]?IgnorePBS:bool) =
            (fun (job: Job) ->
                IgnorePBS  |> DynObj.setValueOpt job "ignorePBS"
                job
            )
    static member tryGetIgnorePBS (job: Job) =
        job.TryGetValue "ignorePBS"

    static member SetInput
        ([<Optional; DefaultParameterValue(null)>]?Input:string) =
            (fun (job: Job) ->
                Input  |> DynObj.setValueOpt job "input"
                job
            )
    static member tryGetInput (job: Job) =
        job.TryGetValue "input"

    static member SetKillOnInvalidDep
        ([<Optional; DefaultParameterValue(null)>]?KillOnInvalidDep:bool) =
            (fun (job: Job) ->
                KillOnInvalidDep  |> DynObj.setValueOpt job "killOnInvalidDep"
                job
            )
    static member tryGetKillOnInvalidDep (job: Job) =
        job.TryGetValue "killOnInvalidDep"

    static member SetMailUser
        ([<Optional; DefaultParameterValue(null)>]?MailUser:string) =
            (fun (job: Job) ->
                MailUser  |> DynObj.setValueOpt job "mailUser"
                job
            )
    static member tryGetMailUser (job: Job) =
        job.TryGetValue "mailUser"

    static member SetMemoryPerGPU
        ([<Optional; DefaultParameterValue(null)>]?MemoryPerGPU:string) =
            (fun (job: Job) ->
                MemoryPerGPU |> DynObj.setValueOpt job "memoryPerGPU"
                job
            )

    static member tryGetMemoryPerGPU (job: Job) =
        job.TryGetValue "memoryPerGPU"

    static member SetMemoryPerCPU
        ([<Optional; DefaultParameterValue(null)>]?MemoryPerCPU:string) =
            (fun (job: Job) ->
                MemoryPerCPU |> DynObj.setValueOpt job "memoryPerCPU"
                job
            )

    static member tryGetMemoryPerCPU (job: Job) =
        job.TryGetValue "memoryPerCPU"


    static member SetMinCPUs
        ([<Optional; DefaultParameterValue(null)>]?MinCPUs:int) =
            (fun (job: Job) ->
                MinCPUs  |> DynObj.setValueOpt job "minCPUs"
                job
            )
    static member tryGetMinCPUs (job: Job) =
        job.TryGetValue "minCPUs"

    static member SetNoRequeue
        ([<Optional; DefaultParameterValue(null)>]?NoRequeue:bool) =
            (fun (job: Job) ->
                NoRequeue  |> DynObj.setValueOpt job "noRequeue"
                job
            )
    static member tryGetNoRequeue (job: Job) =
        job.TryGetValue "noRequeue"

    static member SetNodeFile
        ([<Optional; DefaultParameterValue(null)>]?NodeFile:string) =
            (fun (job: Job) ->
                NodeFile  |> DynObj.setValueOpt job "nodeFile"
                job
            )
    static member tryGetNodeFile (job: Job) =
        job.TryGetValue "nodeFile"

    static member SetNTasksPerCore
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerCore:int) =
            (fun (job: Job) ->
                NTasksPerCore  |> DynObj.setValueOpt job "nTasksPerCore"
                job
            )
    static member tryGetNTasksPerCore (job: Job) =
        job.TryGetValue "nTasksPerCore"

    static member SetNTasksPerGPU
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerGPU:int) =
            (fun (job: Job) ->
                NTasksPerGPU  |> DynObj.setValueOpt job "nTasksPerGPU"
                job
            )
    static member tryGetNTasksPerGPU (job: Job) =
        job.TryGetValue "nTasksPerGPU"

    static member SetNTasksPerNode
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerNode:int) =
            (fun (job: Job) ->
                NTasksPerNode  |> DynObj.setValueOpt job "nTasksPerNode"
                job
            )
    static member tryGetNTasksPerNode (job: Job) =
        job.TryGetValue "nTasksPerNode"

    static member SetNTasksPerSocket
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerSocket:int) =
            (fun (job: Job) ->
                NTasksPerSocket  |> DynObj.setValueOpt job "nTasksPerSocket"
                job
            )
    static member tryGetNTasksPerSocket (job: Job) =
        job.TryGetValue "nTasksPerSocket"

    static member SetOvercommit
        ([<Optional; DefaultParameterValue(null)>]?Overcommit:bool) =
            (fun (job: Job) ->
                Overcommit  |> DynObj.setValueOpt job "overcommit"
                job
            )
    static member tryGetOvercommit (job: Job) =
        job.TryGetValue "overcommit"

    static member SetOversubscribe
        ([<Optional; DefaultParameterValue(null)>]?Oversubscribe:bool) =
            (fun (job: Job) ->
                Oversubscribe  |> DynObj.setValueOpt job "oversubscribe"
                job
            )
    static member tryGetOversubscribe (job: Job) =
        job.TryGetValue "oversubscribe"


    static member SetPrefer
        ([<Optional; DefaultParameterValue(null)>]?Prefer:string list) =
            (fun (job: Job) ->
                Prefer  |> DynObj.setValueOpt job "prefer"
                job
            )
    static member tryGetPrefer (job: Job) =
        job.TryGetValue "prefer"

    static member SetQuiet
        ([<Optional; DefaultParameterValue(null)>]?Quiet:bool) =
            (fun (job: Job) ->
                Quiet  |> DynObj.setValueOpt job "quiet"
                job
            )
    static member tryGetQuiet (job: Job) =
        job.TryGetValue "quiet"

    static member SetReboot
        ([<Optional; DefaultParameterValue(null)>]?Reboot:bool) =
            (fun (job: Job) ->
                Reboot  |> DynObj.setValueOpt job "reboot"
                job
            )
    static member tryGetReboot (job: Job) =
        job.TryGetValue "reboot"

    static member SetRequeue
        ([<Optional; DefaultParameterValue(null)>]?Requeue:bool) =
            (fun (job: Job) ->
                Requeue  |> DynObj.setValueOpt job "requeue"
                job
            )
    static member tryGetRequeue (job: Job) =
        job.TryGetValue "requeue"

    static member SetReservation
        ([<Optional; DefaultParameterValue(null)>]?Reservation:string list) =
            (fun (job: Job) ->
                Reservation  |> DynObj.setValueOpt job "reservation"
                job
            )
    static member tryGetReservation (job: Job) =
        job.TryGetValue "reservation"

    static member SetSocketsPerNode
        ([<Optional; DefaultParameterValue(null)>]?SocketsPerNode:int) =
            (fun (job: Job) ->
                SocketsPerNode  |> DynObj.setValueOpt job "socketsPerNode"
                job
            )
    static member tryGetSocketsPerNode (job: Job) =
        job.TryGetValue "socketsPerNode"

    static member SetSpreadJob
        ([<Optional; DefaultParameterValue(null)>]?SpreadJob:bool) =
            (fun (job: Job) ->
                SpreadJob  |> DynObj.setValueOpt job "spreadJob"
                job
            )
    static member tryGetSpreadJob (job: Job) =
        job.TryGetValue "spreadJob"

    static member SetTestOnly
        ([<Optional; DefaultParameterValue(null)>]?TestOnly:bool) =
            (fun (job: Job) ->
                TestOnly  |> DynObj.setValueOpt job "testOnly"
                job
            )
    static member tryGetTestOnly (job: Job) =
        job.TryGetValue "testOnly"


    static member SetThreadSpec
        ([<Optional; DefaultParameterValue(null)>]?ThreadSpec:int) =
            (fun (job: Job) ->
                ThreadSpec  |> DynObj.setValueOpt job "threadSpec"
                job
            )
    static member tryGetThreadSpec (job: Job) =
        job.TryGetValue "threadSpec"

    static member SetThreadsPerCore
        ([<Optional; DefaultParameterValue(null)>]?ThreadsPerCore:int) =
            (fun (job: Job) ->
                ThreadsPerCore  |> DynObj.setValueOpt job "threadsPerCore"
                job
            )
    static member tryGetThreadsPerCore (job: Job) =
        job.TryGetValue "threadsPerCore"


    static member SetMinTime
        ([<Optional; DefaultParameterValue(null)>]?MinTime:(int*int*int*int)) =
            (fun (job: Job) ->
                MinTime |> DynObj.setValueOpt job "minTime"
                job
            )

    static member tryGetMinTime (job: Job) =
        job.TryGetValue "minTime"

    static member SetUserID
        ([<Optional; DefaultParameterValue(null)>]?UserID:string) =
            (fun (job: Job) ->
                UserID  |> DynObj.setValueOpt job "userID"
                job
            )
    static member tryGetUserID (job: Job) =
        job.TryGetValue "userID"

    static member SetUseMinNodes
        ([<Optional; DefaultParameterValue(null)>]?UseMinNodes:bool) =
            (fun (job: Job) ->
                UseMinNodes  |> DynObj.setValueOpt job "useMinNodes"
                job
            )
    static member tryGetUseMinNodes (job: Job) =
        job.TryGetValue "useMinNodes"

    static member SetVerbose
        ([<Optional; DefaultParameterValue(null)>]?Verbose:bool) =
            (fun (job: Job) ->
                Verbose  |> DynObj.setValueOpt job "verbose"
                job
            )
    static member tryGetVerbose (job: Job) =
        job.TryGetValue "verbose"

    static member SetWait
        ([<Optional; DefaultParameterValue(null)>]?Wait:bool) =
            (fun (job: Job) ->
                Wait  |> DynObj.setValueOpt job "wait"
                job
            )

    static member tryGetWait (job: Job) =
        job.TryGetValue "wait"

    static member SetWaitAllNodes
        ([<Optional; DefaultParameterValue(null)>]?WaitAllNodes:bool) =
            (fun (job: Job) ->
                WaitAllNodes  |> DynObj.setValueOpt job "waitAllNodes"
                job
            )

    static member tryGetWaitAllNodes (job: Job) =
        job.TryGetValue "waitAllNodes"

    static member SetWrap
        ([<Optional; DefaultParameterValue(null)>]?Wrap:string list) =
            (fun (job: Job) ->
                Wrap  |> DynObj.setValueOpt job "wrap"
                job
            )
    static member tryGetWrap (job: Job) =
        job.TryGetValue "wrap"

    static member ifOnlyOneDigit (digit:int) =
        if digit < 10 then
            "0" + digit.ToString()
        else
            digit.ToString()

    static member formatTime (digits:(int*int*int*int))=
        // let (a,b,c,d) = digits
        let (days,hours,minutes,seconds) = digits
        let po = $"{Job.ifOnlyOneDigit days}-{Job.ifOnlyOneDigit hours}:{Job.ifOnlyOneDigit minutes}:{Job.ifOnlyOneDigit seconds}"
        po

    static member createJobscript (job:Job)=
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
                | (Some value) -> (sprintf "#SBATCH --time=%s" (Job.formatTime (value:?>(int*int*int*int))))
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
                (match (job |> Job.tryGetAccount) with
                | (Some value) -> (sprintf "#SBATCH -A %s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetBatch) with
                | (Some value) -> (sprintf "#SBATCH --batch=%s" (((value:?>string list))|> String.concat ""))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetBurstBufferSpecification) with
                | (Some value) -> (sprintf "#SBATCH --bb=%s" (((value:?>string list))|> String.concat ""))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetBurstBufferSpecificationFilePath) with
                | (Some value) -> (sprintf "#SBATCH --bbf=%s" ((value:?>string)))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetWorkingDirectory) with
                | (Some value) -> (sprintf "#SBATCH -D %s" ((value:?>string)))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetClusters) with
                | (Some value) -> (sprintf "#SBATCH -M %s" (((value:?>string list))|> String.concat ","))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetComment) with
                | (Some value) -> (sprintf "#SBATCH --comment=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetContainer) with
                | (Some value) -> (sprintf "#SBATCH --container=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetContainerID) with
                | (Some value) -> (sprintf "#SBATCH --container-id=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetContiguous) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --contiguous" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetSpezializedCores) with
                | (Some value) -> (sprintf "#SBATCH -S %i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetCoresPerSocket) with
                | (Some value) -> (sprintf "#SBATCH --cores-per-socket=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetCPUsPerGPU) with
                | (Some value) -> (sprintf "#SBATCH --cpus-per-gpu=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetDelayBoot) with
                | (Some value) -> (sprintf "#SBATCH --delay-boot=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetExclude) with
                | (Some value) -> (sprintf "#SBATCH -x %s" (((value:?>string list))|> String.concat " "))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetExtra) with
                | (Some value) -> (sprintf "#SBATCH --extra=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetGroupID) with
                | (Some value) -> (sprintf "#SBATCH --gid=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetHold) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --hold" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetIgnorePBS) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --ignore-pbs" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetInput) with
                | (Some value) -> (sprintf "#SBATCH -i %s" (value:?>string) )
                | (None) -> "")
            ;                
                (match (job |> Job.tryGetKillOnInvalidDep) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --kill-on-invalid-dep" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetMailUser) with
                | (Some value) -> (sprintf "#SBATCH --mail-user=%s" (value:?>string) )
                | (None) -> "")
            ;        
                (match (job |> Job.tryGetMemoryPerGPU) with
                | (Some value) -> (sprintf "#SBATCH --mem-per-gpu=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetMemoryPerCPU) with
                | (Some value) -> (sprintf "#SBATCH --mem-per-cpu=%s" (value:?>string))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetMinCPUs) with
                | (Some value) -> (sprintf "#SBATCH --mincpus=%i" (value:?>int))
                | (None) -> "")
            ;

                (match (job |> Job.tryGetNoRequeue) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --no-requeue" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetNodeFile) with
                | (Some value) -> (sprintf "#SBATCH --nodefile=%s" (value:?>string))
                | (None) -> "")
            
            ;
                (match (job |> Job.tryGetNTasksPerCore) with
                | (Some value) -> (sprintf "#SBATCH --ntasks-per-core=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetNTasksPerGPU) with
                | (Some value) -> (sprintf "#SBATCH --ntasks-per-gpu=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetNTasksPerNode) with
                | (Some value) -> (sprintf "#SBATCH --ntasks-per-node=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetNTasksPerSocket) with
                | (Some value) -> (sprintf "#SBATCH --ntasks-per-socket=%i" (value:?>int))
                | (None) -> "")
            ;

                (match (job |> Job.tryGetOvercommit) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --overcommit" )
                    else 
                        ""
                | (None) -> "")
            ;

                (match (job |> Job.tryGetOversubscribe) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --oversubscribe" )
                    else 
                        ""
                | (None) -> "")
            ;

                (match (job |> Job.tryGetPrefer) with
                | (Some value) -> (sprintf "#SBATCH --prefer=%s" (((value:?>string list))|> String.concat " "))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetQuiet) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --quiet" )
                    else 
                        ""
                | (None) -> "")
            ;

                (match (job |> Job.tryGetReboot) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --reboot" )
                    else 
                        ""
                | (None) -> "")
            ;

                (match (job |> Job.tryGetRequeue) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --requeue" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetReservation) with
                | (Some value) -> (sprintf "#SBATCH --reservation=%s" (((value:?>string list))|> String.concat ","))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetSocketsPerNode) with
                | (Some value) -> (sprintf "#SBATCH --sockets-per-node=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetSpreadJob) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --spread-job" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetSpreadJob) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --test-only" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetThreadSpec) with
                | (Some value) -> (sprintf "#SBATCH --thread-spec=%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetThreadsPerCore) with
                | (Some value) -> (sprintf "#SBATCH --threads-per-core==%i" (value:?>int))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetMinTime) with
                | (Some value) -> (sprintf "#SBATCH --time-min=%s" (Job.formatTime (value:?>(int*int*int*int))))
                | (None) -> "")
            ;
                (match (job |> Job.tryGetUserID) with
                | (Some value) -> (sprintf "#SBATCH --uid=%s" (value:?>string))
                | (None) -> "")
            
            ;
                (match (job |> Job.tryGetUseMinNodes) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --use-min-nodes" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetVerbose) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --verbose" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetWait) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --wait" )
                    else 
                        ""
                | (None) -> "")
            ;
                (match (job |> Job.tryGetWaitAllNodes) with
                | (Some value) -> 
                    if value = true then 
                        (sprintf "#SBATCH --wait-all-nodes=1")
                    else 
                        (sprintf "#SBATCH --wait-all-nodes=0")

                | (None) -> "")
            ;

                (match (job |> Job.tryGetWrap) with
                | (Some value) -> (sprintf "#SBATCH --wrap=%s" (((value:?>string list))|> String.concat " "))
                | (None) -> "")
            ;
                (job.Processes  |> List.map (fun x -> createProcessCall x)) |> String.concat "\n"

        |]
        |> Array.filter (fun x -> x <> "")
        |> Array.map (fun x ->
            sprintf "%s" x
        )
        
//let createJobscript (jobToDo:Job) =
//    commands jobToDo
//    |> Array.map (fun x ->
//        sprintf "%s" x
//    )
