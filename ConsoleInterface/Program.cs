using System;
using Connector;
using Connector.ConnectAPI;
using Terminal.Gui;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpConnector = new TcpConnector();
            
            Console.WriteLine("Found a running instance. State: " + tcpConnector.DiscoverRunningInstances().State);
            Console.WriteLine("Connecting..");
            Console.WriteLine("Connection result: " + tcpConnector.TryConnectToTcpServer());

            if (!tcpConnector.CanUpdateFlightState)
            {
                Console.WriteLine("Cannot receive state updates because sim is not in the flight state.");
            }
            else
            {
                //tcpConnector.StartPlaneStateRefresher();
            }

            CurrentFlightState.BindEvents();
            //CurrentFlightState.FlightStateUpdated += OnFlightStateUpdated;

            Console.ReadLine();
        }

        static void OnFlightStateUpdated(object sender, EventArgs eventArgs)
        {
            var fs = CurrentFlightState.FlightState;
            
            Console.Clear();
            
            Console.WriteLine($"AGL: {fs.AltitudeAgl}");
            Console.WriteLine($"MSL: {fs.AltitudeMsl}");
            Console.WriteLine($"IAS: {fs.IndicatedAirspeedKts}");
            Console.WriteLine($"Mach: {fs.MachNumber}");
            Console.WriteLine($"VS: {fs.VerticalSpeed}");
            Console.WriteLine($"GroundSpd: {fs.GroundSpeedKts}");
            Console.WriteLine($"Pitch: {fs.Pitch}");
            Console.WriteLine($"Bank: {fs.Bank}");
            Console.WriteLine($"Hdg: {fs.HeadingTrue}");
            Console.WriteLine($"Course: {fs.CourseTrue}");
            Console.WriteLine($"GearState: {fs.GearState}");
            Console.WriteLine($"AutopilotOn: {fs.IsAutopilotOn}");
            Console.WriteLine($"OnRunway: {fs.IsOnRunway}");
            Console.WriteLine($"TAS: {fs.TrueAirspeed}");
        }
    }
}