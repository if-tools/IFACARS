using Gtk;
using IFACARS.Types.Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace IFACARS.Screens
{
    public class FlightCreationScreen : StackScreen
    {
        private const string _xmlName = "flight_createNew";
        
        /* Glade objects */
        [UI] private Stack createScreenStack = null;

        [UI] private Button create_startAnew = null;
        [UI] private Button create_fromBriefing = null;
        
        public FlightCreationScreen(Stack parentStack) : base(parentStack, _xmlName)
        {
            
        }

        public override void Init()
        {
            SwitchToInitialScreen();
            
            SetupEvents();
        }

        private void SetupEvents()
        {
            create_startAnew.Clicked += (sender, args) => SwitchToFlightInfoScreen();
            create_fromBriefing.Clicked += (sender, args) => SwitchToFlightInfoScreen();
        }

        private void SwitchToInitialScreen()
        {
            createScreenStack.VisibleChildName = "create_initial";
        }
        
        private void SwitchToFlightInfoScreen()
        {
            createScreenStack.VisibleChildName = "create_flightInfo";
        }
    }
}