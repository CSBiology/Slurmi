namespace Slurmi

open DynamicObj
open System.Runtime.InteropServices
open Fli


type timeUnit = 
    | Seconds
    | Minutes
    | Hours
    | Days
    | Weeks
type AmPm =
    | AM
    | PM
type TimeClock = 
    {
    hour: int
    minute: int
    second: int option
    }
    with 
        override this.ToString() = 
            if this = {hour = 0; minute = 0; second = Some 0} then 
                let hourString = sprintf "%02d" this.hour
                let minuteString = sprintf "01"
                let secondString = 
                    match this.second with
                    | Some(s) -> sprintf ":%02d" s
                    | None -> ""
                sprintf "%s:%s%s" hourString minuteString secondString 
            else 
                let hourString = sprintf "%02d" this.hour
                let minuteString = sprintf "%02d" this.minute
                let secondString = 
                    match this.second with
                    | Some(s) -> sprintf ":%02d" s
                    | None -> ""
                sprintf "%s:%s%s" hourString minuteString secondString 

type TimeFormat1 =
    {
        clock: TimeClock
        amPm: AmPm option
    }

    override this.ToString() =
        let amPmString =
            match this.amPm with
            | Some(AM) -> " AM"
            | Some(PM) -> " PM"
            | None -> ""
        sprintf "%s%s" (this.clock.ToString()) amPmString


type Date =
    {
        month: int
        day: int
        year: int option
    }

    override this.ToString() =
        let yearString =
            match this.year with
            | Some(y) -> sprintf "/%02d" y
            | None -> ""
        sprintf "%02d/%02d%s" this.month this.day yearString



type TimeFormat2 =
    {
        date: Date
        time: TimeClock 
    }

    override this.ToString() =
        sprintf "%s-%s" (this.date.ToString()) (this.time.ToString())


type TimeFormat3 =
    {
        count:int option
        unit: timeUnit option
    }
    with 

        override this.ToString() =
            let count'=
                match this.count with 
                | Some (c) -> sprintf "+%i" c
                | None -> ""
            let unit'=
                match this.unit with 
                | Some (u) -> u.ToString()
                | None -> ""
            sprintf "now%s%s" count' unit'

type Time = 
    {
        Days:int option
        clock:TimeClock option
    }
    with 

        override this.ToString() =
            let days = 
                match this.Days with
                | Some(s) -> sprintf "%02d-" s
                | None -> ""
            let clock' = 
                match this.clock with
                | Some(s) -> (s.ToString())
                | None -> ""
            sprintf "%s%s" days clock'


//type TypeOfDep =
//    | All
//    | Any

//    override this.ToString() =
//        match this with 
//        | All -> ","  
//        | Any ->  "?" 



// type DependencyType =
//     /// After the specified jobs start or are cancelled and 'time' in minutes from job start or cancellation happens, this job can begin execution. If no 'time' is given then there is no delay after start or cancellation.
//     | After of (string*int option) list
//     /// This job can begin execution after the specified jobs have terminated. This is the default dependency type.
//     | Afterany of string list
//     /// This job can begin execution after the specified jobs have terminated and any associated burst buffer stage out operations have completed.
//     | Afterburstbuffer of string list
//     /// A task of this job array can begin execution after the corresponding task ID in the specified job has completed successfully (ran to completion with an exit code of zero).
//     | Aftercorr of string list
//     /// This job can begin execution after the specified jobs have terminated in some failed state (non-zero exit code, node failure, timed out, etc).
//     | Afternotok of string list
//     /// This job can begin execution after the specified jobs have successfully executed (ran to completion with an exit code of zero)
//     | Afterok of string list
//     /// This job can begin execution after any previously launched jobs sharing the same job name and user have terminated. In other words, only one job by that name and owned by that user can be running or suspended at any point in time. In a federation, a singleton dependency must be fulfilled on all clusters unless DependencyParameters=disable_remote_singleton is used in slurm.conf.
//     | Singleton of string list

//     static member toString (dep: DependencyType) =
//         match dep with
//         | Afterany (dep) -> sprintf "afterany:%s" (dep |> String.concat ":")
//         | Afterburstbuffer (dep) -> sprintf "afterburstbuffer:%s" (dep |> String.concat ":")
//         | Aftercorr (dep) -> sprintf "aftercorr:%s" (dep |> String.concat ":")
//         | Afternotok (dep) -> sprintf "afternotok:%s" (dep |> String.concat ":")
//         | Afterok (dep) -> sprintf "afterok:%s" (dep |> String.concat ":")
//         | Singleton (dep) -> sprintf "singleton"
//         | After (dep) ->
//             let depTimes = 
//                 dep 
//                 |> List.map (
//                     fun x ->  
//                         match (snd x) with
//                         |Some value -> sprintf "%s+%i" (fst x) value
//                         |None -> sprintf "%s" (fst x))
                
            
//                 |> String.concat (":")
//             sprintf "after:%s" depTimes


//     static member buildDependencies (typeOfDep: TypeOfDep)(depL: DependencyType[]) =
//         let sep = 
//             typeOfDep.ToString()
//         depL
//         |> Array.map (fun x -> DependencyType.toString x)
//         |> String.concat sep

// let aaa = DependencyType.buildDependencies All [|Afterany ["job1";"job2"];Afterok ["job3";"job4"]|]

type Batch = 
    {
        batches : string list
    }
    with 
        override this.ToString() =
            this.batches |> String.concat ""

type SetBurstBufferSpecification = 
    {
        batches : string list
    }
    with 
        override this.ToString() =
            this.batches |> String.concat ""
type Clusters =
    {
        clusters : string list
    }
    with 
        override this.ToString() =
            this.clusters |> String.concat ","
type Exclude = 
    {
        exclude : string list
    }
    with 
        override this.ToString() =
            this.exclude |> String.concat " "

type MemoryUnit = 
    | K
    | M
    | G
    | T

type Memory = 
    {
        memory : int
        unit : MemoryUnit
    }
    with 
        override this.ToString() =
            sprintf "%i%s" this.memory (match this.unit with | K -> "K" | M -> "M" | G -> "G" | T -> "T")


type Prefer = 
    {
        prefer : string list
    }
    with 
        override this.ToString() =
            this.prefer |> String.concat " "



type Reservation = 
    {
        reservation : string list
    }
    with 
        override this.ToString() =
            this.reservation |> String.concat ","

type WaitAllNodes = 
    {
        waitAllNodes : bool
    }
    with 
        override this.ToString() =
            sprintf "%i" (if this.waitAllNodes then 1 else 0)

type AccountingType = 
    | Task 
    | Energy
    | Network
    | Filesystem

type AccountingFreq = 
    {
        accountingType : AccountingType
        frequency : int
    }
    with 
        override this.ToString() =
            sprintf "%s=%i" (match this.accountingType with | Task -> "task" | Energy -> "energy" | Network -> "network" | Filesystem -> "filesystem") this.frequency

type AccountingFrequencies = 
    {
        accountingFrequencies : AccountingFreq list
    }
    with 
        override this.ToString() =
            this.accountingFrequencies |> List.map (fun x -> x.ToString()) |> String.concat ","
// {accountingType = Task; frequency = 1}

// {accountingFrequencies = [{accountingType = Task; frequency = 1};{accountingType = Energy; frequency = 2}]}.ToString()
type Range = 
    | Basic of int*int
    | WithStepFunction of int*int*int
    override this.ToString() =
        match this with 
        | Basic (start,stop) -> sprintf "%i-%i" start stop
        | WithStepFunction (start,stop,step) -> sprintf "%i-%i:%i" start stop step
    
type Seperators = 
    {
        seperators : int list
    }
    with 
        override this.ToString() =
            // (this.seperators |> List.map (fun x -> sprintf "%i" x) |> String.concat ",")
            (this.seperators |> List.map (fun x -> x.ToString()) |> String.concat ",")


type SimultaniousJobs = 
    {
        sim : int
    }
    with 
        override this.ToString() =
            sprintf "%%%i" this.sim


type ArraySlurm = 
    {
        seperatorsArray : Seperators option
        rangeOfValues : Range list option
        simultaniousJobsArray : SimultaniousJobs option

    }
    with 
        override this.ToString() = 
            let strArr = System.Text.StringBuilder()
            if this.seperatorsArray.IsSome then
                strArr.Append(this.seperatorsArray.Value.ToString()) |> ignore 
                strArr.Append(",") |> ignore
            if this.rangeOfValues.IsSome then
                this.rangeOfValues.Value |> List.map (fun x -> x.ToString()) |> String.concat "," |> strArr.Append |> ignore
                // strArr.Append((this.rangeOfValues.Value.ToString())) |> ignore
            if this.simultaniousJobsArray.IsSome then
                strArr.Append(this.simultaniousJobsArray.Value.ToString()) |> ignore
            strArr.ToString()

