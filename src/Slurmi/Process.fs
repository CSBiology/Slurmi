namespace Slurmi
open Fli

module Process = 

    let createProcess (programCall:string*string list):ShellContext= 
        let fullComm = (fst programCall)::(snd programCall)
        let commands = fullComm |> String.concat " "
        {
            config =         
                {   Shell = CMD
                    Command = Some commands
                    Input = None
                    Output = None
                    WorkingDirectory = None
                    EnvironmentVariables = None
                    Encoding = None
                    CancelAfter = None }    
        }

    let createProcessCall (programCall:string*string list) = 
        let processContext = createProcess programCall |> Command.toString 
        let commandFull = processContext.[11..]
        commandFull

