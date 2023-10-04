namespace Slurmi
open DynamicObj
open System.Runtime.InteropServices
open Fli

////let checkStringIntOpt (a:string*int option)= 
////    match (snd a) with
////    |Some value -> sprintf "%s+%i" (fst a) value
////    |None -> sprintf "%s" (fst a)
//type TypeOfDep =
//    | All
//    | Any

//    static member toString (dep:TypeOfDep)= 
//        match dep with 
//        | All -> ","  
//        | Any ->  "?" 

//type DependencyType =
//    /// After the specified jobs start or are cancelled and 'time' in minutes from job start or cancellation happens, this job can begin execution. If no 'time' is given then there is no delay after start or cancellation.
//    | After of (string*int option) list
//    /// This job can begin execution after the specified jobs have terminated. This is the default dependency type.
//    | Afterany of string list
//    /// This job can begin execution after the specified jobs have terminated and any associated burst buffer stage out operations have completed.
//    | Afterburstbuffer of string list
//    /// A task of this job array can begin execution after the corresponding task ID in the specified job has completed successfully (ran to completion with an exit code of zero).
//    | Aftercorr of string list
//    /// This job can begin execution after the specified jobs have terminated in some failed state (non-zero exit code, node failure, timed out, etc).
//    | Afternotok of string list
//    /// This job can begin execution after the specified jobs have successfully executed (ran to completion with an exit code of zero)
//    | Afterok of string list
//    /// This job can begin execution after any previously launched jobs sharing the same job name and user have terminated. In other words, only one job by that name and owned by that user can be running or suspended at any point in time. In a federation, a singleton dependency must be fulfilled on all clusters unless DependencyParameters=disable_remote_singleton is used in slurm.conf.
//    | Singleton of string list

//    static member toString (dep:DependencyType) =
//        match dep with
//        | Afterany (dep) -> sprintf "afterany:%s" (dep |> String.concat ":")
//        | Afterburstbuffer (dep) -> sprintf "afterburstbuffer:%s" (dep |> String.concat ":")
//        | Aftercorr (dep) -> sprintf "aftercorr:%s" (dep |> String.concat ":")
//        | Afternotok (dep) -> sprintf "afternotok:%s" (dep |> String.concat ":")
//        | Afterok (dep) -> sprintf "afterok:%s" (dep |> String.concat ":")
//        | Singleton (dep) -> sprintf "singleton"
//        | After (dep) ->
//            let depTimes = 
//                dep 
//                |> List.map (
//                    fun x ->  
//                        match (snd x) with
//                        |Some value -> sprintf "%s+%i" (fst x) value
//                        |None -> sprintf "%s" (fst x))
                
            
//                |> String.concat (":")
//            sprintf "after:%s" depTimes


//    static member concat (typeOfDep: TypeOfDep)(depL: DependencyType[]) =
//        let sep = 
//            TypeOfDep.toString typeOfDep 
//        depL
//        |> Array.map (fun x -> DependencyType.toString x)
//        |> String.concat sep









////let depTypeToString (dep:DependencyType) =
////    match dep with
////    | Afterany (dep) -> sprintf "afterany:%s" (dep |> String.concat ":")
////    | Afterburstbuffer (dep) -> sprintf "afterburstbuffer:%s" (dep |> String.concat ":")
////    | Aftercorr (dep) -> sprintf "aftercorr:%s" (dep |> String.concat ":")
////    | Afternotok (dep) -> sprintf "afternotok:%s" (dep |> String.concat ":")
////    | Afterok (dep) -> sprintf "afterok:%s" (dep |> String.concat ":")
////    | Singleton (dep) -> sprintf "singleton"
////    | After (dep) ->
////        let depTimes = 
////            dep 
////            |> List.map (
////                fun x ->  
////                    checkStringIntOpt x)
                
            
////            |> String.concat (":")
////        sprintf "after:%s" depTimes


////let internal concatDependencies  (seperator:string) (depL: DependencyType[])=
////    depL
////    |> Array.map (fun x -> depTypeToString x)
////    |> String.concat seperator



        