type DateFormatted =
    {
        month: int
        day: int
        year: int
    }
    override this.ToString() =
        sprintf "%02d-%02d-%02d" this.month this.day this.year

type BeginTime =
    /// (day, month, year, hour, minute, second)
    | DateTime of DateFormatted * TimeClock 
    /// (hour, minute, second)
    | OnlyTime of TimeClock
    /// ("now" + count * time unit)
    | NowPlusTime of TimeFormat3
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
    with 
        override this.ToString() = 
            match this with 
            | DateTime (date,time)-> 
                sprintf "%sT%s" (date.ToString()) (time.ToString())
            | OnlyTime (time) -> (time.ToString())
            | NowPlusTime (time) -> (time.ToString())
            | Now -> "now"
            | Midnight -> "midnight"
            | Noon -> "noon"
            | Fika -> "fika"
            | TeaTime -> "teatime"
            | Today -> "today"
            | Tomorrow -> "tomorrow"

type Deadline = 
    | Time of TimeFormat1
    | Date of Date
    | DateTime of TimeFormat2
    | Offset of TimeFormat3 
    with 
        override this.ToString() = 
            match this with 
            | Time (time) -> (time.ToString())
            | Date (date) -> (date.ToString())
            | DateTime (dateTime) -> (dateTime.ToString())
            | Offset (offset) -> (offset.ToString())

type TaskToNodes = 
    | Default
    | Block
    | Cyclic
    | Plane of int 
    | Arbitrary

type CPUsAcrossSockets = 
    | Default
    | Block 
    | Cyclic 
    | Fcyclic 

type CPUsAcrossCores = 
    | Default
    | Block
    | Cyclic
    | Fcyclic

type ControlOverNodes = 
    | Pack
    | NoPack 
/// Specify alternate distribution methods for remote processes. For job allocation, this sets environment variables that will be used by subsequent srun requests and also affects which cores will be selected for job allocation. 
type Distributions = 
    {
        DistTaskToNodes : TaskToNodes option
        DistCPUsAcrossSockets : CPUsAcrossSockets option
        DistCPUsAcrossCores : CPUsAcrossCores option
        DistControlOverNodes : ControlOverNodes option
    }
    with 
        override this.ToString() = 
            let strArr = System.Text.StringBuilder()
            if this.DistTaskToNodes.IsSome then
                strArr.Append(sprintf "%s" (match this.DistTaskToNodes.Value with | TaskToNodes.Default -> "*" | TaskToNodes.Block -> "block" | TaskToNodes.Cyclic -> "cyclic" | TaskToNodes.Plane (planes) -> ($"plane={planes}" ) | TaskToNodes.Arbitrary -> "arbitrary")) |> ignore
            if this.DistCPUsAcrossSockets.IsSome then
                strArr.Append(":") |> ignore
                strArr.Append(sprintf "%s" (match this.DistCPUsAcrossSockets.Value with | CPUsAcrossSockets.Default -> "*" | CPUsAcrossSockets.Block -> "block" | CPUsAcrossSockets.Cyclic -> "cyclic" | CPUsAcrossSockets.Fcyclic -> "fcyclic")) |> ignore
                
            if this.DistCPUsAcrossCores.IsSome then
                strArr.Append(":") |> ignore
                strArr.Append(sprintf "%s" (match this.DistCPUsAcrossCores.Value with | CPUsAcrossCores.Default -> "*" | CPUsAcrossCores.Block -> "block" | CPUsAcrossCores.Cyclic -> "cyclic" | CPUsAcrossCores.Fcyclic -> "fcyclic")) |> ignore

            if this.DistControlOverNodes.IsSome then
                strArr.Append(",") |> ignore
                strArr.Append(sprintf "%s" (match this.DistControlOverNodes.Value with | Pack -> "Pack" | NoPack -> "NoPack")) |> ignore
            strArr.ToString()

type Exclusive = 
    {
        Users: bool option
        MCS: bool option
    }
    with 
        override this.ToString() = 
            let strArr = System.Text.StringBuilder()

            if this.Users.IsSome then
                strArr.Append(sprintf "=user" ) |> ignore
            if this.MCS.IsSome then
                strArr.Append(sprintf "=mcs") |> ignore
            strArr.ToString()

type ExtraNodeInfo = 
    {
        Sockets: int option
        Cores  : int option
        Threads: int option
    }
    with 
        override this.ToString()=
            let strArr = System.Text.StringBuilder()
            if this.Sockets.IsSome then
                strArr.Append(sprintf $"{this.Sockets.Value}") |> ignore
            if this.Cores.IsSome then
                strArr.Append(sprintf ":") |> ignore
                strArr.Append(sprintf $"{this.Cores.Value}") |> ignore
            if this.Threads.IsSome then
                strArr.Append(sprintf ":") |> ignore
                strArr.Append(sprintf $"{this.Threads.Value}") |> ignore
            strArr.ToString()
type Mode = 
    | S 
    | L 
type GetUserEnv = 
    {
        Timeout : int option
        Mode    : Mode option
    }
    with 
        override this.ToString() = 
            let strArr = System.Text.StringBuilder()
            if this.Timeout.IsSome then
                strArr.Append(sprintf $"{this.Timeout.Value}") |> ignore
            if this.Mode.IsSome then
                strArr.Append(sprintf $"{this.Mode.Value}") |> ignore
            strArr.ToString()


type GPUFreqVal = 
    | Low
    | Medium
    | High 
    | Highhm1
    | Mhz of int 

type GPUMem = 
    | Low 
    | Medium
    | High
    | Highhm1

type GPUFreq = 
    {
        GPUFreq : GPUFreqVal option
        Memory  : GPUMem option
        Verbose : bool option
    }
    with 
        override this.ToString() =
            let strArr = System.Text.StringBuilder()
            if this.GPUFreq.IsSome then
                let gpuVal = 
                    (match this.GPUFreq.Value with 
                    | GPUFreqVal.Low -> "low" 
                    | GPUFreqVal.Medium -> "medium"
                    | GPUFreqVal.High -> "high"
                    | GPUFreqVal.Highhm1 -> "highm1"
                    | Mhz (mhz: int) -> sprintf "%i" mhz)
                strArr.Append(gpuVal) |> ignore
            if this.Memory.IsSome then
                strArr.Append(sprintf ",") |> ignore
                let memVal = 
                    (match this.Memory.Value with
                    | GPUMem.Low -> "low"
                    | GPUMem.Medium -> "medium"
                    | GPUMem.High -> "high"
                    | GPUMem.Highhm1 -> "highm1"
                    )
                strArr.Append(memVal) |> ignore
            if this.Verbose.IsSome then
                strArr.Append(sprintf ",verbose") |> ignore

            strArr.ToString()


type GPUTypes = 
    | Hopper
    | AdaLovelace 
    | Ampere 
    | Turing 
    | Volta
    | Pascal
    | Kepler
    | Maxwell
    | Fermi
    | Custom of string
    with 
        override this.ToString() =
            match this with 
            | Hopper -> "hopper"
            | AdaLovelace -> "adalovelace"
            | Ampere -> "ampere"
            | Turing -> "turing"
            | Volta -> "volta"
            | Pascal -> "pascal"
            | Kepler -> "kepler"
            | Maxwell -> "maxwell"
            | Fermi -> "fermi"
            | Custom st-> $"{st}"

type GPUSpez = 
    {
        GPUs : GPUTypes option
        Amount: int
    }
    with 
        override this.ToString() =
            let strArr = System.Text.StringBuilder()
            if this.GPUs.IsSome then
                strArr.Append(sprintf $"{this.GPUs.Value}") |> ignore
                strArr.Append(sprintf ":") |> ignore
            strArr.Append(sprintf $"{this.Amount}") |> ignore
            strArr.ToString()

type GPUSpezList = 
    {
        GPUSpezList : GPUSpez list
    }
    with 
        override this.ToString() =
            this.GPUSpezList |> List.map (fun x -> x.ToString()) |> String.concat ","

type Suffix = 
    | K
    | M 
    | G
    | T
    | P
    
type GresName = 
    | GPU
    | Custom of string
    with 
        override this.ToString() = 
            match this with 
            | GPU -> "gpu"
            | Custom st -> $"{st}"

