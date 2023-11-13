namespace Slurmi 
open DynamicObj
open System.Runtime.InteropServices
open Fli

module Connection = 

    let matchOutput (output) = 
        match output with 
        | Some value -> value 
        | None -> failwith "No output"

    let callToTerminalCMD (command:string) = 
        let processResponse = 
            cli {
                Shell CMD
                Command command
            }
            |> Command.execute
        // processResponse.Text
        processResponse
    let callToTerminalBash (command:string) = 
        let processResponse = 
            cli {
                Shell BASH
                Command command
            }
            |> Command.execute
        // processResponse.Text
        processResponse

    let getResultFromCallCMD (command:string) = 
        (callToTerminalCMD (command)).Text
        |> matchOutput

    let getResultFromCallBash (command:string) = 
        (callToTerminalBash (command)).Text
        |> matchOutput

    let sendToTerminalAndReceiveJobIDBash (job:Job)= 
        //job.OnlyKey |> OnlyKey.SetParsable true |> ignore
    
        let res = getResultFromCallBash (job.formatProcess)
        // submit 
        // get return 
        // set as Job ID 
        job |> Job.SetJobID res |> ignore 
        

    let sendToTerminalAndReceiveJobIDCMD (job:Job)= 
        // job 
        // set parsable 
        //job.OnlyKey |> OnlyKey.SetParsable true |> ignore

        let res = getResultFromCallCMD (job.formatProcess)
        // submit 
        // get return 
        // set as Job ID 
        job |> Job.SetJobID res |> ignore 
        