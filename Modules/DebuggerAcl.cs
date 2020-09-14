using System.Collections.Generic;
using System.Diagnostics;

namespace Nokae.DebugProtector
{
    internal static class DebuggerAcl
    {
        internal static bool Run()
        {
            var returnvalue = false;

            if (Debugger.IsAttached || Debugger.IsLogging())
            {
                returnvalue = true;
            }
            else
            {
                var strArray = new string[41]
                {
                    "codecracker",
                    "x32dbg",
                    "x64dbg",
                    "ollydbg",
                    "ida",
                    "charles",
                    "dnspy",
                    "simpleassembly",
                    "peek",
                    "httpanalyzer",
                    "httpdebug",
                    "fiddler",
                    "wireshark",
                    "dbx",
                    "mdbg",
                    "gdb",
                    "windbg",
                    "dbgclr",
                    "kdb",
                    "kgdb",
                    "mdb",
                    "processhacker",
                    "scylla_x86",
                    "scylla_x64",
                    "scylla",
                    "idau64",
                    "idau",
                    "idaq",
                    "idaq64",
                    "idaw",
                    "idaw64",
                    "idag",
                    "idag64",
                    "ida64",
                    "ida",
                    "ImportREC",
                    "IMMUNITYDEBUGGER",
                    "MegaDumper",
                    "CodeBrowser",
                    "reshacker",
                    "cheat engine"
                };
                foreach (var process in Process.GetProcesses())
                    if (process != Process.GetCurrentProcess())
                        for (var index = 0; index < strArray.Length; ++index)
                        {
                            if (process.ProcessName.ToLower().Contains(strArray[index])) returnvalue = true;

                            if (process.MainWindowTitle.ToLower().Contains(strArray[index])) returnvalue = true;
                        }
            }

            return returnvalue;
        }

        private static string ReturnProcessLists()
        {
            var processlist = Process.GetProcesses();

            var myCollection = new List<string>();

            foreach (var theprocess in processlist) myCollection.Add(theprocess.ProcessName);
            return string.Join("|", myCollection.ToArray());
        }
    }
}