type GresUnit = 
    {
        UnitName: GresName
        GPUs : GPUTypes option
        Amount: int
        Suffix: Suffix option
    }
    with 
        override this.ToString() =
            let strArr = System.Text.StringBuilder()
            strArr.Append(sprintf $"{this.UnitName}") |> ignore
            if this.GPUs.IsSome then
                strArr.Append(sprintf ":") |> ignore
                strArr.Append(sprintf $"{this.GPUs.Value}") |> ignore
                strArr.Append(sprintf ":") |> ignore

            strArr.Append(sprintf $"{this.Amount}") |> ignore
            if this.Suffix.IsSome then 
                strArr.Append(sprintf $"{this.Suffix.Value}") |> ignore
            strArr.ToString()


type Gres = 
    {
        Gres : GresUnit list
    }
    with 
        override this.ToString() =
            let gresL = this.Gres |> List.map (fun x -> x.ToString()) |> String.concat ","
            sprintf "%s" gresL


// type GresOption = 
//     | GresList of Gres 
//     | None 
//     with 
//         override this.ToString() = 
//             match this with 
//             | GresList (gres) -> (sprintf "%s" (gres.ToString()))
//             | None -> "none"


// (GresOption.GresList {Gres = [{UnitName = GPU; GPUs = Some Kepler; Amount = 2; Suffix = Some K};{UnitName = GresName.Custom "Unit"; GPUs = Some AdaLovelace; Amount = 4; Suffix = Some M}]}).ToString()

// let gpspezl = {GPUSpezList = [{GPUs = Some Kepler; Amount = 2};{GPUs = Some AdaLovelace; Amount = 4};{GPUs = Some Ampere; Amount = 8}]}.ToString()
type GresFlags = 
    | DisableBinding
    | EnforceBinding
    with 
        override this.ToString() = 
            match this with 
            | DisableBinding -> "disable-binding"
            | EnforceBinding -> "enforce-binding"

type Hint = 
    | ComputeBound
    | MemoryBound
    | Multithread
    | NoMultithread
    with
        override this.ToString() = 
            match this with 
            | ComputeBound -> "compute_bound"
            | MemoryBound -> "memory_bound"
            | Multithread -> "multithread"
            | NoMultithread -> "nomultithread"

// {DistTaskToNodes= Some (TaskToNodes.Plane 2); DistCPUsAcrossSockets = Some CPUsAcrossSockets.Fcyclic; DistCPUsAcrossCores = Some CPUsAcrossCores.Fcyclic; DistControlOverNodes = Some NoPack}.ToString()

// Deadline.Date({month = 1; day = 1; year = Some 2021}).ToString()
// Deadline.Time({clock = {hour = 1; minute = 1; second = Some 1}; amPm = Some AM}).ToString()
// Deadline.DateTime({date = {month = 1; day = 1; year = Some 2021}; time = {hour = 1; minute = 1; second = Some 1}}).ToString()
// Deadline.Offset({count = Some 1; unit = Some Seconds}).ToString()

type Immediate = 
    {
        duration: int  
    }
    with 
        override this.ToString() = 
            sprintf $"I{this.duration}"


type License = 
    {
        license: string
        db: string option
        count: int option  
    }
    with 
        override this.ToString()=
            let strArr = System.Text.StringBuilder()
            strArr.Append(sprintf $"{this.license}") |> ignore
            if this.db.IsSome then
                strArr.Append(sprintf $"@{this.db.Value}") |> ignore
            if this.count.IsSome then
                strArr.Append(sprintf $":{this.count.Value}") |> ignore
            strArr.ToString()


type LicenseList = 
    {
        licenses: License list
    }
    with 
        override this.ToString()= 
            this.licenses |> List.map (fun x -> x.ToString()) |> String.concat ","    

type MailType = 
    | NONE
    | BEGIN
    | END
    | FAIL
    | REQUEUE
    | ALL
    | INVALID_DEPEND
    | STAGE_OUT
    | TIME_LIMIT
    | TIME_LIMIT_90
    | TIME_LIMIT_80
    | TIME_LIMIT_50

type MailTypeList = 
    {
        mailTypes: MailType list
    }
    with 
        override this.ToString()= 
            this.mailTypes |> List.map (fun x -> x.ToString()) |> String.concat ","


type Priority = 
    | TOP 
    | Val of int
    with 
        override this.ToString() = 
            match this with 
            | TOP -> "TOP"
            | Val (v) -> sprintf "%i" v
