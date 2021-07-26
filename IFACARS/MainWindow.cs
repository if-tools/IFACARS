using System;
using System.Xml.Serialization;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace IFACARS
{
    class MainWindow : Window
    {
        [UI] private MenuItem _aboutMenuItem = null;
        [UI] private MenuItem _quitMenuItem = null;
        
        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            
            _aboutMenuItem.Activated += AboutMenuItem_Activated;
            _quitMenuItem.Activated += QuitMenuItem_Activated;
            
            DeleteEvent += Window_DeleteEvent;
        }

        private void AboutDialogOnDestroyEvent(object o, DestroyEventArgs args)
        {
            
        }

        private void AboutMenuItem_Activated(object? sender, EventArgs e)
        {
            var aboutDialog = new IFACARS.Dialogs.AboutDialog();
            aboutDialog.Show();
        }
        
        private void QuitMenuItem_Activated(object? sender, EventArgs e)
        {
            Application.Quit();
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}