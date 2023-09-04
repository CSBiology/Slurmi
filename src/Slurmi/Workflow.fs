namespace Slurmi 
open DynamicObj
open System.Runtime.InteropServices
open Fli 

open Job



module Workflow = 


    type Workflow (jobs:Job array)= 

        inherit DynamicObj()

        member val Jobs = jobs with get, set
        /// Wall clock time limit.
        static member SetTime
            ([<Optional; DefaultParameterValue(null)>]?Time:(int*int*int*int)) =
                (fun (wf: Workflow) ->
                    Time |> DynObj.setValueOpt wf "time"
                    wf
                )

        static member tryGetTime (wf: Workflow) =
            wf.TryGetValue "time"

        static member SetPartition
            ([<Optional; DefaultParameterValue(null)>]?Partition:string) =
                (fun (wf: Workflow) ->
                    Partition |> DynObj.setValueOpt wf "partition"
                    wf
                )

        static member tryGetPartition (wf: Workflow) =
            wf.TryGetValue "partition"