type OnlyKey (jobName:string) = 
        inherit DynamicObj()
        member val Name = jobName with get, set

        
        /// Do not automatically terminate a job if one of the nodes it has been allocated fails. This option applies to job and step allocations. The job will assume all responsibilities for fault-tolerance. Tasks launched using this option will not be considered terminated (e.g. -K, --kill-on-bad-exit and -W, --wait options will have no effect upon the job step). The active job step (MPI job) will likely suffer a fatal error, but subsequent job steps may be run if this option is specified. 
        static member SetNoKill
            ([<Optional; DefaultParameterValue(null)>]?NoKill:bool) =
                (fun (job: OnlyKey) ->
                    NoKill  |> DynObj.setValueOpt job "k"
                    job
                )
        static member tryGetNoKill (job: OnlyKey) =
            job.TryGetValue "k"

        static member removeNoKill(job: OnlyKey) =
            job.Remove "k" 
        /// Addition to NoKill. Specify an optional argument of "off" disable the effect of the SLURM_NO_KILL environment variable. 
        static member SetNoKillWithDisable
            ([<Optional; DefaultParameterValue(null)>]?NoKill:bool) =
                (fun (job: OnlyKey) ->
                    NoKill  |> DynObj.setValueOpt job "k=off"
                    job
                )
        static member tryGetNoKillWithDisable (job: OnlyKey) =
            job.TryGetValue "k=off"

        static member removeNoKillWithDisable(job: OnlyKey) =
            job.Remove "k=off"   
        static member SetKillOnBadExit
            ([<Optional; DefaultParameterValue(null)>]?KillOnBadExit:bool) =
                (fun (job: OnlyKey) ->
                    KillOnBadExit  |> DynObj.setValueOpt job "K=1"
                    job
                )
        static member tryGetKillOnBadExit (job: OnlyKey) =
            job.TryGetValue "K=1"

        static member removeKillOnBadExit(job: OnlyKey) =
            job.Remove "K=1"    
        /// Do not exit until the submitted job terminates. The exit code of the sbatch command will be the same as the exit code of the submitted job. If the job terminated due to a signal rather than a normal exit, the exit code will be set to 1. In the case of a job array, the exit code recorded will be the highest value for any task in the job array. 
        static member SetWait
            ([<Optional; DefaultParameterValue(null)>]?Wait:bool) =
                (fun (job: OnlyKey) ->
                    Wait  |> DynObj.setValueOpt job "wait"
                    job
                )
        static member tryGetWait (job: OnlyKey) =
            job.TryGetValue "wait"

        static member removeWait (job: OnlyKey) =
            job.Remove "wait"
        
        /// Increase the verbosity of sbatch's informational messages. Multiple -v's will further increase sbatch's verbosity. By default only errors will be displayed. 
        static member SetVerbose
            ([<Optional; DefaultParameterValue(null)>]?Verbose:bool) =
                (fun (job: OnlyKey) ->
                    Verbose  |> DynObj.setValueOpt job "verbose"
                    job
                )
        static member tryGetVerbose (job: OnlyKey) =
            job.TryGetValue "verbose"

        static member removeVerbose (job: OnlyKey) =
            job.Remove "verbose"
        /// If a range of node counts is given, prefer the smaller count. 
        static member SetUseMinNodes
            ([<Optional; DefaultParameterValue(null)>]?UseMinNodes:bool) =
                (fun (job: OnlyKey) ->
                    UseMinNodes  |> DynObj.setValueOpt job "use-min-nodes"
                    job
                )
        static member tryGetUseMinNodes (job: OnlyKey) =
            job.TryGetValue "use-min-nodes"
            
        static member removeUseMinNodes (job: OnlyKey) =
            job.Remove "use-min-nodes"
        /// Validate the batch script and return an estimate of when a job would be scheduled to run given the current job queue and all the other arguments specifying the job requirements. No job is actually submitted. 
        static member SetTestOnly
            ([<Optional; DefaultParameterValue(null)>]?TestOnly:bool) =
                (fun (job: OnlyKey) ->
                    TestOnly  |> DynObj.setValueOpt job "test-only"
                    job
                )
        static member tryGetTestOnly (job: OnlyKey) =
            job.TryGetValue "test-only"

        static member removetestOnly (job: OnlyKey) =
            job.Remove "test-only"

        /// Spread the job allocation over as many nodes as possible and attempt to evenly distribute tasks across the allocated nodes. This option disables the topology/tree plugin. 
        static member SetSpreadJob
            ([<Optional; DefaultParameterValue(null)>]?SpreadJob:bool) =
                (fun (job: OnlyKey) ->
                    SpreadJob  |> DynObj.setValueOpt job "spread-job"
                    job
                )
        static member tryGetSpreadJob (job: OnlyKey) =
            job.TryGetValue "spread-job"

        static member removeSpreadJob (job: OnlyKey) =
            job.Remove "spread-job"
        /// Specifies that the batch job should be eligible for requeuing. The job may be requeued explicitly by a system administrator, after node failure, or upon preemption by a higher priority job. When a job is requeued, the batch script is initiated from its beginning.
        static member SetRequeue
            ([<Optional; DefaultParameterValue(null)>]?Requeue:bool) =
                (fun (job: OnlyKey) ->
                    Requeue  |> DynObj.setValueOpt job "requeue"
                    job
                )
        static member tryGetRequeue (job: OnlyKey) =
            job.TryGetValue "requeue"

        static member removeRequeue (job: OnlyKey) =
            job.Remove "requeue"

        /// Force the allocated nodes to reboot before starting the job. This is only supported with some system configurations and will otherwise be silently ignored. Only root, SlurmUser or admins can reboot nodes. 
        static member SetReboot
            ([<Optional; DefaultParameterValue(null)>]?Reboot:bool) =
                (fun (job: OnlyKey) ->
                    Reboot  |> DynObj.setValueOpt job "reboot"
                    job
                )
        static member tryGetReboot (job: OnlyKey) =
            job.TryGetValue "reboot"

        static member removeReboot (job: OnlyKey) =
            job.Remove "reboot"
        /// Suppress informational messages from sbatch such as Job ID. Only errors will still be displayed. 
        static member SetQuiet
            ([<Optional; DefaultParameterValue(null)>]?Quiet:bool) =
                (fun (job: OnlyKey) ->
                    Quiet  |> DynObj.setValueOpt job "quiet"
                    job
                )
        static member tryGetQuiet (job: OnlyKey) =
            job.TryGetValue "quiet"

        static member removeQuiet (job: OnlyKey) =
            job.Remove "quiet"
        /// The job allocation can over-subscribe resources with other running jobs. The resources to be over-subscribed can be nodes, sockets, cores, and/or hyperthreads depending upon configuration. The default over-subscribe behavior depends on system configuration and the partition's OverSubscribe option takes precedence over the job's option.
        static member SetOversubscribe
            ([<Optional; DefaultParameterValue(null)>]?Oversubscribe:bool) =
                (fun (job: OnlyKey) ->
                    Oversubscribe  |> DynObj.setValueOpt job "oversubscribe"
                    job
                )
        static member tryGetOversubscribe (job: OnlyKey) =
            job.TryGetValue "oversubscribe"

        static member removeOversubscribe (job: OnlyKey) =
            job.Remove "oversubscribe"
        /// Overcommit resources. When applied to a job allocation (not including jobs requesting exclusive access to the nodes) the resources are allocated as if only one task per node is requested. This means that the requested number of cpus per task (-c, --cpus-per-task) are allocated per node rather than being multiplied by the number of tasks. Options used to specify the number of tasks per node, socket, core, etc. are ignored. 
        static member SetOvercommit
            ([<Optional; DefaultParameterValue(null)>]?Overcommit:bool) =
                (fun (job: OnlyKey) ->
                    Overcommit  |> DynObj.setValueOpt job "overcommit"
                    job
                )
        static member tryGetOvercommit (job: OnlyKey) =
            job.TryGetValue "overcommit"

        static member removeOvercommit (job: OnlyKey) =
            job.Remove "overcommit"
        /// Specifies that the batch job should never be requeued under any circumstances.
        static member SetNoRequeue
            ([<Optional; DefaultParameterValue(null)>]?NoRequeue:bool) =
                (fun (job: OnlyKey) ->
                    NoRequeue  |> DynObj.setValueOpt job "no-requeue"
                    job
                )
        static member tryGetNoRequeue (job: OnlyKey) =
            job.TryGetValue "no-requeue"

        static member removeNoRequeue (job: OnlyKey) =
            job.Remove "no-requeue"

        
        /// If a job has an invalid dependency and it can never run this parameter tells Slurm to terminate it or not.
        static member SetKillOnInvalidDep
            ([<Optional; DefaultParameterValue(null)>]?KillOnInvalidDep:bool) =
                (fun (job: OnlyKey) ->
                    KillOnInvalidDep  |> DynObj.setValueOpt job "kill-on-invalid-dep"
                    job
                )
        static member tryGetKillOnInvalidDep (job: OnlyKey) =
            job.TryGetValue "kill-on-invalid-dep"

        static member removeKillOnInvalidDep (job: OnlyKey) =
            job.Remove "kill-on-invalid-dep"

        /// Ignore all "#PBS" and "#BSUB" options specified in the batch script. 
        static member SetIgnorePBS
            ([<Optional; DefaultParameterValue(null)>]?IgnorePBS:bool) =
                (fun (job: OnlyKey) ->
                    IgnorePBS  |> DynObj.setValueOpt job "ignore-pbs"
                    job
                )
        static member tryGetIgnorePBS (job: OnlyKey) =
            job.TryGetValue "ignore-pbs"
        static member removeIgnorePBS (job: OnlyKey) =
            job.Remove "ignore-pbs"
        /// Specify the job is to be submitted in a held state (priority of zero). A held job can now be released using scontrol to reset its priority (e.g. "scontrol release <job_id>"). 
        static member SetHold
            ([<Optional; DefaultParameterValue(null)>]?Hold:bool) =
                (fun (job: OnlyKey) ->
                    Hold  |> DynObj.setValueOpt job "hold"
                    job
                )
        static member tryGetHold (job: OnlyKey) =
            job.TryGetValue "hold"
        static member removeHold (job: OnlyKey) =
            job.Remove "hold"
        static member SetParsable
            ([<Optional; DefaultParameterValue(null)>]?Parsable:bool) =
                (fun (job: OnlyKey) ->
                    Parsable  |> DynObj.setValueOpt job "parsable"
                    job
                )
        static member tryGetParsable (job: OnlyKey) =
            job.TryGetValue "parsable"

        static member removeParsable(job: OnlyKey) =
            job.Remove "parsable"

        /// If set, then the allocated nodes must form a contiguous set. 
        static member SetContiguous
            ([<Optional; DefaultParameterValue(null)>]?Contiguous:bool) =
                (fun (job: OnlyKey) ->
                    Contiguous  |> DynObj.setValueOpt job "contiguous"
                    job
                )
        static member tryGetContiguous (job: OnlyKey) =
            job.TryGetValue "contiguous"

        static member removeContiguous (job: OnlyKey) =
            job.Remove "contiguous"

