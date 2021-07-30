using System;
using Gtk;
using IFACARS.Windows;

namespace IFACARS
{
    public static class WindowManager
    {
        public static HubWindow HubWindow;
        public static FlightWindow FlightWindow;

        public static HubWindow CreateHubWindow()
        {
            HubWindow = new HubWindow();
            
            return HubWindow;
        }

        public static void ShowHubWindow()
        {
            if (HubWindow == null) HubWindow = CreateHubWindow();
            if (HubWindow.IsVisible) return;
            
            HubWindow.Show();
        }
        
        public static FlightWindow CreateFlightWindow()
        {
            FlightWindow = new FlightWindow();
            return FlightWindow;
        }
        
        public static void ShowFlightWindow()
        {
            if (FlightWindow == null) FlightWindow = CreateFlightWindow();
            if (FlightWindow.IsVisible) return;

            FlightWindow.Show();
        }

        public static void CloseHubWindow()
        {
            HubWindow.Destroy();
            HubWindow = null;
        }
        
        public static void CloseFlightWindow()
        {
            FlightWindow.Destroy();
            FlightWindow = null;
        }

        public static void HubWindowDeleteEvent(DeleteEventArgs deleteEventArgs)
        {
            if (FlightWindow != null && FlightWindow.FlightActive)
            {
                if (ConfirmFlightCancellationDialog(HubWindow)) Application.Quit();
                else deleteEventArgs.RetVal = true; // cancel closing the window
                
                return;
            }
            
            Application.Quit();
        }

        public static void FlightWindowDeleteEvent(DeleteEventArgs deleteEventArgs)
        {
            if (FlightWindow.FlightActive && !ConfirmFlightCancellationDialog(FlightWindow))
                deleteEventArgs.RetVal = true; // cancel closing the window
            else CloseFlightWindow();
        }

        public static bool ConfirmFlightCancellationDialog(Window window)
        {
            var dialog = new MessageDialog(window, DialogFlags.DestroyWithParent, MessageType.Question,
                ButtonsType.YesNo, "If you exit now, your flight will not be saved. Are you sure?");

            var value = (ResponseType)dialog.Run();
            dialog.Destroy();

            return value == ResponseType.Yes;
        }
    }
}