namespace Slurmi

open DynamicObj
open System.Runtime.InteropServices
open Fli
open Process
open System.Collections.Generic


// for more Info, see https://slurm.schedmd.com/sbatch.html
type timeUnit = 
    | Seconds
    | Minutes
    | Hours
    | Days
    | Weeks
    

    
type DateTimeString =
    /// (day, month, year, hour, minute, second)
    | DateTime of int * int * int * int * int * int  
    /// (hour, minute, second)
    | OnlyTime of int * int * int  
    /// ("now" + count * time unit)
    | NowPlusTime of int * timeUnit
    /// ("now")
    | Now 
    /// ("midnight")
    | Midnight  
    /// ("noon")
    | Noon  
    /// ("fika (3:00pm))"
    | Fika  
    /// ("teatime (4:00pm)")
    | TeaTime  
    /// ("today")
    | Today  
    /// ("tomorrow")
    | Tomorrow  


    

type AccountingFrequency = 
    | Task 
    | Energy
    | Network
    | Filesystem

type Range = 
    | Basic of int*int
    | WithStepFunction of int*int*int


type ArraySlurm = 
    {
        Seperators : int[] option
        RangeOfValues : Range option
        SimultaniousJobs : int option

    }
    with
        static member getArray(a:ArraySlurm) =
                a |> fun x -> 
                    [|
                        match x.Seperators with 
                        | Some value -> sprintf "%s" (value |> Array.map (fun x -> sprintf "%i" x) |> String.concat ",")
                        | None -> ""
                    ;
                        match x.RangeOfValues with 
                        | Some value -> 
                            match value with 
                            | Basic (start,stop) -> sprintf "%i-%i" start stop
                            | WithStepFunction (start,stop,step) -> sprintf "%i-%i:%i" start stop step
                        | None -> ""
                    ;
                        match x.SimultaniousJobs with 
                        | Some value -> 
                            let sim =  sprintf "%i" value
                            ("%"+sim)
                        | None -> ""

                        
                    |]
                    |> Array.filter (fun x -> x <> "")
                    |> String.concat ","

