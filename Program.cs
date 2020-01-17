using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;

namespace tripletwenty
{
    internal class Program
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
        
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        
        public static void Main(string[] args)
        {
            Console.WriteLine(@"Running Windows Service ist not possible on the command line. Use InstallUtil.exe from dotnet framework to install the service!");

            //#if DEBUG
            //Thread.Sleep(10000);
            //#endif
            Stopwatch s = new Stopwatch();
            s.Start();
            
            var desktopPtr = GetDC(IntPtr.Zero);
            var font = new Font(FontFamily.GenericSerif, 100);

            
            while (s.Elapsed < TimeSpan.FromSeconds(2))
            {
                var graphics = Graphics.FromHdc(desktopPtr);
                
                graphics.FillRectangle(Brushes.Aqua, new Rectangle(1000, 0, 4096, 2160));

                graphics.DrawString("Test", font, Brushes.White, new PointF());
            }
            ReleaseDC(IntPtr.Zero, desktopPtr);
            
            SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);

            s.Stop();
            

            //System.ServiceProcess.ServiceBase.Run(new TripleTwentyService());
        }
    }
}