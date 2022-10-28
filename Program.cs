using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

static class App {

    private static bool StartPHP(string phpPath, UInt16 port) {
        try {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = phpPath;
            psi.Arguments = " -b 127.0.0.1:" + port;
            psi.CreateNoWindow = true;
            psi.LoadUserProfile = false;
            psi.RedirectStandardError = false;
            psi.RedirectStandardOutput = false;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            //psi.WorkingDirectory = ".";
            Process process = new Process();
            process.StartInfo = psi;
            process.Start();
            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    [STAThread]
    static int Main(string[] args) {
        if (args.Length < 1) {
            Console.WriteLine("Starts instance(s) of php in background.");
            Console.WriteLine("Usage:");
            Console.WriteLine("    HiddenPHP {Start Port[;End Port[;Increment]]");
            Console.WriteLine("Example:");
            Console.WriteLine("    HiddenPHP 9000          # starts one instance of php on port 9000");
            Console.WriteLine("    HiddenPHP 9000;9500;100 # starts 5 instances of php on ports 9000,9100,9200,9300,9400");
            return 0;
        }

        UInt16 start_port = 0;
        UInt16 end_port = 0;
        UInt16 increment = 1;

        string[] parts = args[0].Split(';');
        if (parts.Length < 1 || parts.Length > 3) {
            Console.WriteLine("Error: invalid usage");
            return -1;
        }

        try {
            start_port = Convert.ToUInt16(parts[0]);
        } catch {
            Console.WriteLine(args[0] + " is not a valid port number");
            return -1;
        }

        if (parts.Length > 1) {
            try {
                end_port = Convert.ToUInt16(parts[1]);
            } catch {
                Console.WriteLine(args[0] + " is not a valid port number");
                return -1;
            }
        } else {
            end_port = (ushort) (start_port + 1);
        }
        if (parts.Length > 2) {
            try {
                increment = Convert.ToUInt16(parts[2]);
                if (increment < 1)
                    throw new Exception();
            } catch {
                Console.WriteLine("is not a valid increment number");
                return -1;
            }
        }
        if ((end_port - start_port) / increment > 32) {
            Console.WriteLine("This utility can spawn maximum 32");
            return -1;

        }
        string phpPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\php-cgi.exe";
        if (!File.Exists(phpPath)) {
            Console.WriteLine("php-cgi not found at '" + phpPath + "', this executable should be in same php folder that you want to run");
            return 1;
        }
        Console.WriteLine("Executing PHP at " + phpPath);

        for (UInt16 port = start_port;port < end_port;port += increment) {
            Console.WriteLine("starting php on port " + port);
            StartPHP(phpPath, port);
        }
        return 0;
    }
}
