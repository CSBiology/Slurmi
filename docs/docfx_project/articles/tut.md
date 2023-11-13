# Writing a jobscript

Jobscripts consist of three components: sbatch commands, optional environment settings, and the actual process. 

Firstly, let's create a job. In this tool, jobs require a name and a process. The process is composed of a command (such as the tool name), and its respective arguments. 

```fsharp
let myJob =
    Job("MyJob",[("MyProgram",["MyArgument1";"MyArgument2"]);("MyProgram2",["MyArgument1";"MyArgument2"])])
```

When displaying the job, the following information is included: 

```fsharp
myJob 

//   Slurmi.Job
//     {CommandWithArgument = Slurmi.Command;
//      Name = "MyJob";
//      OnlyKey = Slurmi.OnlyKey;
//      Processes = [("MyProgram", ["MyArgument1"; "MyArgument2"]);
//                   ("MyProgram2", ["MyArgument1"; "MyArgument2"])];
//      formatEnv = ?;
//      formatOnlyKey = "";
//      formatProcess = "MyProgram MyArgument1 MyArgument2
//                       MyProgram2 MyArgument1 MyArgument2
// ";
//      formatSlurmCalls = "#!/bin/bash 
//                          #SBATCH -J MyJob --parsable 
//                          MyProgram MyArgument1 MyArgument2
//                          MyProgram2 MyArgument1 MyArgument2
//                          ";
//      formatCommands = "";
//      getEnvironment = ?;
//      produceCall = "sbatch <<EOF
//                    #!/bin/bash 
//                    #SBATCH -J MyJob --parsable 
//                    MyProgram MyArgument1 MyArgument2
//                    MyProgram2 MyArgument1 MyArgument2
//                    EOF
//                    ";}

```
The fields `OnlyKey` and `CommandWithArgument` are empty and will be filled with information regarding the job submission. 

The following step is to add information to the job. This information will later be converted into Slurm commands.

```fsharp 
myJob
myJob.CommandWithArgument   |> Command.SetPartition "test-Partition"
myJob.CommandWithArgument   |> Command.SetNode "MyNode"
myJob.CommandWithArgument   |> Command.SetOutput "MyOutput"
myJob.CommandWithArgument   |> Command.SetNTasks 5
myJob.CommandWithArgument   |> Command.SetMemory {memory = 30; unit = MemoryUnit.G }
myJob.CommandWithArgument   |> Command.SetTime{Days = None; clock = Some {hour = 2; minute = 30; second = Some 15}}
myJob.OnlyKey               |> OnlyKey.SetWait true 

```
Plenty of Slurm commands are available. For instance, the user's input at `Command.SetTime{Days = None; clock = Some {hour = 2; minute = 30; second = Some 15}}` reflects the command `#SBATCH --time=02:30:15` , which displays a duration of 0 days, 2 hours, 30 minutes, and 15 seconds. More specific formats are also encoded and applied automatically.

Once these values have been set, the job will be displayed accordingly.

```fsharp
myJob 

//   Slurmi.Job
//     {CommandWithArgument = Slurmi.Command;
//      Name = "MyJob";
//      OnlyKey = Slurmi.OnlyKey;
//      Processes = [("MyProgram", ["MyArgument1"; "MyArgument2"]);
//                   ("MyProgram2", ["MyArgument1"; "MyArgument2"])];
//      formatCommands = "--partition=test-Partition --nodes=MyNode --output=MyOutput --ntasks=5 --mem=30G --time=02:30:15 ";
//      formatEnv = ?;
//      formatOnlyKey = "--wait ";
//      formatProcess = "MyProgram MyArgument1 MyArgument2
//                      MyProgram2 MyArgument1 MyArgument2
//                      ";
//      formatSlurmCalls = "#!/bin/bash 
//                         #SBATCH -J MyJob --parsable --partition=test-Partition --nodes=MyNode --output=MyOutput --ntasks=5 --mem=30G --time=02:30:15 --wait 
//                         MyProgram MyArgument1 MyArgument2
//                         MyProgram2 MyArgument1 MyArgument2
//                         ";
//      getEnvironment = ?;
//      produceCall = "sbatch <<EOF
//                     #!/bin/bash 
//                     #SBATCH -J MyJob --parsable --partition=test-Partition --nodes=MyNode --output=MyOutput --ntasks=5 --mem=30G --time=02:30:15 --wait 
//                     MyProgram MyArgument1 MyArgument2
//                     MyProgram2 MyArgument1 MyArgument2
//                     EOF
//                     ";}

```

Entries can be removed via `Command.RemoveX` or `OnlyKey.removeX`, where X is the name of the value. If you want to display a specific value alone, use the `Job.TryGetX` members.
If an environment is necessary, such as loading a module, it can be included in the task in the following manner:

```fsharp
let env = EnvironmentSLURM()
env.AddCommandAndArgument "module load" "dotnet" 
myJob |> Job.SetEnvironment env

// access the environment
myJob.getEnvironment

// (string * string) list = [("module load", "dotnet")]
```

The environment is initialized first, followed by the addition of commands and arguments, and setting it to the job. 
To display the formatted environment, the following command is used: 

```fsharp
myJob.formatEnv

// "module load dotnet"
```



To generate the job call, the following command is used: 

```fsharp
myJob.produceCall

// "sbatch <<EOF
// #!/bin/bash 
// #SBATCH -J MyJob --parsable --partition=test-Partition --nodes=MyNode --output=MyOutput --ntasks=5 --mem=30G --time=02:30:15 --wait 
// module load dotnet
// MyProgram MyArgument1 MyArgument2
// MyProgram2 MyArgument1 MyArgument2
// EOF
// "
```

This job can be submitted using the `Connection.callToTerminalBash` function. 
The job ID is returned as a string. 

```fsharp
Connection.callToTerminalBash myJob.produceCall
```