type ShortCommand (jobName: string) =
        inherit DynamicObj()
        member val Name = jobName with get, set

        static member SetLicenses
            ([<Optional; DefaultParameterValue(null)>]?Licenses:(LicenseList)) =
                (fun (job: ShortCommand) ->

                    Licenses |> DynObj.setValueOpt job "L"
                    job
                )
        static member tryGetLicenses (job: ShortCommand) =
            job.TryGetValue "L"

        static member removeLicenses (job: ShortCommand) =
            job.Remove "L"
        static member SetGPU
            ([<Optional; DefaultParameterValue(null)>]?GPU:(GPUSpez)) =
                (fun (job: ShortCommand) ->

                    GPU |> DynObj.setValueOpt job "G"
                    job
                )
        static member tryGetGPU (job: ShortCommand) =
            job.TryGetValue "G"

        static member removeGPU (job: ShortCommand) =
            job.Remove "G"


        static member SetExtraNodeInfo
            ([<Optional; DefaultParameterValue(null)>]?ExtraNodeInfo:(ExtraNodeInfo)) =
                (fun (job: ShortCommand) ->

                    ExtraNodeInfo |> DynObj.setValueOpt job "B"
                    job
                )
        static member tryGetExtraNodeInfo (job: ShortCommand) =
            job.TryGetValue "B"

        static member removeExtraNodeInfo (job: ShortCommand) =
            job.Remove "B"
        static member SetDistribution
            ([<Optional; DefaultParameterValue(null)>]?Distribution:(Distributions)) =
                (fun (job: ShortCommand) ->

                    Distribution |> DynObj.setValueOpt job "m"
                    job
                )
        static member tryGetDistribution (job: ShortCommand) =
            job.TryGetValue "m"

        static member removeDistribution (job: ShortCommand) =
            job.Remove "m"
        static member SetBegin
            ([<Optional; DefaultParameterValue(null)>]?BeginTime:(BeginTime)) =
                (fun (job: ShortCommand) ->

                    BeginTime |> DynObj.setValueOpt job "b"
                    job
                )
        static member tryGetBegin (job: ShortCommand) =
            job.TryGetValue "b"

        static member removeBegin (job: ShortCommand) =
            job.Remove "b"
        static member SetArray
            ([<Optional; DefaultParameterValue(null)>]?Array:(ArraySlurm)) =
                (fun (job: ShortCommand) ->

                    Array |> DynObj.setValueOpt job "a"
                    job
                )
        static member tryGetArray (job: ShortCommand) =
            job.TryGetValue "a"

        static member removeArray (job: ShortCommand) =
            job.Remove "a"
        /// Instruct Slurm to connect the batch script's standard input directly to the file name specified in the "filename pattern".
        static member SetInput
            ([<Optional; DefaultParameterValue(null)>]?Input:string) =
                (fun (job: ShortCommand) ->
                    Input  |> DynObj.setValueOpt job "i"
                    job
                )
        static member tryGetInput (job: ShortCommand) =
            job.TryGetValue "i"

        static member removeInput (job: ShortCommand) =
            job.Remove "i"
        static member SetExclude
            ([<Optional; DefaultParameterValue(null)>]?Exclude:Exclude) =
                (fun (job: ShortCommand) ->
                    Exclude  |> DynObj.setValueOpt job "x"
                    job
                )
        static member tryGetExclude (job: ShortCommand) =
            job.TryGetValue "x"

        static member removeExclude (job: ShortCommand) =
            job.Remove "x"
        /// Count of Specialized Cores per node reserved by the job for system operations and not used by the application.
        static member SetSpezializedCores
            ([<Optional; DefaultParameterValue(null)>]?SpezializedCores:int) =
                (fun (job: ShortCommand) ->
                    SpezializedCores  |> DynObj.setValueOpt job "S"
                    job
                )
        static member tryGetSpezializedCores (job: ShortCommand) =
            job.TryGetValue "S"
        static member removeSpezializedCores (job: ShortCommand) =
            job.Remove "S"
        
        static member SetClusters
            ([<Optional; DefaultParameterValue(null)>]?Clusters:Clusters) =
                (fun (job: ShortCommand) ->
                    Clusters  |> DynObj.setValueOpt job "M"
                    job
                )
        static member tryGetClusters (job: ShortCommand) =
            job.TryGetValue "M"

        static member removeClusters (job: ShortCommand) =
            job.Remove "M"
        /// Set the working directory of the batch script to directory before it is executed. The path can be specified as full path or relative path to the directory where the command is executed. 
        static member SetWorkingDirectory
            ([<Optional; DefaultParameterValue(null)>]?WorkingDirectory:string) =
                (fun (job: ShortCommand) ->
                    WorkingDirectory  |> DynObj.setValueOpt job "D"
                    job
                )
        static member tryGetWorkingDirectory (job: ShortCommand) =
            job.TryGetValue "D"

        static member removeWorkingDirectory (job: ShortCommand) =
            job.Remove "D"
        static member SetAccount
            ([<Optional; DefaultParameterValue(null)>]?Account:string) =
                (fun (job: ShortCommand) ->
                    Account  |> DynObj.setValueOpt job "A"
                    job
                )
        static member tryGetAccount (job: ShortCommand) =
            job.TryGetValue "A"

        static member removeAccount (job: ShortCommand) =
            job.Remove "A"
        /// Request that a minimum of minnodes nodes be allocated to this job.

        static member SetNode
            ([<Optional; DefaultParameterValue(null)>]?Node: string) =
                (fun (job: ShortCommand) ->

                    Node |> DynObj.setValueOpt job "N"
                    job
                )
        static member tryGetNode (job: ShortCommand) =
            job.TryGetValue "N"

        static member removeNode (job: ShortCommand) =
            job.Remove "N"

        /// Instruct Slurm to connect the batch script's standard output directly to the file name specified in the "filename pattern"
        static member SetOutput
            ([<Optional; DefaultParameterValue(null)>]?Output: string) =
                (fun (job: ShortCommand) ->
                    Output |> DynObj.setValueOpt job "o"
                    job
                )
        static member tryGetOutput (job: ShortCommand) =
            job.TryGetValue "o"

        static member removeOutput (job: ShortCommand) =
            job.Remove "o"

        /// Instruct Slurm to connect the batch script's standard error directly to the file name specified in the "filename pattern".
        static member SetError
            ([<Optional; DefaultParameterValue(null)>]?Error: string) =
                (fun (job: ShortCommand) ->
                    Error |> DynObj.setValueOpt job "e"
                    job
                )
        static member tryGetError (job: ShortCommand) =
            job.TryGetValue "e"

        static member removeError (job: ShortCommand) =
            job.Remove "e"

        /// Request a specific partition for the resource allocation.
        static member SetPartition
            ([<Optional; DefaultParameterValue(null)>]?Partition:string) =
                (fun (job: ShortCommand) ->
                    Partition |> DynObj.setValueOpt job "p"
                    job
                )
        static member tryGetPartition (job: ShortCommand) =
            job.TryGetValue "p"

        static member removePartition (job: ShortCommand) =
            job.Remove "p"

