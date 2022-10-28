# HiddenPHP
This is a tiny utility that spawn instances of PHP from command line without console window

Normally when working with nginx and php on windows machines, you will need to run php separately to listen to a port and then configure nginx to connect to that port to send php scripts to php processor. This will require running php in listen mode from console that in turn creates a console window on your taskbar which you cannot close occupying your taskbar space. 
You could move these console windows (you will need more than one php instance to work with ngninx in n-tier php applications) to a different work space but hiding the console window is more elegant way of doing it.

## How to use it
```console
HiddenPHP {Start Port[;End Port[;Increment]]
```
### Example:
```console
HiddenPHP 9000          # starts one instance of php on port 9000
HiddenPHP 4000;4002     # starts 3 instances of php on ports 4000,4001,4002
HiddenPHP 9000;9500;100 # starts 5 instances of php on ports 9000,9100,9200,9300,9400
```

You can put your command line in a batch file and run that, console window of batch file will be closed after completion

### Windows 10 issue
In windows 10, running a console application that will create an extra `conhost.exe` process which will stay in memory until the original console application is closed. this process normally has no use other than occupying memory. At this moment, killing it automatically is not possible, since I cannot determine which `conhost.exe` process belongs to processes that I have spawned. You can open task manager and kill them if they bother you (they do bother me and each of them occupies 5Mb of my precious memory, so I kill them manually)

