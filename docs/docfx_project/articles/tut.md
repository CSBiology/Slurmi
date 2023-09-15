# Writing a jobscript

Jobscripts consist of three components: sbatch commands, optional environment settings, and the actual process. 

Firstly, let's create a job. In this tool, jobs require a name and a process. The process is composed of a command (such as the tool name), and its respective arguments. 

```fsharp
let myJob =
    Job("MyJob",[("MyProgram",["MyArgument1";"MyArgument2"]);("MyProgram2",["MyArgument1";"MyArgument2"])])
```

When displaying the job, the following information is included: 

```fsharp
myJob |> DynObj.format

//?Name: MyJob
//?Processes: [(MyProgram, [MyArgument1; MyArgument2]); (MyProgram2, [MyArgument1; MyArgument2])]
```
The following step is to add information to the job. This information will later be converted into Slurm commands.

```fsharp 
myJob |> Job.SetNode "MyNode"
myJob |> Job.SetOutput "MyOutput"
myJob |> Job.SetTime ((1,2,3,4))
myJob |> Job.SetNTasks 5
myJob |> Job.SetCPUsPerTask 5
myJob |> Job.SetMemory "30 gb"
myJob |> Job.SetPartition "MyPartition"
myJob |> Job.SetError "MyError"
```
Plenty of Slurm commands are available. For instance, the user's input at `Job.SetTime((1,2,3,4))` reflects the command `#SBATCH --time=01-02:03:04` , which displays a duration of 1 day, 2 hours, 3 minutes, and 4 seconds. More specific formats are also encoded and applied automatically.

Once these values have been set, the job will be displayed accordingly.

```fsharp
myJob |> DynObj.format

// ?Name: MyJob
// ?Processes: [(MyProgram, [MyArgument1; MyArgument2]); (MyProgram2, [MyArgument1; MyArgument2])]
// ?node: MyNode
// ?output: MyOutput
// ?time: (1, 2, 3, 4)
// ?nTasks: 5
// ?cpusPerTask: 5
// ?memory: 30 gb
// ?partition: MyPartition
// ?error: MyError
```

Entries can be removed via `Job.RemoveX`, where x is the name of the value. If you want to display a specific value alone, use the `Job.TryGetX` members.
If an environment is necessary, such as loading a module, it can be included in the task in the following manner:

```fsharp
let newenv = new EnvironmentSLURM()
newenv.AddCommandAndArgument "module load" "proteomiqon"
newenv.AddCommandAndArgument "module load" "testtool"
myJob |> Job.SetEnvironment newenv 
```

The environment is initialized first, followed by the addition of commands and arguments, and setting it to the job. 
To display the set environment, the following command is used: 

```fsharp
let getEnv  = [(myJob |> Job.tryGetEnvironment)] |> List.choose id |> List.map (fun value -> ((value:?>EnvironmentSLURM).GetEnvironment()))

// val it: (string * string) list list = [[("module load", "testtool"); ("module load", "proteomiqon")]]
```

To generate the jobscript, the following command is used: 

```fsharp
let myJobscript = myJob |> Job.createJobscript

// #!/bin/bash
// #SBATCH -J MyJob
// #SBATCH -N MyNode
// #SBATCH -o MyOutput.out
// #SBATCH -e MyError.err
// #SBATCH --time=01-02:03:04
// #SBATCH --ntasks=5
// #SBATCH --cpus-per-task=5
// #SBATCH --mem=30 gb
// #SBATCH -p MyPartition
// module load testtool
// module load proteomiqon
// MyProgram MyArgument1 MyArgument2
// MyProgram2 MyArgument1 MyArgument2
```