type LongCommand  (jobName: string) =
    inherit DynamicObj()
    member val Name = jobName with get, set
    static member SetDependency
        ([<Optional; DefaultParameterValue(null)>]?Dependency:string) =
            (fun (job: LongCommand) ->

                Dependency |> DynObj.setValueOpt job "dependency"
                job
            )
    static member tryGetDependency(job: LongCommand) =
        job.TryGetValue "dependency"

    static member removeDependency(job: LongCommand) =
        job.Remove "dependency"
    static member SetPriority
        ([<Optional; DefaultParameterValue(null)>]?Priority:Priority) =
            (fun (job: LongCommand) ->

                Priority |> DynObj.setValueOpt job "priority"
                job
            )
    static member tryGetPriority(job: LongCommand) =
        job.TryGetValue "priority"

    static member removePriority(job: LongCommand) =
        job.Remove "priority"
    static member SetNice
        ([<Optional; DefaultParameterValue(null)>]?Nice:int) =
            (fun (job: LongCommand) ->

                Nice |> DynObj.setValueOpt job "nice"
                job
            )
    static member tryGetNice(job: LongCommand) =
        job.TryGetValue "nice"

    static member removeNice(job: LongCommand) =
        job.Remove "nice"
    static member SetHint
        ([<Optional; DefaultParameterValue(null)>]?Hint:Hint) =
            (fun (job: LongCommand) ->

                Hint |> DynObj.setValueOpt job "hint"
                job
            )
    static member tryGetHint(job: LongCommand) =
        job.TryGetValue "hint"

    static member removeHint(job: LongCommand) =
        job.Remove "hint"

    /// Specify generic resource task binding options.
    static member SetGresFlag
        ([<Optional; DefaultParameterValue(null)>]?GresFlag:GresFlags) =
            (fun (job: LongCommand) ->

                GresFlag |> DynObj.setValueOpt job "gres-flags"
                job
            )
    static member tryGetGresFlag(job: LongCommand) =
        job.TryGetValue "gres-flags"

    static member removeGresFlag(job: LongCommand) =
        job.Remove "gres-flags"
    static member SetGres
        ([<Optional; DefaultParameterValue(null)>]?Gres:Gres) =
            (fun (job: LongCommand) ->

                Gres |> DynObj.setValueOpt job "gres"
                job
            )
    static member tryGetGres(job: LongCommand) =
        job.TryGetValue "gres"

    static member removeGres(job: LongCommand) =
        job.Remove "gres"
    static member SetGPUPerTask
        ([<Optional; DefaultParameterValue(null)>]?GPUPerTask:GPUSpezList) =
            (fun (job: LongCommand) ->

                GPUPerTask |> DynObj.setValueOpt job "gpus-per-task"
                job
            )
    static member tryGetGPUPerTask(job: LongCommand) =
        job.TryGetValue "gpus-per-task"

    static member removeGPUPerTask(job: LongCommand) =
        job.Remove "gpus-per-task"
    static member SetGPUPerSocket
        ([<Optional; DefaultParameterValue(null)>]?GPUPerSocket:GPUSpezList) =
            (fun (job: LongCommand) ->

                GPUPerSocket |> DynObj.setValueOpt job "gpus-per-socket"
                job
            )
    static member tryGetGPUPerSocket(job: LongCommand) =
        job.TryGetValue "gpus-per-socket"

    static member removeGPUPerSocket(job: LongCommand) =
        job.Remove "gpus-per-socket"
    static member SetGPUPerNode
        ([<Optional; DefaultParameterValue(null)>]?GPUPerNode:GPUSpezList) =
            (fun (job: LongCommand) ->

                GPUPerNode |> DynObj.setValueOpt job "gpus-per-node"
                job
            )
    static member tryGetGPUPerNode(job: LongCommand) =
        job.TryGetValue "gpus-per-node"

    static member removeGPUPerNode(job: LongCommand) =
        job.Remove "gpus-per-node"

    static member SetGPUFreq
        ([<Optional; DefaultParameterValue(null)>]?GPUFreq:GPUFreq) =
            (fun (job: LongCommand) ->

                GPUFreq |> DynObj.setValueOpt job "gpu-freq"
                job
            )
    static member tryGetGPUFreq (job: LongCommand) =
        job.TryGetValue "gpu-freq"

    static member removeGPUFreq(job: LongCommand) =
        job.Remove "gpu-freq"
    static member SetExclusive
        ([<Optional; DefaultParameterValue(null)>]?Exclusive:Exclusive) =
            (fun (job: LongCommand) ->

                Exclusive |> DynObj.setValueOpt job "exclusive"
                job
            )
    static member tryGetExclusive (job: LongCommand) =
        job.TryGetValue "Exclusive"

    static member removeExclusive(job: LongCommand) =
        job.Remove "Exclusive"
    static member SetDeadline
        ([<Optional; DefaultParameterValue(null)>]?Deadline:Deadline) =
            (fun (job: LongCommand) ->

                Deadline |> DynObj.setValueOpt job "deadline"
                job
            )
    static member tryGetDeadline (job: LongCommand) =
        job.TryGetValue "deadline"

    static member removeDeadline(job: LongCommand) =
        job.Remove "deadline"
    static member SetAccountingFrequency
        ([<Optional; DefaultParameterValue(null)>]?AccountingFrequency:AccountingFrequencies) =
            (fun (job: LongCommand) ->

                AccountingFrequency |> DynObj.setValueOpt job "acctg-freq"
                job
            )
    static member tryGetAccountingFrequency (job: LongCommand) =
        job.TryGetValue "acctg-freq"

    static member removeAccountingFrequency(job: LongCommand) =
        job.Remove "acctg-freq"
    // /// Sbatch will wrap the specified command string in a simple "sh" shell script, and submit that script to the slurm controller. When --wrap is used, a script name and arguments may not be specified on the command line; instead the sbatch-generated wrapper script is used. 
    // static member SetWrap
    //     ([<Optional; DefaultParameterValue(null)>]?Wrap:string list) =
    //         (fun (job: LongCommand) ->
    //             Wrap  |> DynObj.setValueOpt job "--wrap="
    //             job
    //         )
    // static member tryGetWrap (job: LongCommand) =
    //     job.TryGetValue "--wrap="

    // static member removeWrap (job: LongCommand) =
    //     job.Remove "--wrap="
    
    /// Controls when the execution of the command begins. By default the job will begin execution as soon as the allocation is made.
    static member SetWaitAllNodes
        ([<Optional; DefaultParameterValue(null)>]?WaitAllNodes:bool) =
            (fun (job: LongCommand) ->
                WaitAllNodes  |> DynObj.setValueOpt job "wait-all-nodes"
                job
            )
    static member tryGetWaitAllNodes (job: LongCommand) =
        job.TryGetValue "wait-all-nodes"

    static member removeWaitAllNodes (job: LongCommand) =
        job.Remove "wait-all-nodes"
    
    /// Attempt to submit and/or run a job as user instead of the invoking user id. The invoking user's credentials will be used to check access permissions for the target partition. User root may use this option to run jobs as a normal user in a RootOnly partition for example. If run as root, sbatch will drop its permissions to the uid specified after node allocation is successful. user may be the user name or numerical user ID. 
    static member SetUserID
        ([<Optional; DefaultParameterValue(null)>]?UserID:string) =
            (fun (job: LongCommand) ->
                UserID  |> DynObj.setValueOpt job "uid"
                job
            )
    static member tryGetUserID (job: LongCommand) =
        job.TryGetValue "uid"

    static member removeUserID (job: LongCommand) =
        job.Remove "uid"
    /// Set a minimum time limit on the job allocation. Format is (day,hours,minutes,seconds). If specified, the job may have its --time limit lowered to a value no lower than --time-min if doing so permits the job to begin execution earlier than otherwise possible. 
    static member SetMinTime
        ([<Optional; DefaultParameterValue(null)>]?MinTime:Time) =
            (fun (job: LongCommand) ->
                MinTime |> DynObj.setValueOpt job "time-min"
                job
            )
    static member tryGetMinTime (job: LongCommand) =
        job.TryGetValue "time-min"

    static member removeMinTime (job: LongCommand) =
        job.Remove "time-min"
    /// Restrict node selection to nodes with at least the specified number of threads per core. In task layout, use the specified maximum number of threads per core. NOTE: "Threads" refers to the number of processing units on each core rather than the number of application tasks to be launched per core.
    static member SetThreadsPerCore
        ([<Optional; DefaultParameterValue(null)>]?ThreadsPerCore:int) =
            (fun (job: LongCommand) ->
                ThreadsPerCore  |> DynObj.setValueOpt job "threads-per-core"
                job
            )
    static member tryGetThreadsPerCore (job: LongCommand) =
        job.TryGetValue "threads-per-core"

    static member removeThreadsPerCore (job: LongCommand) =
        job.Remove "threads-per-core"
    
    /// Count of specialized threads per node reserved by the job for system operations and not used by the application. The application will not use these threads, but will be charged for their allocation. This option can not be used with the --core-spec option. 
    static member SetThreadSpec
        ([<Optional; DefaultParameterValue(null)>]?ThreadSpec:int) =
            (fun (job: LongCommand) ->
                ThreadSpec  |> DynObj.setValueOpt job "thread-spec"
                job
            )
    static member tryGetThreadSpec (job: LongCommand) =
        job.TryGetValue "thread-spec"

    static member removeThreadSpec (job: LongCommand) =
        job.Remove "thread-spec"

    ///Restrict node selection to nodes with at least the specified number of sockets. See additional information under -B option above when task/affinity plugin is enabled. NOTE: This option may implicitly set the number of tasks (if -n was not specified) as one task per requested thread.
    static member SetSocketsPerNode
        ([<Optional; DefaultParameterValue(null)>]?SocketsPerNode:int) =
            (fun (job: LongCommand) ->
                SocketsPerNode  |> DynObj.setValueOpt job "sockets-per-node"
                job
            )
    static member tryGetSocketsPerNode (job: LongCommand) =
        job.TryGetValue "sockets-per-node"

    static member removeSocketsPerNode (job: LongCommand) =
        job.Remove "sockets-per-node"
    /// Allocate resources for the job from the named reservation.
    static member SetReservation
        ([<Optional; DefaultParameterValue(null)>]?Reservation:Reservation) =
            (fun (job: LongCommand) ->
                Reservation  |> DynObj.setValueOpt job "reservation"
                job
            )
    static member tryGetReservation (job: LongCommand) =
        job.TryGetValue "reservation"

    static member removeReservation (job: LongCommand) =
        job.Remove "reservation"

    /// Nodes can have features assigned to them by the Slurm administrator. Users can specify which of these features are desired but not required by their job using the prefer option.
    static member SetPrefer
        ([<Optional; DefaultParameterValue(null)>]?Prefer:string list) =
            (fun (job: LongCommand) ->
                Prefer  |> DynObj.setValueOpt job "prefer"
                job
            )
    static member tryGetPrefer (job: LongCommand) =
        job.TryGetValue "prefer"

    static member removePrefer (job: LongCommand) =
        job.Remove "prefer"
    
    /// Request the maximum ntasks be invoked on each socket. Meant to be used with the --ntasks option. Related to --ntasks-per-node except at the socket level instead of the node level. 
    static member SetNTasksPerSocket
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerSocket:int) =
            (fun (job: LongCommand) ->
                NTasksPerSocket  |> DynObj.setValueOpt job "ntasks-per-socket"
                job
            )
    static member tryGetNTasksPerSocket (job: LongCommand) =
        job.TryGetValue "ntasks-per-socket"

    static member removeNTasksPerSocket (job: LongCommand) =
        job.Remove "ntasks-per-socket"
    /// Request that ntasks be invoked on each node. If used with the --ntasks option, the --ntasks option will take precedence and the --ntasks-per-node will be treated as a maximum count of tasks per node. Meant to be used with the --nodes option. This is related to --cpus-per-task=ncpus, but does not require knowledge of the actual number of cpus on each node.
    static member SetNTasksPerNode
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerNode:int) =
            (fun (job: LongCommand) ->
                NTasksPerNode  |> DynObj.setValueOpt job "ntasks-per-node"
                job
            )
    static member tryGetNTasksPerNode (job: LongCommand) =
        job.TryGetValue "ntasks-per-node"

    static member removeNTasksPerNode (job: LongCommand) =
        job.Remove "ntasks-per-node"
    
    
    /// Request that there are ntasks tasks invoked for every GPU. This option can work in two ways: 1) either specify --ntasks in addition, in which case a type-less GPU specification will be automatically determined to satisfy --ntasks-per-gpu, or 2) specify the GPUs wanted (e.g. via --gpus or --gres) without specifying --ntasks, and the total task count will be automatically determined. 
    static member SetNTasksPerGPU
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerGPU:int) =
            (fun (job: LongCommand) ->
                NTasksPerGPU  |> DynObj.setValueOpt job "ntasks-per-gpu"
                job
            )
    static member tryGetNTasksPerGPU (job: LongCommand) =
        job.TryGetValue "ntasks-per-gpu"

    static member removeNTasksPerGPU (job: LongCommand) =
        job.Remove "ntasks-per-gpu"
    
    /// Request the maximum ntasks be invoked on each core. Meant to be used with the --ntasks option. Related to --ntasks-per-node except at the core level instead of the node level.
    static member SetNTasksPerCore
        ([<Optional; DefaultParameterValue(null)>]?NTasksPerCore:int) =
            (fun (job: LongCommand) ->
                NTasksPerCore  |> DynObj.setValueOpt job "ntasks-per-core"
                job
            )
    static member tryGetNTasksPerCore (job: LongCommand) =
        job.TryGetValue "ntasks-per-core"

    static member removeNTasksPerCore (job: LongCommand) =
        job.Remove "ntasks-per-core"
    /// Much like --nodelist, but the list is contained in a file of name node file.
    static member SetNodeFile
        ([<Optional; DefaultParameterValue(null)>]?NodeFile:string) =
            (fun (job: LongCommand) ->
                NodeFile  |> DynObj.setValueOpt job "nodefile"
                job
            )
    static member tryGetNodeFile (job: LongCommand) =
        job.TryGetValue "nodefile"

    static member removeNodeFile (job: LongCommand) =
        job.Remove "nodefile"
    /// Specify a minimum number of logical cpus/processors per node. 
    static member SetMinCPUs
        ([<Optional; DefaultParameterValue(null)>]?MinCPUs:int) =
            (fun (job: LongCommand) ->
                MinCPUs  |> DynObj.setValueOpt job "mincpus"
                job
            )
    static member tryGetMinCPUs (job: LongCommand) =
        job.TryGetValue "mincpus"

    static member removeMinCPUs (job: LongCommand) =
        job.Remove "mincpus"
    /// Minimum memory required per usable allocated CPU (with unit, e.g. "30gb"). 
    static member SetMemoryPerCPU
        ([<Optional; DefaultParameterValue(null)>]?MemoryPerCPU:Memory) =
            (fun (job: LongCommand) ->
                MemoryPerCPU |> DynObj.setValueOpt job "mem-per-cpu"
                job
            )
    static member tryGetMemoryPerCPU (job: LongCommand) =
        job.TryGetValue "mem-per-cpu"

    static member removeMemoryPerCPU (job: LongCommand) =
        job.Remove "mem-per-cpu"
    /// Minimum memory required per allocated GPU (with unit, e.g. "30gb").
    static member SetMemoryPerGPU
        ([<Optional; DefaultParameterValue(null)>]?MemoryPerGPU:Memory) =
            (fun (job: LongCommand) ->
                MemoryPerGPU |> DynObj.setValueOpt job "mem-per-gpu"
                job
            )
    static member tryGetMemoryPerGPU (job: LongCommand) =
        job.TryGetValue "mem-per-gpu"

    static member removememoryPerGPU (job: LongCommand) =
        job.Remove "mem-per-gpu"
    ///User to receive email notification of state changes as defined by --mail-type. The default value is the submitting user.
    static member SetMailUser
        ([<Optional; DefaultParameterValue(null)>]?MailUser:string) =
            (fun (job: LongCommand) ->
                MailUser  |> DynObj.setValueOpt job "mail-user"
                job
            )
    static member tryGetMailUser (job: LongCommand) =
        job.TryGetValue "mail-user"

    static member removeMailUser (job: LongCommand) =
        job.Remove "mail-user"
    /// If sbatch is run as root, and the --gid option is used, submit the job with group's group access permissions. group may be the group name or the numerical group ID. 
    static member SetGroupID
        ([<Optional; DefaultParameterValue(null)>]?GroupID:string) =
            (fun (job: LongCommand) ->
                GroupID  |> DynObj.setValueOpt job "gid"
                job
            )
    static member tryGetGroupID (job: LongCommand) =
        job.TryGetValue "gid"

    static member removeGroupID (job: LongCommand) =
        job.Remove "gid"
    static member SetDelayBoot
        ([<Optional; DefaultParameterValue(null)>]?DelayBoot:int) =
            (fun (job: LongCommand) ->
                DelayBoot  |> DynObj.setValueOpt job "delay-boot"
                job
            )

    /// An arbitrary string enclosed in double quotes if using spaces or some special characters. 
    static member SetExtra
        ([<Optional; DefaultParameterValue(null)>]?Extra:string) =
            (fun (job: LongCommand) ->
                Extra  |> DynObj.setValueOpt job "extra"
                job
            )
    static member tryGetExtra (job: LongCommand) =
        job.TryGetValue "extra"
    static member removeExtra (job: LongCommand) =
        job.Remove "extra"
    static member tryGetDelayBoot (job: LongCommand) =
        job.TryGetValue "delay-boot"

    static member removeDelayBoot (job: LongCommand) =
        job.Remove "delay-boot"
    /// Advise Slurm that ensuing job steps will require ncpus processors per allocated GPU. Not compatible with the --cpus-per-task option. 
    static member SetCPUsPerGPU
        ([<Optional; DefaultParameterValue(null)>]?CPUsPerGPU:int) =
            (fun (job: LongCommand) ->
                CPUsPerGPU  |> DynObj.setValueOpt job "cpus-per-gpu"
                job
            )
    static member tryGetCPUsPerGPU (job: LongCommand) =
        job.TryGetValue "cpus-per-gpu"

    static member removeCPUsPerGPU (job: LongCommand) =
        job.Remove "cpus-per-gpu"
    /// Restrict node selection to nodes with at least the specified number of cores per socket.
    static member SetCoresPerSocket
        ([<Optional; DefaultParameterValue(null)>]?CoresPerSocket:int) =
            (fun (job: LongCommand) ->
                CoresPerSocket  |> DynObj.setValueOpt job "cores-per-socket"
                job
            )
    static member tryGetCoresPerSocket (job: LongCommand) =
        job.TryGetValue "cores-per-socket"

    static member removeCoresPerSocker (job: LongCommand) =
        job.Remove "cores-per-socket"
    
    /// Unique name for OCI container.
    static member SetContainerID
        ([<Optional; DefaultParameterValue(null)>]?ContainerID:string) =
            (fun (job: LongCommand) ->
                ContainerID  |> DynObj.setValueOpt job "container-id"
                job
            )
    static member tryGetContainerID (job: LongCommand) =
        job.TryGetValue "container-id"

    static member removeContainerID (job: LongCommand) =
        job.Remove "container-id"
    static member SetContainer
        ([<Optional; DefaultParameterValue(null)>]?Container:string) =
            (fun (job: LongCommand) ->
                Container  |> DynObj.setValueOpt job "container"
                job
            )
    static member tryGetContainer (job: LongCommand) =
        job.TryGetValue "container"

    static member removeContainer (job: LongCommand) =
        job.Remove "container"
    /// An arbitrary comment enclosed in double quotes if using spaces or some special characters. 
    static member SetComment
        ([<Optional; DefaultParameterValue(null)>]?Comment:string) =
            (fun (job: LongCommand) ->
                Comment  |> DynObj.setValueOpt job "comment"
                job
            )
    static member tryGetComment (job: LongCommand) =
        job.TryGetValue "comment"

    static member removeComment (job: LongCommand) =
        job.Remove "comment"
    /// Path of file containing burst buffer specification.
    static member SetBurstBufferSpecificationFilePath
        ([<Optional; DefaultParameterValue(null)>]?BurstBufferSpecificationFilePath:string) =
            (fun (job: LongCommand) ->
                BurstBufferSpecificationFilePath  |> DynObj.setValueOpt job "bbf"
                job
            )
    static member tryGetBurstBufferSpecificationFilePath (job: LongCommand) =
        job.TryGetValue "bbf"

    static member removeBurstBufferSpecificationFilePath (job: LongCommand) =
        job.Remove "bbf"
    /// When the --bb option is used, Slurm parses this option and creates a temporary burst buffer script file that is used internally by the burst buffer plugins.
    static member SetBurstBufferSpecification
        ([<Optional; DefaultParameterValue(null)>]?BurstBufferSpecification:SetBurstBufferSpecification) =
            (fun (job: LongCommand) ->
                BurstBufferSpecification  |> DynObj.setValueOpt job "bb"
                job
            )
    static member tryGetBurstBufferSpecification (job: LongCommand) =
        job.TryGetValue "bb"

    static member removeBurstBufferSpecification (job: LongCommand) =
        job.Remove "bb"


    /// Nodes can have features assigned to them by the Slurm administrator. Users can specify which of these features are required by their batch script using this options.
    static member SetBatch
        ([<Optional; DefaultParameterValue(null)>]?Batch:Batch) =
            (fun (job: LongCommand) ->
                Batch  |> DynObj.setValueOpt job "batch"
                job
            )
    static member tryGetBatch (job: LongCommand) =
        job.TryGetValue "batch"

    static member removeBatch (job: LongCommand) =
        job.Remove "batch"


    static member SetTime
        ([<Optional; DefaultParameterValue(null)>]?Time:Time) =
            (fun (job: LongCommand) ->
                Time |> DynObj.setValueOpt job "time"
                job
            )
    static member tryGetTime (job: LongCommand) =
        job.TryGetValue "time"

    static member removeTime (job: LongCommand) =
        job.Remove "time"


    /// sbatch does not launch tasks, it requests an allocation of resources and submits a batch script.
    static member SetNTasks
        ([<Optional; DefaultParameterValue(null)>]?NTasks:int) =
            (fun (job: LongCommand) ->
                NTasks |> DynObj.setValueOpt job "ntasks"
                job
            )
    static member tryGetNTasks (job: LongCommand) =
        job.TryGetValue "ntasks"

    static member removeNTasks (job: LongCommand) =
        job.Remove "ntasks"

    /// Advise the Slurm controller that ensuing job steps will require ncpus number of processors per task.
    static member SetCPUsPerTask
        ([<Optional; DefaultParameterValue(null)>]?CPUsPerTask:int) =
            (fun (job: LongCommand) ->
                if CPUsPerTask = (Some 0) then 
                    Some 1 |> DynObj.setValueOpt job "cpus-per-task"
                else
                    CPUsPerTask |> DynObj.setValueOpt job "cpus-per-task"
                job
            )
    static member tryGetCPUsPerTask (job: LongCommand) =
        job.TryGetValue "cpus-per-task"
        
    static member removeCPUsPerTask (job: LongCommand) =
        job.Remove "cpus-per-task"


    /// Specify the real memory required per node (with unit, e.g. "30gb").
    static member SetMemory
        ([<Optional; DefaultParameterValue(null)>]?Memory:Memory) =
            (fun (job: LongCommand) ->
                Memory |> DynObj.setValueOpt job "mem"
                job
            )
    static member tryGetMemory (job: LongCommand) =
        job.TryGetValue "mem"

    static member removeMemory (job: LongCommand) =
        job.Remove "mem"



