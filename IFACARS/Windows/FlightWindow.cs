using Gtk;
using IFACARS.Screens;
using UI = Gtk.Builder.ObjectAttribute;

namespace IFACARS.Windows
{
    public class FlightWindow : Window
    {
        /* Glade objects */
        [UI] private Stack mainStack = null;
        
        public bool FlightActive = false;

        private FlightCreationScreen _flightCreationScreen;
        
        public FlightWindow() : this(new Builder("FlightWindow.glade"))
        {
            
        }

        private FlightWindow(Builder builder) : base(builder.GetRawOwnedObject("FlightWindow"))
        {
            builder.Autoconnect(this);
            
            SetupEvents();

            _flightCreationScreen = new FlightCreationScreen(mainStack);
            builder.Autoconnect(_flightCreationScreen);
            _flightCreationScreen.Init();
        }
        
        private void SetupEvents()
        {
            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            WindowManager.FlightWindowDeleteEvent(a);
        }
    }
}