# Writing a workflow 

Developing a workflow is one of the key parts of using the application. 
Here, you define the sequential steps for completing the work.
Some of these steps are dependent on each other, and these dependencies are defined in the workflow.
To do this, you first create a job list.

```fsharp 
let myJoblist  =
    [|
        Job ("MyJob",[("MyProgram",["MyArgument1";"MyArgument2"])]);
        Job ("MyJob2",[("MyProgram2",["MyArgument12";"MyArgument22"])]);
        Job ("MyJob3",[("MyProgram3",["MyArgument13";"MyArgument23"])]);
    |]
```

This also enables you to set a value to multiple jobs at once, for instance:

```fsharp
myJoblist
|> Array.mapi (fun i x -> x |> Job.SetCPUsPerTask(i))
```

In this case, the following values are set to each job: 

```fsharp 
myJoblist |> Array.map (fun x -> x |> Job.tryGetCPUsPerTask)

// val it: obj option array = [|Some 0; Some 1; Some 2|]
```

The next step is to set up the dependencies. 
Dependencies consist of a job, a list of dependencies, and a dependency type.
If more than one dependency is specified, the value `Any` or `All` is used to specify that any or all of the dependencies must be satisfied.
In the workflow itself, jobs are referenced by a name which is automatically assigned. To find the correct job by name, the `findDependencyName` function is used.

```fsharp
// simple dependency
let dependency = Afternotok ["jobToDependOn"]

// complex dependencies
myJoblist.[1] |> Job.SetDependency((All,[|Afterany [(findDependencyName "MyJob3" myJoblist)];Afterok [(findDependencyName "MyJob" myJoblist);"Example1";"Example2"]|]))
myJoblist.[2] |> Job.SetDependency((Any,[|After [(findDependencyName "MyJob" myJoblist,Some 2);("alsoJustAnExample",None)]; Afternotok [findDependencyName "MyJob" myJoblist]|]))
```


The workflow is initiated with the joblist as an argument. Here you can set the required time and partition.

```fsharp 
let myWorkflow = new Workflow(myJoblist)
myWorkflow |> Workflow.SetTime((2,3,4,1))
myWorkflow |> Workflow.SetPartition("csb")
```

To create the workflow, the `createWorkflowFromWFType` function is used.

```fsharp 
let myWorkflow = createWorkflowFromWFType myWorkflow

// #!/bin/bash
// #SBATCH --time=02-03:04:01
// #SBATCH -p csb
// Job0=$(sbatch --parsable  MyJob)
// Job1=$(sbatch --parsable --dependency=afterany:$Job2,afterok:$Job0:Example1:Example2 MyJob2)
// Job2=$(sbatch --parsable --dependency=after:$Job0+2:alsoJustAnExample?afternotok:$Job0 MyJob3)
```
