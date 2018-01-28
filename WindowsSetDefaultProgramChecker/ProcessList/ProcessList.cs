using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsSetDefaultProgramChecker.ProcessList
{
    /// <summary>
    /// modified based on Form1.cs from http://blog.bigbasti.com/c-alle-sichtbaren-prozesse-fenster-auflisten/
    /// </summary>
    public class ProcessList
    {
        private List<string> _windowTitles;

        /// <summary>
        /// adappeted from http://blog.bigbasti.com/c-alle-sichtbaren-prozesse-fenster-auflisten/
        /// </summary>
        public List<string> GrabWindowNames(bool visibleWindows = true)
        {
            Windows windows = new Windows();
            _windowTitles = new List<string>();
            //Alle Fenster durchgehen und in die Lite einsetzen
            foreach (Window w in windows.lstWindows)
            {
               if (visibleWindows && !w.winVisible)
               {
                    continue; //Wenn Fenster unsichtbar ist übrspringen
               }
               _windowTitles.Add(w.winTitle);
            }
            return _windowTitles;
        }

        public bool HasWindow(string windowName, int delayMilliseconds = 30000)
        {
            if (_windowTitles == null)
            {
                GrabWindowNames();
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (!DoHasWindow(windowName) && timer.ElapsedMilliseconds < delayMilliseconds)
            {
                GrabWindowNames();
                Task.Delay(250).Wait();
            }
            return DoHasWindow(windowName);
        }

        private bool DoHasWindow(string windowName)
        {
            return _windowTitles.Any(x => Regex.IsMatch(x, windowName));
        }
    }
}