type EnvironmentSLURM() =
    let mutable environment : (string * string) list = []

    member this.AddCommandAndArgument (command: string) (argument: string) =
        environment <- (command, argument) :: environment

    member this.GetEnvironment() =
        environment
type Job (jobName: string,processList:(string*string list)list)= 
    inherit DynamicObj()
    member val Name = jobName with get, set
    
    member val Processes = processList with get,set 

    member val OneDash = ShortCommand(jobName) with get,set  
    member val TwoDashes = LongCommand(jobName) with get,set
    member val OnlyKey = OnlyKey(jobName) with get,set


    static member SetJobID
        ([<Optional; DefaultParameterValue(null)>]?JobID: string) =
            (fun (job: Job) ->

                JobID |> DynObj.setValueOpt job "jobid"
                job
            )
    static member tryGetJobID (job: Job) =
        job.TryGetValue "jobid"

    static member removeJobID (job: Job) =
        job.Remove "jobid"

    static member SetEnvironment 
        ([<Optional; DefaultParameterValue(null)>]?environment: EnvironmentSLURM) =
            (fun (job: Job) ->

                environment |> DynObj.setValueOpt job "env"
                job
            )
    static member tryGetEnvironment  (job: Job) =
        job.TryGetValue "env"

    static member removeEnvironment  (job: Job) =
        job.Remove "env"

    member this.getEnvironment = 
        let environmentOption = this |> Job.tryGetEnvironment |> Option.map (fun value -> (value :?> EnvironmentSLURM).GetEnvironment())
        environmentOption |> Option.toList |> List.head

    member this.formatEnv = 
        let localStr = new System.Text.StringBuilder()
        this.getEnvironment 
        |> Seq.toArray 
        |> Array.map (fun x -> localStr.AppendLine (sprintf "%s %s" (fst x) (snd x))) |> ignore 
        localStr.ToString()

    member this.formatProcess   = 
        let localStr = new System.Text.StringBuilder()
        for (processX, args) in this.Processes do
            localStr.AppendFormat("{0} {1}\n", processX, (String.concat " " args)) |> ignore
        localStr.ToString()

    member this.formatOneDash = 
        let localStr = new System.Text.StringBuilder()
        this.OneDash.GetProperties(true)
        |> Seq.toArray
        |> Array.filter (fun x -> x.Key <> "Name")
        |> Array.map (fun x -> localStr.Append (sprintf "-%s %s " x.Key (x.Value.ToString()))) |> ignore
        localStr.ToString()

    member this.formatTwoDashes = 
        let localStr = new System.Text.StringBuilder()
    
        this.TwoDashes.GetProperties(true)
        |> Seq.toArray
        |> Array.filter (fun x -> x.Key <> "Name")
        |> Array.map (fun x -> localStr.Append (sprintf "--%s=%s " x.Key (x.Value.ToString()))) |> ignore
        localStr.ToString()

    member this.formatOnlyKey = 
        let localStr = new System.Text.StringBuilder()
        this.OnlyKey.GetProperties(true)
        |> Seq.toArray
        |> Array.filter (fun x -> x.Key <> "Name")
        |> Array.map (fun x -> localStr.Append (sprintf "--%s " x.Key)) |> ignore
        localStr.ToString()


    member this.formatSlurmCalls = 
        let localStr = new System.Text.StringBuilder()
        localStr.Append ("#!/bin/bash \n") |> ignore
        localStr.Append ("#SBATCH ") |> ignore
        localStr.AppendFormat (sprintf "-J %s " this.Name) |> ignore
        localStr.Append (this.formatOneDash)       |> ignore
        localStr.Append (this.formatTwoDashes)     |> ignore
        localStr.Append (this.formatOnlyKey)       |> ignore
        localStr.Append ("\n")           |> ignore
        if this |> Job.tryGetEnvironment |> Option.isSome then
            localStr.Append (this.formatEnv)            |> ignore
        localStr.Append this.formatProcess    |> ignore
        localStr.ToString()

    member this.produceCall = 
        let strLocal = new System.Text.StringBuilder()
        //strLocal.AppendLine ("#!/bin/bash") |> ignore
        strLocal.AppendLine ("sbatch <<EOF") |> ignore
        strLocal.Append this.formatSlurmCalls |> ignore 
        strLocal.AppendLine("EOF")           |> ignore 
        strLocal.ToString()

     member private this.matchOutput x = 
        match x with 
        | Some value -> value 
        | None -> failwith "No output"
    member private this.callToTerminalCMD (command:string) = 
        let processResponse = 
            cli {
                Shell CMD
                Command command
            }
            |> Command.execute
        // processResponse.Text
        processResponse
    member private this.callToTerminalBash (command:string) = 
        let processResponse = 
            cli {
                Shell BASH
                Command command
            }
            |> Command.execute
        // processResponse.Text
        processResponse
    member this.getResultFromCallCMD (command:string) = 
        (this.callToTerminalCMD (command)).Text
        |> this.matchOutput

    member this.getResultFromCallBash (command:string) = 
        (this.callToTerminalBash (command)).Text
        |> this.matchOutput

    member this.sendToTerminalAndReceiveJobIDBash (job:Job)= 
        job.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
        let res = this.getResultFromCallBash (job.formatProcess)
        // submit 
        // get return 
        job |> Job.SetJobID res |> ignore 
        // set as Job ID 


    member this.sendToTerminalAndReceiveJobIDCMD (job:Job)= 
        // job 
        // set parsable 
        job.OnlyKey |> OnlyKey.SetParsable true |> ignore

    
        let res = this.getResultFromCallCMD (job.formatProcess)
        // submit 
        // get return 
        job |> Job.SetJobID res |> ignore 
        // set as Job ID 