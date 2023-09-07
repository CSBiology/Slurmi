# Introduction Slurmi 

Slurm, which stands for "Simple Linux Utility for Resource Management", is an open-source system for managing clusters and scheduling jobs. It allocates resources such as CPU cores, memory, and GPUs to enable the completion of tasks. 

The submission of jobscripts  (also known as batch scripts) is required to initiate jobs in the cluster. These scripts define the essential resource allocations and provide details of the commands and arguments required to run processes. Jobscripts consist of three components: sbatch commands, optional environment settings, and the actual process.

The sbatch command section adheres to the "#SBATCH" format, which is followed by a command and its associated arguments. This tool simplifies job creation by enabling users to define key information and arguments, automating the formatting process and reducing the chance of typographical errors. Furthermore, the tool's structure facilitates the concurrent creation and modification of several jobs, for example through mapping techniques. 
