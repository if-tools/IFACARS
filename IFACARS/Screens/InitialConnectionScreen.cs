using Gtk;
using IFACARS.Types.Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace IFACARS.Screens
{
    public class InitialConnectionScreen : StackScreen
    {
        private const string _xmlName = "flight_initialConnection";
        
        /* Glade objects */
        [UI] private Stack initialConnectionScreenStack = null;

        [UI] private Button connection_continueSearchingButton = null;
        [UI] private Button connection_connectButton = null;
        
        public InitialConnectionScreen(Stack parentStack) : base(parentStack, _xmlName)
        {
            
        }

        public override void Init()
        {
            SwitchToDiscoverInstancesScreen();
            
            SetupEvents();
        }

        private void SetupEvents()
        {
            //create_startAnew.Clicked += (sender, args) => SwitchToFlightInfoScreen();
            //create_fromBriefing.Clicked += (sender, args) => SwitchToFlightInfoScreen();
        }

        private void SwitchToDiscoverInstancesScreen()
        {
            initialConnectionScreenStack.VisibleChildName = "connection_discoverInstances";
        }
        
        private void SwitchToInstanceInfoScreen()
        {
            initialConnectionScreenStack.VisibleChildName = "connection_instanceInformation";
        }
        
        private void SwitchToConnectScreen()
        {
            initialConnectionScreenStack.VisibleChildName = "connection_connect";
        }
    }
}