type Job(jobName: string,processList:(string*string list)list) =
    inherit DynamicObj()
    static member private dictForKeywords =  
        let dictForSLURM = new Dictionary<string,string>()
        
        let pairs = 
            [|
                ("Name","-J ");
                ("node","-N ");
                ("output","-o ");
                ("error","-e ");
                ("time","--time=");
                ("nTasks","--ntasks=");
                ("cpusPerTask","--cpus-per-task=");
                ("memory","--mem=");
                ("partition","-p ");
                //("environment","");
                ("parsable","--parsable");
                ("dependency","--dependency=");

                ("account","-A ");
                ("batch","--batch=");
                ("bb","--bb=");
                ("bbf","--bbf=");
                ("workingdirectory","-D ");
                ("clusters","-M ");
                ("comment","--comment=");
                ("container","--container=");
                ("containerID","--container-id=");
                ("contiguous","--contiguous");
                ("spezCore","-S ");
                ("coresPerSocket","--cores-per-socket=");
                ("cpuPerGPU","--cpus-per-gpu=");

                ("delayBoot","--delay-boot=");
                ("exclude","-x ");
                ("extra","--extra=");
                ("gid","--gid=");
                ("hold","--hold");
                ("ignorePBS","--ignore-pbs");
                ("input","-i ");
                ("killOnInvalidDep","--kill-on-invalid-dep");
                ("mailUser","--mail-user=");
                ("memoryPerGPU","--mem-per-gpu=");
                ("memoryPerCPU","--mem-per-cpu=");
                ("minCPUs","--mincpus=");

                ("noRequeue","--no-requeue");
                ("nodeFile","--nodefile=");
                ("nTasksPerCore","--ntasks-per-core=");
                ("nTasksPerGPU","--ntasks-per-gpu=");
                ("nTasksPerNode","--ntasks-per-node=");
                ("nTasksPerSocket","--ntasks-per-socket=");
                ("overcommit","--overcommit");
                ("oversubscribe","--oversubscribe");
                ("prefer","--prefer=");
                ("quiet","--quiet");
                ("reboot","--reboot");
                ("requeue","--requeue");

                ("reservation","--reservation=");
                ("socketsPerNode","--sockets-per-node=");
                ("spreadJob","--spread-job");
                ("testOnly","--test-only");
                ("threadSpec","--thread-spec=");
                ("threadsPerCore","--threads-per-core==");
                ("minTime","--time-min=");
                ("userID","--uid=");
                ("useMinNodes","--use-min-nodes");
                ("verbose","--verbose");
                ("wait","--wait");
                ("waitAllNodes","--wait-all-nodes=");

                ("wrap","--wrap=");
                ("accountingFrequency","--acctg-freq=");
                ("array","-a ");
                ("begin","-b ");
                ("environment","");
                ("Processes","")


            |]
            |> Array.map (fun (key,value) -> dictForSLURM.Add (key,value)) |> ignore 
        dictForSLURM




    /// Job Name
    member val Name = jobName with get, set
    
    member val Processes = processList with get,set 

    /// JobID that is returned by Slurm. 
    static member SetJobID
        ([<Optional; DefaultParameterValue(null)>]?JobID: int) =
            (fun (job: Job) ->

                JobID |> DynObj.setValueOpt job "jobid"
                job
            )
    static member tryGetJobID (job: Job) =
        job.TryGetValue "jobid"

    static member removeJobID (job: Job) =
        job.Remove "jobid"

    /// Request that a minimum of minnodes nodes be allocated to this job.
    static member SetNode
        ([<Optional; DefaultParameterValue(null)>]?Node: string) =
            (fun (job: Job) ->

                Node |> DynObj.setValueOpt job "node"
                job
            )
    static member tryGetNode (job: Job) =
        job.TryGetValue "node"

    static member removeNode (job: Job) =
        job.Remove "node"

    /// Instruct Slurm to connect the batch script's standard output directly to the file name specified in the "filename pattern"
    static member SetOutput
        ([<Optional; DefaultParameterValue(null)>]?Output: string) =
            (fun (job: Job) ->
                Output |> DynObj.setValueOpt job "output"
                job
            )
    static member tryGetOutput (job: Job) =
        job.TryGetValue "output"

    static member removeOutput (job: Job) =
        job.Remove "output"

    /// Instruct Slurm to connect the batch script's standard error directly to the file name specified in the "filename pattern".
    static member SetError
        ([<Optional; DefaultParameterValue(null)>]?Error: string) =
            (fun (job: Job) ->
                Error |> DynObj.setValueOpt job "error"
                job
            )
    static member tryGetError (job: Job) =
        job.TryGetValue "error"

    static member removeError (job: Job) =
        job.Remove "error"

    /// Set a limit on the total run time of the job allocation. Format is (day,hours,minutes,seconds).
    static member SetTime
        ([<Optional; DefaultParameterValue(null)>]?Time:(int*int*int*int)) =
            (fun (job: Job) ->
                Time |> DynObj.setValueOpt job "time"
                job
            )
    static member tryGetTime (job: Job) =
        job.TryGetValue "time"

    static member removeTime (job: Job) =
        job.Remove "time"

    /// sbatch does not launch tasks, it requests an allocation of resources and submits a batch script.
    static member SetNTasks
        ([<Optional; DefaultParameterValue(null)>]?NTasks:int) =
            (fun (job: Job) ->
                NTasks |> DynObj.setValueOpt job "nTasks"
                job
            )
    static member tryGetNTasks (job: Job) =
        job.TryGetValue "nTasks"

    static member removeNTasks (job: Job) =
        job.Remove "nTasks"

    /// Advise the Slurm controller that ensuing job steps will require ncpus number of processors per task.
    static member SetCPUsPerTask
        ([<Optional; DefaultParameterValue(null)>]?CPUsPerTask:int) =
            (fun (job: Job) ->
                CPUsPerTask |> DynObj.setValueOpt job "cpusPerTask"
                job
            )
    static member tryGetCPUsPerTask (job: Job) =
        job.TryGetValue "cpusPerTask"
        
    static member removeCPUsPerTask (job: Job) =
        job.Remove "cpusPerTask"


    /// Specify the real memory required per node (with unit, e.g. "30gb").
    static member SetMemory
        ([<Optional; DefaultParameterValue(null)>]?Memory:string) =
            (fun (job: Job) ->
                Memory |> DynObj.setValueOpt job "memory"
                job
            )
    static member tryGetMemory (job: Job) =
        job.TryGetValue "memory"

    static member removeMemory (job: Job) =
        job.Remove "memory"
    /// Request a specific partition for the resource allocation.
    static member SetPartition
        ([<Optional; DefaultParameterValue(null)>]?Partition:string) =
            (fun (job: Job) ->
                Partition |> DynObj.setValueOpt job "partition"
                job
            )
    static member tryGetPartition (job: Job) =
        job.TryGetValue "partition"

    static member removePartition (job: Job) =
        job.Remove "partition"

    //static member SetFileName
    //    ([<Optional; DefaultParameterValue(null)>]?FileName:string) =
    //        (fun (job: Job) ->
    //            FileName |> DynObj.setValueOpt job "fileName"
    //            job
    //        )
    //static member tryGetFileName (job: Job) =
    //    job.TryGetValue "fileName"

    //static member SetOutputFile
    //    ([<Optional; DefaultParameterValue(null)>]?OutputFile:string) =
    //        (fun (job: Job) ->
    //            OutputFile |> DynObj.setValueOpt job "outputFile"
    //            job
    //        )
    //static member tryGetOutputFile (job: Job) =
    //    job.TryGetValue "outputFile"

    /// Set Commands to load the specified environment before executing the job script.
    static member SetEnvironment
        ([<Optional; DefaultParameterValue(null)>]?Environment:EnvironmentSLURM) =
            (fun (job: Job) ->
                Environment  |> DynObj.setValueOpt job "environment"
                job
            )
    static member tryGetEnvironment (job: Job) =
        job.TryGetValue "environment"

    static member removeEnvironment (job: Job) =
        job.Remove "environment"

    /// Outputs only the job id number and the cluster name if present.
    static member SetParsable
        ([<Optional; DefaultParameterValue(null)>]?Parsable:bool) =
            (fun (job: Job) ->
                Parsable  |> DynObj.setValueOpt job "parsable"
                job
            )
    static member tryGetParsable (job: Job) =
        job.TryGetValue "parsable"

    static member removeParsable(job: Job) =
        job.Remove "parsable"

    /// Defer the start of this job until the specified dependencies have been satisfied.
    static member SetDependency
        ([<Optional; DefaultParameterValue(null)>]?Dependency:TypeOfDep*DependencyType[]) =
            (fun (job: Job) ->
                Job.SetParsable true job |> ignore 
                Dependency  |> DynObj.setValueOpt job "dependency"
                job
            )
    static member tryGetDependency (job: Job) =
        job.TryGetValue "dependency"

    static member removeDependency(job: Job) =
        job.Remove "dependency"

    /// Charge resources used by this job to specified account.
    static member SetAccount
        ([<Optional; DefaultParameterValue(null)>]?Account:string) =
            (fun (job: Job) ->
                Account  |> DynObj.setValueOpt job "account"
                job
            )
    static member tryGetAccount (job: Job) =
        job.TryGetValue "account"

    static member removeAccount (job: Job) =
        job.Remove "account"

    /// Nodes can have features assigned to them by the Slurm administrator. Users can specify which of these features are required by their batch script using this options.
    static member SetBatch
        ([<Optional; DefaultParameterValue(null)>]?Batch:string list) =
            (fun (job: Job) ->
                Batch  |> DynObj.setValueOpt job "batch"
                job
            )
    static member tryGetBatch (job: Job) =
        job.TryGetValue "batch"

    static member removeBatch (job: Job) =
        job.Remove "batch"

    /// When the --bb option is used, Slurm parses this option and creates a temporary burst buffer script file that is used internally by the burst buffer plugins.
    static member SetBurstBufferSpecification
        ([<Optional; DefaultParameterValue(null)>]?BurstBufferSpecification:string list) =
            (fun (job: Job) ->
                BurstBufferSpecification  |> DynObj.setValueOpt job "bb"
                job
            )
    static member tryGetBurstBufferSpecification (job: Job) =
        job.TryGetValue "bb"

    static member removeBurstBufferSpecification (job: Job) =
        job.Remove "bb"

    /// Path of file containing burst buffer specification.
    static member SetBurstBufferSpecificationFilePath
        ([<Optional; DefaultParameterValue(null)>]?BurstBufferSpecificationFilePath:string) =
            (fun (job: Job) ->
                BurstBufferSpecificationFilePath  |> DynObj.setValueOpt job "bbf"
                job
            )
    static member tryGetBurstBufferSpecificationFilePath (job: Job) =
        job.TryGetValue "bbf"

    static member removeBurstBufferSpecificationFilePath (job: Job) =
        job.Remove "bbf"

    /// Set the working directory of the batch script to directory before it is executed. The path can be specified as full path or relative path to the directory where the command is executed. 
    static member SetWorkingDirectory
        ([<Optional; DefaultParameterValue(null)>]?WorkingDirectory:string) =
            (fun (job: Job) ->
                WorkingDirectory  |> DynObj.setValueOpt job "workingdirectory"
                job
            )
    static member tryGetWorkingDirectory (job: Job) =
        job.TryGetValue "workingdirectory"

    static member removeWorkingDirectory (job: Job) =
        job.Remove "workingdirectory"

    /// Clusters to issue commands to.
    static member SetClusters
        ([<Optional; DefaultParameterValue(null)>]?Clusters:string list) =
            (fun (job: Job) ->
                Clusters  |> DynObj.setValueOpt job "clusters"
                job
            )
    static member tryGetClusters (job: Job) =
        job.TryGetValue "clusters"

    static member removeClusters (job: Job) =
        job.Remove "clusters"

    /// An arbitrary comment enclosed in double quotes if using spaces or some special characters. 
    static member SetComment
        ([<Optional; DefaultParameterValue(null)>]?Comment:string) =
            (fun (job: Job) ->
                Comment  |> DynObj.setValueOpt job "comment"
                job
            )
    static member tryGetComment (job: Job) =
        job.TryGetValue "comment"

    static member removeComment (job: Job) =
        job.Remove "comment"
        
    /// Absolute path to OCI container bundle. 
    static member SetContainer
        ([<Optional; DefaultParameterValue(null)>]?Container:string) =
            (fun (job: Job) ->
                Container  |> DynObj.setValueOpt job "container"
                job
            )
    static member tryGetContainer (job: Job) =
        job.TryGetValue "container"

    static member removeContainer (job: Job) =
        job.Remove "container"

    /// Unique name for OCI container.
    static member SetContainerID
        ([<Optional; DefaultParameterValue(null)>]?ContainerID:string) =
            (fun (job: Job) ->
                ContainerID  |> DynObj.setValueOpt job "containerID"
                job
            )
    static member tryGetContainerID (job: Job) =
        job.TryGetValue "containerID"

    static member removeContainerID (job: Job) =
        job.Remove "containerID"

    /// If set, then the allocated nodes must form a contiguous set. 
    static member SetContiguous
        ([<Optional; DefaultParameterValue(null)>]?Contiguous:bool) =
            (fun (job: Job) ->
                Contiguous  |> DynObj.setValueOpt job "contiguous"
                job
            )
    static member tryGetContiguous (job: Job) =
        job.TryGetValue "contiguous"

    static member removeContiguous (job: Job) =
        job.Remove "contiguous"

    /// Count of Specialized Cores per node reserved by the job for system operations and not used by the application.
    static member SetSpezializedCores
        ([<Optional; DefaultParameterValue(null)>]?SpezializedCores:int) =
            (fun (job: Job) ->
                SpezializedCores  |> DynObj.setValueOpt job "spezCore"
                job
            )
    static member tryGetSpezializedCores (job: Job) =
        job.TryGetValue "spezCore"
    static member removeSpezializedCores (job: Job) =
        job.Remove "spezCore"

    /// Restrict node selection to nodes with at least the specified number of cores per socket.
    static member SetCoresPerSocket
        ([<Optional; DefaultParameterValue(null)>]?CoresPerSocket:int) =
            (fun (job: Job) ->
                CoresPerSocket  |> DynObj.setValueOpt job "coresPerSocket"
                job
            )
    static member tryGetCoresPerSocket (job: Job) =
        job.TryGetValue "coresPerSocket"

    static member removeCoresPerSocker (job: Job) =
        job.Remove "coresPerSocket"

    /// Advise Slurm that ensuing job steps will require ncpus processors per allocated GPU. Not compatible with the --cpus-per-task option. 
    static member SetCPUsPerGPU
        ([<Optional; DefaultParameterValue(null)>]?CPUsPerGPU:int) =
            (fun (job: Job) ->
                CPUsPerGPU  |> DynObj.setValueOpt job "cpuPerGPU"
                job
            )
    static member tryGetCPUsPerGPU (job: Job) =
        job.TryGetValue "cpuPerGPU"

    static member removeCPUsPerGPU (job: Job) =
        job.Remove "cpuPerGPU"

    /// Do not reboot nodes in order to satisfied this job's feature specification if the job has been eligible to run for less than this time period. If the job has waited for less than the specified period, it will use only nodes which already have the specified features. The argument is in units of minutes.
    static member SetDelayBoot
        ([<Optional; DefaultParameterValue(null)>]?DelayBoot:int) =
            (fun (job: Job) ->
                DelayBoot  |> DynObj.setValueOpt job "delayBoot"
                job
            )
    static member tryGetDelayBoot (job: Job) =
        job.TryGetValue "delayBoot"

    static member removeDelayBoot (job: Job) =
        job.Remove "delayBoot"

    /// Explicitly exclude certain nodes from the resources granted to the job. 
    static member SetExclude
        ([<Optional; DefaultParameterValue(null)>]?Exclude:string list) =
            (fun (job: Job) ->
                Exclude  |> DynObj.setValueOpt job "exclude"
                job
            )
    static member tryGetExclude (job: Job) =
        job.TryGetValue "exclude"

    static member removeExclude (job: Job) =
        job.Remove "ecxlude"

    /// An arbitrary string enclosed in double quotes if using spaces or some special characters. 
    static member SetExtra
        ([<Optional; DefaultParameterValue(null)>]?Extra:string) =
            (fun (job: Job) ->
                Extra  |> DynObj.setValueOpt job "extra"
                job
            )
    static member tryGetExtra (job: Job) =
        job.TryGetValue "extra"
    static member removeExtra (job: Job) =
        job.Remove "extra"
    /// If sbatch is run as root, and the --gid option is used, submit the job with group's group access permissions. group may be the group name or the numerical group ID. 
    static member SetGroupID
        ([<Optional; DefaultParameterValue(null)>]?GroupID:string) =
            (fun (job: Job) ->
                GroupID  |> DynObj.setValueOpt job "gid"
                job
            )
    static member tryGetGroupID (job: Job) =
        job.TryGetValue "gid"

    static member removeGroupID (job: Job) =
        job.Remove "gid"

    /// Specify the job is to be submitted in a held state (priority of zero). A held job can now be released using scontrol to reset its priority (e.g. "scontrol release <job_id>"). 
    static member SetHold
        ([<Optional; DefaultParameterValue(null)>]?Hold:bool) =
            (fun (job: Job) ->
                Hold  |> DynObj.setValueOpt job "hold"
                job
            )
    static member tryGetHold (job: Job) =
        job.TryGetValue "hold"
    static member removeHold (job: Job) =
        job.Remove "hold"

    /// Ignore all "#PBS" and "#BSUB" options specified in the batch script. 
    static member SetIgnorePBS
        ([<Optional; DefaultParameterValue(null)>]?IgnorePBS:bool) =
            (fun (job: Job) ->
                IgnorePBS  |> DynObj.setValueOpt job "ignorePBS"
                job
            )
    static member tryGetIgnorePBS (job: Job) =
        job.TryGetValue "ignorePBS"
    static member removeIgnorePBS (job: Job) =
        job.Remove "ignorePBS"

    /// Instruct Slurm to connect the batch script's standard input directly to the file name specified in the "filename pattern".
    static member SetInput
        ([<Optional; DefaultParameterValue(null)>]?Input:string) =
            (fun (job: Job) ->
                Input  |> DynObj.setValueOpt job "input"
                job
            )
    static member tryGetInput (job: Job) =
        job.TryGetValue "input"

    static member removeInput (job: Job) =
        job.Remove "input"

    /// If a job has an invalid dependency and it can never run this parameter tells Slurm to terminate it or not.
    static member SetKillOnInvalidDep
        ([<Optional; DefaultParameterValue(null)>]?KillOnInvalidDep:bool) =
            (fun (job: Job) ->
                KillOnInvalidDep  |> DynObj.setValueOpt job "killOnInvalidDep"
                job
            )
    static member tryGetKillOnInvalidDep (job: Job) =
        job.TryGetValue "killOnInvalidDep"

    static member removeKillOnInvalidDep (job: Job) =
        job.Remove "killOnInvalidDep"

    ///User to receive email notification of state changes as defined by --mail-type. The default value is the submitting user.
    static member SetMailUser
        ([<Optional; DefaultParameterValue(null)>]?MailUser:string) =
            (fun (job: Job) ->
                MailUser  |> DynObj.setValueOpt job "mailUser"
                job
            )
    static member tryGetMailUser (job: Job) =
        job.TryGetValue "mailUser"

    static member removeMailUser (job: Job) =
        job.Remove "mailUser"
    /// Minimum memory required per allocated GPU (with unit, e.g. "30gb").
    static member SetMemoryPerGPU
        ([<Optional; DefaultParameterValue(null)>]?MemoryPerGPU:string) =
            (fun (job: Job) ->
                MemoryPerGPU |> DynObj.setValueOpt job "memoryPerGPU"
                job
            )
    static member tryGetMemoryPerGPU (job: Job) =
        job.TryGetValue "memoryPerGPU"

    static member removememoryPerGPU (job: Job) =
        job.Remove "memoryPerGPU"

    /// Minimum memory required per usable allocated CPU (with unit, e.g. "30gb"). 
    static member SetMemoryPerCPU
        ([<Optional; DefaultParameterValue(null)>]?MemoryPerCPU:string) =
            (fun (job: Job) ->
                MemoryPerCPU |> DynObj.setValueOpt job "memoryPerCPU"
                job
            )
    static member tryGetMemoryPerCPU (job: Job) =
        job.TryGetValue "memoryPerCPU"

    static member removeMemoryPerCPU (job: Job) =
        job.Remove "memoryPerCPU"

    /// Specify a minimum number of logical cpus/processors per node. 
    static member SetMinCPUs
        ([<Optional; DefaultParameterValue(null)>]?MinCPUs:int) =
            (fun (job: Job) ->
                MinCPUs  |> DynObj.setValueOpt job "minCPUs"
                job
            )
    static member tryGetMinCPUs (job: Job) =
        job.TryGetValue "minCPUs"

    static member removeMinCPUs (job: Job) =
        job.Remove "minCPUs"

    /// Specifies that the batch job should never be requeued under any circumstances.
    static member SetNoRequeue
        ([<Optional; DefaultParameterValue(null)>]?NoRequeue:bool) =
            (fun (job: Job) ->
                NoRequeue  |> DynObj.setValueOpt job "noRequeue"
                job
            )
    static member tryGetNoRequeue (job: Job) =
        job.TryGetValue "noRequeue"

    static member removeNoRequeue (job: Job) =
        job.Remove "noRequeue"

    /// Much like --nodelist, but the list is contained in a file of name node file.
    static member SetNodeFile
        ([<Optional; DefaultParameterValue(null)>]?NodeFile:string) =
            (fun (job: Job) ->
                NodeFile  |> DynObj.setValueOpt job "nodeFile"
                job
            )
    static member tryGetNodeFile (job: Job) =
        job.TryGetValue "nodeFile"

    static member removeNodeFile (job: Job) =
        job.Remove "nodeFile"

    /// Request the maximum ntasks be invoked on each core. Meant to be used with the --ntasks option. Related to --ntasks-per-node except at the core level instead of the node level.
    static member SetNTasksPerCore
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerCore:int) =
            (fun (job: Job) ->
                NTasksPerCore  |> DynObj.setValueOpt job "nTasksPerCore"
                job
            )
    static member tryGetNTasksPerCore (job: Job) =
        job.TryGetValue "nTasksPerCore"

    static member removeNTasksPerCore (job: Job) =
        job.Remove "nTasksPerCore"

    /// Request that there are ntasks tasks invoked for every GPU. This option can work in two ways: 1) either specify --ntasks in addition, in which case a type-less GPU specification will be automatically determined to satisfy --ntasks-per-gpu, or 2) specify the GPUs wanted (e.g. via --gpus or --gres) without specifying --ntasks, and the total task count will be automatically determined. 
    static member SetNTasksPerGPU
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerGPU:int) =
            (fun (job: Job) ->
                NTasksPerGPU  |> DynObj.setValueOpt job "nTasksPerGPU"
                job
            )
    static member tryGetNTasksPerGPU (job: Job) =
        job.TryGetValue "nTasksPerGPU"

    static member removeNTasksPerGPU (job: Job) =
        job.Remove "nTasksPerGPU"

    /// Request that ntasks be invoked on each node. If used with the --ntasks option, the --ntasks option will take precedence and the --ntasks-per-node will be treated as a maximum count of tasks per node. Meant to be used with the --nodes option. This is related to --cpus-per-task=ncpus, but does not require knowledge of the actual number of cpus on each node.
    static member SetNTasksPerNode
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerNode:int) =
            (fun (job: Job) ->
                NTasksPerNode  |> DynObj.setValueOpt job "nTasksPerNode"
                job
            )
    static member tryGetNTasksPerNode (job: Job) =
        job.TryGetValue "nTasksPerNode"

    static member removeNTasksPerNode (job: Job) =
        job.Remove "nTasksPerNode"

    /// Request the maximum ntasks be invoked on each socket. Meant to be used with the --ntasks option. Related to --ntasks-per-node except at the socket level instead of the node level. 
    static member SetNTasksPerSocket
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerSocket:int) =
            (fun (job: Job) ->
                NTasksPerSocket  |> DynObj.setValueOpt job "nTasksPerSocket"
                job
            )
    static member tryGetNTasksPerSocket (job: Job) =
        job.TryGetValue "nTasksPerSocket"

    static member removeNTasksPerSocket (job: Job) =
        job.Remove "nTasksPerSocket"

    /// Overcommit resources. When applied to a job allocation (not including jobs requesting exclusive access to the nodes) the resources are allocated as if only one task per node is requested. This means that the requested number of cpus per task (-c, --cpus-per-task) are allocated per node rather than being multiplied by the number of tasks. Options used to specify the number of tasks per node, socket, core, etc. are ignored. 
    static member SetOvercommit
        ([<Optional; DefaultParameterValue(null)>]?Overcommit:bool) =
            (fun (job: Job) ->
                Overcommit  |> DynObj.setValueOpt job "overcommit"
                job
            )
    static member tryGetOvercommit (job: Job) =
        job.TryGetValue "overcommit"

    static member removeOvercommit (job: Job) =
        job.Remove "overcommit"

    /// The job allocation can over-subscribe resources with other running jobs. The resources to be over-subscribed can be nodes, sockets, cores, and/or hyperthreads depending upon configuration. The default over-subscribe behavior depends on system configuration and the partition's OverSubscribe option takes precedence over the job's option.
    static member SetOversubscribe
        ([<Optional; DefaultParameterValue(null)>]?Oversubscribe:bool) =
            (fun (job: Job) ->
                Oversubscribe  |> DynObj.setValueOpt job "oversubscribe"
                job
            )
    static member tryGetOversubscribe (job: Job) =
        job.TryGetValue "oversubscribe"

    static member removeOversubscribe (job: Job) =
        job.Remove "oversubscribe"

    /// Nodes can have features assigned to them by the Slurm administrator. Users can specify which of these features are desired but not required by their job using the prefer option.
    static member SetPrefer
        ([<Optional; DefaultParameterValue(null)>]?Prefer:string list) =
            (fun (job: Job) ->
                Prefer  |> DynObj.setValueOpt job "prefer"
                job
            )
    static member tryGetPrefer (job: Job) =
        job.TryGetValue "prefer"

    static member removePrefer (job: Job) =
        job.Remove "prefer"

    /// Suppress informational messages from sbatch such as Job ID. Only errors will still be displayed. 
    static member SetQuiet
        ([<Optional; DefaultParameterValue(null)>]?Quiet:bool) =
            (fun (job: Job) ->
                Quiet  |> DynObj.setValueOpt job "quiet"
                job
            )
    static member tryGetQuiet (job: Job) =
        job.TryGetValue "quiet"

    static member removeQuiet (job: Job) =
        job.Remove "quiet"

    /// Force the allocated nodes to reboot before starting the job. This is only supported with some system configurations and will otherwise be silently ignored. Only root, SlurmUser or admins can reboot nodes. 
    static member SetReboot
        ([<Optional; DefaultParameterValue(null)>]?Reboot:bool) =
            (fun (job: Job) ->
                Reboot  |> DynObj.setValueOpt job "reboot"
                job
            )
    static member tryGetReboot (job: Job) =
        job.TryGetValue "reboot"

    static member removeReboot (job: Job) =
        job.Remove "reboot"

    /// Specifies that the batch job should be eligible for requeuing. The job may be requeued explicitly by a system administrator, after node failure, or upon preemption by a higher priority job. When a job is requeued, the batch script is initiated from its beginning.
    static member SetRequeue
        ([<Optional; DefaultParameterValue(null)>]?Requeue:bool) =
            (fun (job: Job) ->
                Requeue  |> DynObj.setValueOpt job "requeue"
                job
            )
    static member tryGetRequeue (job: Job) =
        job.TryGetValue "requeue"

    static member removeRequeue (job: Job) =
        job.Remove "requeue"

    /// Allocate resources for the job from the named reservation.
    static member SetReservation
        ([<Optional; DefaultParameterValue(null)>]?Reservation:string list) =
            (fun (job: Job) ->
                Reservation  |> DynObj.setValueOpt job "reservation"
                job
            )
    static member tryGetReservation (job: Job) =
        job.TryGetValue "reservation"

    static member removeReservation (job: Job) =
        job.Remove "reservation"

    ///Restrict node selection to nodes with at least the specified number of sockets. See additional information under -B option above when task/affinity plugin is enabled. NOTE: This option may implicitly set the number of tasks (if -n was not specified) as one task per requested thread.
    static member SetSocketsPerNode
        ([<Optional; DefaultParameterValue(null)>]?SocketsPerNode:int) =
            (fun (job: Job) ->
                SocketsPerNode  |> DynObj.setValueOpt job "socketsPerNode"
                job
            )
    static member tryGetSocketsPerNode (job: Job) =
        job.TryGetValue "socketsPerNode"

    static member removeSocketsPerNode (job: Job) =
        job.Remove "sockertsPerNode"

    /// Spread the job allocation over as many nodes as possible and attempt to evenly distribute tasks across the allocated nodes. This option disables the topology/tree plugin. 
    static member SetSpreadJob
        ([<Optional; DefaultParameterValue(null)>]?SpreadJob:bool) =
            (fun (job: Job) ->
                SpreadJob  |> DynObj.setValueOpt job "spreadJob"
                job
            )
    static member tryGetSpreadJob (job: Job) =
        job.TryGetValue "spreadJob"

    static member removeSpreadJob (job: Job) =
        job.Remove "spreadJob"

    /// Validate the batch script and return an estimate of when a job would be scheduled to run given the current job queue and all the other arguments specifying the job requirements. No job is actually submitted. 
    static member SetTestOnly
        ([<Optional; DefaultParameterValue(null)>]?TestOnly:bool) =
            (fun (job: Job) ->
                TestOnly  |> DynObj.setValueOpt job "testOnly"
                job
            )
    static member tryGetTestOnly (job: Job) =
        job.TryGetValue "testOnly"

    static member removetestOnly (job: Job) =
        job.Remove "testOnly"

    /// Count of specialized threads per node reserved by the job for system operations and not used by the application. The application will not use these threads, but will be charged for their allocation. This option can not be used with the --core-spec option. 
    static member SetThreadSpec
        ([<Optional; DefaultParameterValue(null)>]?ThreadSpec:int) =
            (fun (job: Job) ->
                ThreadSpec  |> DynObj.setValueOpt job "threadSpec"
                job
            )
    static member tryGetThreadSpec (job: Job) =
        job.TryGetValue "threadSpec"

    static member removeThreadSpec (job: Job) =
        job.Remove "threadSpec"

    /// Restrict node selection to nodes with at least the specified number of threads per core. In task layout, use the specified maximum number of threads per core. NOTE: "Threads" refers to the number of processing units on each core rather than the number of application tasks to be launched per core.
    static member SetThreadsPerCore
        ([<Optional; DefaultParameterValue(null)>]?ThreadsPerCore:int) =
            (fun (job: Job) ->
                ThreadsPerCore  |> DynObj.setValueOpt job "threadsPerCore"
                job
            )
    static member tryGetThreadsPerCore (job: Job) =
        job.TryGetValue "threadsPerCore"

    static member removeThreadsPerCore (job: Job) =
        job.Remove "threadsPerCore"

    /// Set a minimum time limit on the job allocation. Format is (day,hours,minutes,seconds). If specified, the job may have its --time limit lowered to a value no lower than --time-min if doing so permits the job to begin execution earlier than otherwise possible. 
    static member SetMinTime
        ([<Optional; DefaultParameterValue(null)>]?MinTime:(int*int*int*int)) =
            (fun (job: Job) ->
                MinTime |> DynObj.setValueOpt job "minTime"
                job
            )
    static member tryGetMinTime (job: Job) =
        job.TryGetValue "minTime"

    static member removeMinTime (job: Job) =
        job.Remove "minTime"

    /// Attempt to submit and/or run a job as user instead of the invoking user id. The invoking user's credentials will be used to check access permissions for the target partition. User root may use this option to run jobs as a normal user in a RootOnly partition for example. If run as root, sbatch will drop its permissions to the uid specified after node allocation is successful. user may be the user name or numerical user ID. 
    static member SetUserID
        ([<Optional; DefaultParameterValue(null)>]?UserID:string) =
            (fun (job: Job) ->
                UserID  |> DynObj.setValueOpt job "userID"
                job
            )
    static member tryGetUserID (job: Job) =
        job.TryGetValue "userID"

    static member removeUserID (job: Job) =
        job.Remove "userID"

    /// If a range of node counts is given, prefer the smaller count. 
    static member SetUseMinNodes
        ([<Optional; DefaultParameterValue(null)>]?UseMinNodes:bool) =
            (fun (job: Job) ->
                UseMinNodes  |> DynObj.setValueOpt job "useMinNodes"
                job
            )
    static member tryGetUseMinNodes (job: Job) =
        job.TryGetValue "useMinNodes"
        
    static member removeUseMinNodes (job: Job) =
        job.Remove "useMinNodes"

    /// Increase the verbosity of sbatch's informational messages. Multiple -v's will further increase sbatch's verbosity. By default only errors will be displayed. 
    static member SetVerbose
        ([<Optional; DefaultParameterValue(null)>]?Verbose:bool) =
            (fun (job: Job) ->
                Verbose  |> DynObj.setValueOpt job "verbose"
                job
            )
    static member tryGetVerbose (job: Job) =
        job.TryGetValue "verbose"

    static member removeVerbose (job: Job) =
        job.Remove "verbose"

    /// Do not exit until the submitted job terminates. The exit code of the sbatch command will be the same as the exit code of the submitted job. If the job terminated due to a signal rather than a normal exit, the exit code will be set to 1. In the case of a job array, the exit code recorded will be the highest value for any task in the job array. 
    static member SetWait
        ([<Optional; DefaultParameterValue(null)>]?Wait:bool) =
            (fun (job: Job) ->
                Wait  |> DynObj.setValueOpt job "wait"
                job
            )
    static member tryGetWait (job: Job) =
        job.TryGetValue "wait"

    static member removeWait (job: Job) =
        job.Remove "wait"

    /// Controls when the execution of the command begins. By default the job will begin execution as soon as the allocation is made.
    static member SetWaitAllNodes
        ([<Optional; DefaultParameterValue(null)>]?WaitAllNodes:bool) =
            (fun (job: Job) ->
                WaitAllNodes  |> DynObj.setValueOpt job "waitAllNodes"
                job
            )
    static member tryGetWaitAllNodes (job: Job) =
        job.TryGetValue "waitAllNodes"

    static member removeWaitAllNodes (job: Job) =
        job.Remove "waitAllNodes"
    /// Sbatch will wrap the specified command string in a simple "sh" shell script, and submit that script to the slurm controller. When --wrap is used, a script name and arguments may not be specified on the command line; instead the sbatch-generated wrapper script is used. 
    static member SetWrap
        ([<Optional; DefaultParameterValue(null)>]?Wrap:string list) =
            (fun (job: Job) ->
                Wrap  |> DynObj.setValueOpt job "wrap"
                job
            )
    static member tryGetWrap (job: Job) =
        job.TryGetValue "wrap"

    static member removeWrap (job: Job) =
        job.Remove "wrap"

    static member SetAccountingFrequency
        ([<Optional; DefaultParameterValue(null)>]?AccountingFrequency:(AccountingFrequency*int)[]) =
            (fun (job: Job) ->

                AccountingFrequency |> DynObj.setValueOpt job "accountingFrequency"
                job
            )
    static member tryGetAccountingFrequency (job: Job) =
        job.TryGetValue "accountingFrequency"

    static member removeAccountingFrequency(job: Job) =
        job.Remove "accountingFrequency"

    static member SetArray
        ([<Optional; DefaultParameterValue(null)>]?Array:(ArraySlurm)) =
            (fun (job: Job) ->

                Array |> DynObj.setValueOpt job "array"
                job
            )
    static member tryGetArray (job: Job) =
        job.TryGetValue "array"

    static member removeArray (job: Job) =
        job.Remove "array"

    static member SetBegin
        ([<Optional; DefaultParameterValue(null)>]?BeginTime:(DateTimeString)) =
            (fun (job: Job) ->
                match BeginTime with 
                | Some v -> 
                    (Job.matchDateTime v)
                    |> DynObj.setValue job "begin"
                    job
                | None -> job

            )
    static member tryGetBegin (job: Job) =
        job.TryGetValue "begin"

    static member removeBegin (job: Job) =
        job.Remove "begin"

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

    static member getTimeUnit (tU:timeUnit) = 
        match tU with 
        | Seconds -> "seconds"
        | Minutes -> "minutes"
        | Hours -> "hours"
        | Days -> "days"
        | Weeks -> "weeks"

    static member matchDateTime (dT:DateTimeString) = 
        match dT with 
        | DateTime (day, month, year, hour, minute, second) -> 
             $"{year}-{Job.ifOnlyOneDigit month}-{Job.ifOnlyOneDigit day}T{Job.ifOnlyOneDigit hour}:{Job.ifOnlyOneDigit minute}:{Job.ifOnlyOneDigit second}"
        // sprintf "%i-%i-%iT%i:%i:%i" year (Job.ifOnlyOneDigit month) day hour minute second
        | OnlyTime (hour, minute, second) -> $"{Job.ifOnlyOneDigit hour}:{Job.ifOnlyOneDigit minute}:{Job.ifOnlyOneDigit second}"
        | NowPlusTime (count, timeUnit) -> sprintf "now+%i%s" count (Job.getTimeUnit timeUnit)
        | Now -> "now"
        | Midnight -> "midnight"
        | Noon -> "noon"
        | Fika -> "fika"
        | TeaTime -> "teatime"
        | Today -> "today"
        | Tomorrow -> "tomorrow"

    static member private  createSLURMFormat (arg:KeyValuePair<string,obj>) (str:System.Text.StringBuilder)=
            if arg.Key = "time" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (Job.formatTime (arg.Value:?>(int*int*int*int)))) |> ignore 
            elif arg.Key = "environment" then 
                str.AppendFormat("{0}{1}\n",          (Job.dictForKeywords).Item arg.Key,((arg.Value:?>EnvironmentSLURM).GetEnvironment()) |> List.map (fun x -> String.concat " " [(fst x);(snd x)] ) |> String.concat "\n" )|> ignore 
            elif arg.Key = "batch" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (((arg.Value:?>string list))|> String.concat "")) |> ignore 
            elif arg.Key = "bb" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (((arg.Value:?>string list))|> String.concat "")) |> ignore 
            elif arg.Key = "clusters" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (((arg.Value:?>string list))|> String.concat ",")) |> ignore 
            elif arg.Key = "exclude" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (((arg.Value:?>string list))|> String.concat " ")) |> ignore 
            elif arg.Key = "prefer" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (((arg.Value:?>string list))|> String.concat " ")) |> ignore 
            elif arg.Key = "reservation" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (((arg.Value:?>string list))|> String.concat ",")) |> ignore 
            elif arg.Key = "minTime" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (Job.formatTime (arg.Value:?>(int*int*int*int)))) |> ignore 
            elif arg.Key = "accountingFrequency" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, ((arg.Value:?>(AccountingFrequency*int)[] |> Array.map (fun (fst,snd) -> (fst.ToString()+ "=" + (snd |> string)) )) |> String.concat "," )) |> ignore 
            elif arg.Key = "array" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (arg.Value:?>ArraySlurm |> ArraySlurm.getArray)) |> ignore 
            elif arg.Key = "dependency" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, (DependencyType.concat (fst (arg.Value:?>(TypeOfDep*DependencyType[]))) (snd (arg.Value:?>(TypeOfDep*DependencyType[]))))) |> ignore 
            //elif arg.Key = "begin" then 
            //    str.AppendFormat("#SBATCH {0}{1}\n",  dictForSLURM.Item arg.Key, ((arg.Value:?>string ))) |> ignore 
            elif arg.Key = "waitAllNodes" then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, 1) |> ignore 
            elif arg.Value = true  then 
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, "") |> ignore 
            elif arg.Key = "jobid" then 
                () 
            elif arg.Key = "Processes" then 
                str.AppendFormat("{0}{1}",(arg.Value:?>(string*string list)list |> List.map (fun x -> createProcessCall x) |> String.concat "\n"),"")  |> ignore 
            else
                str.AppendFormat("#SBATCH {0}{1}\n",  (Job.dictForKeywords).Item arg.Key, arg.Value) |> ignore 

    static member createJobScript (job:Job) = 

        let str = new System.Text.StringBuilder()  

        let props = job.GetProperties(true)
        let sorted = 
            props 
            |> Seq.toArray 
            |> Array.sortBy ( fun x -> 
                if x.Key = "Processes" then 
                    2
                elif x.Key = "environment" then 
                    1
                else 
                    0
            )
        sorted |> Array.map (fun x -> Job.createSLURMFormat x str) |> ignore 
        str.ToString()
