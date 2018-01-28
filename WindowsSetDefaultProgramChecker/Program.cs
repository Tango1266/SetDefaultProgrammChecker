using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsSetDefaultProgramChecker.ProcessList;
using Newtonsoft.Json;

namespace WindowsSetDefaultProgramChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var version = File.ReadAllText("LastBuildInfo.txt");
            Console.WriteLine(version);
            var command = "/C control /name Microsoft.DefaultPrograms /page pageDefaultProgram";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            process.StartInfo = startInfo;
            process.Start();

            var windowExists = new ProcessList.ProcessList().hasWindow("Standardprogramme festlegen");
            if(windowExists) Console.WriteLine("Die Batch kann genutzt werden");
            else Console.WriteLine("Die Batch kann NICHT genutzt werden");
            CloseApp();
        }

        private static void AskUserForEvaluation()
        {
            Console.WriteLine("Hat sich das Fenster \"Standardprogramme festlegen\" geöffnet?: [ja|nein]");
            var result = string.Empty;
            int answers = 0;
            while (InputIsInvalid(result))
            {
                Console.WriteLine("Hat sich das Fenster \"Standardprogramme festlegen\" geöffnet?: [ja|nein]");
                if (answers > 1)
                {
                    Console.Write("Bitte antworte nur mit ja oder nein: ");
                }
                result = Console.ReadLine();
                answers++;
            }
            var ja = result.Equals("ja");
            Console.WriteLine($"\n Das Programm kann {(ja ? "" : "NICHT")} verwendet werden");
        }

        private static void CloseApp()
        {
            Console.WriteLine("\n Programm wird beendet in...");
            Console.Write(" :");

            var count = 0;
            while (count < 3)
            {
                Console.Write($"{3- count++}...");
                Task.Delay(1250).Wait();
            }
            Environment.Exit(0);
        }

        private static bool InputIsInvalid(string result)
        {
            return result != null && !(result.ToLower().Equals("ja") || result.ToLower().Equals("nein"));
        }
    }
}
