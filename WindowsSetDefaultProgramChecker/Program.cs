using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsSetDefaultProgramChecker.ProcessList;
using Newtonsoft.Json;

namespace WindowsSetDefaultProgramChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayVersionInformation();
            ExectuteShellCommand("control /name Microsoft.DefaultPrograms /page pageDefaultProgram");
            EvaluateBatchExecutedSuccessfully("Standardprogramme festlegen");
            CloseApp();
        }

        private static void EvaluateBatchExecutedSuccessfully(string expectedWindowName)
        {
            var windowExists = CheckWaitExists(expectedWindowName);
            if (windowExists)
            {
                Console.WriteLine("Die Batch kann genutzt werden");
                CloseWindowOnFocus();
            }
            else Console.WriteLine("Die Batch kann NICHT genutzt werden");
        }

        private static void CloseWindowOnFocus()
        {
            SendKeys.SendWait("%({F4})");
        }

        private static bool CheckWaitExists(string expectedWindowName)
        {
            return new ProcessList.ProcessList().HasWindow(expectedWindowName);
        }

        private static void DisplayVersionInformation()
        {
            var version = File.ReadAllText("LastBuildInfo.txt");
            Console.WriteLine(version + "\n");
        }

        private static void ExectuteShellCommand(string batchCommand)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = string.Format("/C " + batchCommand)
                };
            process.StartInfo = startInfo;
            process.Start();
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
            Console.Write(" : ");

            var count = 0;
            while (count < 3)
            {
                Console.Write($"{3- count++}...");
                Task.Delay(1000).Wait();
            }
            Environment.Exit(0);
        }

        private static bool InputIsInvalid(string result)
        {
            return result != null && !(result.ToLower().Equals("ja") || result.ToLower().Equals("nein"));
        }
    }
}
