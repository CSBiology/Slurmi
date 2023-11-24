# SSH connection

When working with a cluster or HPC, it can be useful to connect via SSH. 
To submit the created Jobs directly to the cluster, Slurmi allows you a SSH connection using [SSH.NET](https://github.com/sshnet/SSH.NET). 

First, you need to create a `SshClient` containing your connection info (host, username, password).

```fsharp 
let hostName = "testHost" 
let userName = "testUser"
let password = "testPassword"

let connectionInfo =
    ConnectionInfo(hostName, userName, PasswordAuthenticationMethod(userName,password))

let client = let client = new SshClient(connectionInfo)
```

To start the connection to the host, you can use the `Connect` method.

```fsharp
client.Connect()
```

To disconnect from the host, you can use the `Disconnect` method.

```fsharp
client.Disconnect()
```

## Submitting Jobs

To submit a job, you need to create a [job](../articles/jobCreation.html) object.

For a single job, use the function `Runner.sshToTerminal`. This will give you the jobID after submission as an output. 

```fsharp
let myJob =
    Job("MyJob",[("echo",["Hello World"])])

let jobID = Runner.sshToTerminal client myJob
```

When submitting a [workflow](../articles/workflowCreation.html), use the function `Runner.submitAllSSH`. 
This will resolve the dependencies and submit the jobs in the correct order.

```fsharp
// for the creation, see workflowCreation
let resultGraph = createWorkflow [jd1;jd2;jd3;jd4;jd5]

Runner.submitAllSSH resultGraph.Graph.Graph workedOn client
```

And with that, you're able to interact with your cluster or HPC via SSH using Slurmi and SSH.NET. 

