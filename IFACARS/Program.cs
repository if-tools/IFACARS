using System;
using Gtk;

namespace IFACARS
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.IF-Tools.IFACARS", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            app.AddWindow(WindowManager.CreateHubWindow());

            WindowManager.ShowHubWindow();
            Application.Run();
        }
    }
}