namespace Slurmi
open DynamicObj
open System.Runtime.InteropServices
open Fli


type TypeOfDep =
   | All
   | Any

   static member toString (dep:TypeOfDep)= 
       match dep with 
       | All -> ","  
       | Any ->  "?" 
type KindOfDependency =
    /// After the specified jobs start or are cancelled and 'time' in minutes from job start or cancellation happens, this job can begin execution. If no 'time' is given then there is no delay after start or cancellation.
    | After of int option
    /// This job can begin execution after the specified jobs have terminated. This is the default dependency type.
    | Afterany 
    /// This job can begin execution after the specified jobs have terminated and any associated burst buffer stage out operations have completed.
    | Afterburstbuffer 
    /// A task of this job array can begin execution after the corresponding task ID in the specified job has completed successfully (ran to completion with an exit code of zero).
    | Aftercorr 
    /// This job can begin execution after the specified jobs have terminated in some failed state (non-zero exit code, node failure, timed out, etc).
    | Afternotok 
    /// This job can begin execution after the specified jobs have successfully executed (ran to completion with an exit code of zero)
    | Afterok 
    /// This job can begin execution after any previously launched jobs sharing the same job name and user have terminated. In other words, only one job by that name and owned by that user can be running or suspended at any point in time. In a federation, a singleton dependency must be fulfilled on all clusters unless DependencyParameters=disable_remote_singleton is used in slurm.conf.
    | Singleton 
    with 
    static member intOpt (a:int option)= 
        match ( a) with
        |Some value -> a.ToString()
        |None -> ""
    static member toString (dep:KindOfDependency)=
        match dep with
        | Afterany -> "afterany:"
        | Afterburstbuffer -> "afterburstbuffer:" 
        | Aftercorr ->  "aftercorr:" 
        | Afternotok  -> "afternotok:" 
        | Afterok -> "afterok:" 
        | Singleton -> "singleton:" 
        | After value-> "after:" 

        
