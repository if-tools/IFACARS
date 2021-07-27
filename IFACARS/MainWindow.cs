using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace IFACARS
{
    class MainWindow : Window
    {
        [UI] private ModelButton _aboutButton = null;
        
        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            _aboutButton.Clicked += AboutMenuItem_Activated;

            DeleteEvent += Window_DeleteEvent;
        }

        private void AboutMenuItem_Activated(object? sender, EventArgs e)
        {
            var aboutDialog = new IFACARS.Dialogs.AboutDialog();
            aboutDialog.Show();
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}