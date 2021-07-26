using System;
using Connector.ConnectAPI;
using Shared.Types;
using Shared.Types.ConnectAPI;

namespace Connector
{
    public class CurrentFlightState
    {
        public static event EventHandler FlightStateUpdated = delegate { };

        public static FlightState FlightState;

        public static void BindEvents()
        {
            TcpConnector.PlaneStateReceived += ReceivedStateUpdate;
        }

        private static void ReceivedStateUpdate(object sender, EventArgs eventArgs)
        {
            //Console.WriteLine("received");
            
            var newStateEntries = TcpConnector.PlaneStateEntries;
            
            // TODO: logic before updating the flight state.

            // make equal to the old state in case some values don't have a definition in the new entries. 
            var newFlightState = FlightState ?? new FlightState();

            var coordinate = newFlightState.Location ?? new Coordinate();

            // TODO: come up with a more elegant solution.
            foreach (var stateEntry in newStateEntries)
            {
                var val = stateEntry.Value.ToString();

                switch (stateEntry.Path)
                {
                    case "aircraft/0/altitude_agl":
                        newFlightState.AltitudeAgl = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.AltitudeAgl
                            : float.Parse(val);
                        break;
                    case "aircraft/0/altitude_msl":
                        newFlightState.AltitudeMsl = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.AltitudeMsl
                            : float.Parse(val);
                        break;
                    case "aircraft/0/indicated_airspeed":
                        newFlightState.IndicatedAirspeed = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.IndicatedAirspeed
                            : float.Parse(val);
                        break;
                    case "aircraft/0/mach_speed":
                        newFlightState.MachNumber = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.MachNumber
                            : float.Parse(val);
                        break;
                    case "aircraft/0/vertical_speed":
                        newFlightState.VerticalSpeed = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.VerticalSpeed
                            : float.Parse(val);
                        break;
                    case "aircraft/0/groundspeed":
                        newFlightState.GroundSpeed = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.GroundSpeed
                            : float.Parse(val);
                        break;
                    case "aircraft/0/pitch":
                        newFlightState.Pitch = string.IsNullOrWhiteSpace(val) ? newFlightState.Pitch : float.Parse(val);
                        break;
                    case "aircraft/0/bank":
                        newFlightState.Bank = string.IsNullOrWhiteSpace(val) ? newFlightState.Bank : float.Parse(val);
                        break;
                    case "aircraft/0/heading_true":
                        newFlightState.HeadingTrue = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.HeadingTrue
                            : float.Parse(val);
                        break;
                    case "aircraft/0/course":
                        newFlightState.CourseTrue = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.CourseTrue
                            : float.Parse(val);
                        break;
                    case "aircraft/0/systems/landing_gear/state":
                        newFlightState.GearState = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.GearState
                            : (GearState) int.Parse(val);
                        break;
                    case "aircraft/0/systems/autopilot/on":
                        newFlightState.IsAutopilotOn = stateEntry.Value.ToString() == "True";
                        break;
                    case "aircraft/0/is_on_runway":
                        newFlightState.IsOnRunway = stateEntry.Value.ToString() == "True";
                        break;
                    case "aircraft/0/latitude":
                        coordinate.Latitude = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.Location?.Latitude ?? 0
                            : float.Parse(val);
                        break;
                    case "aircraft/0/longitude":
                        coordinate.Longitude = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.Location?.Longitude ?? 0
                            : float.Parse(val);
                        break;
                    case "aircraft/0/true_airspeed":
                        newFlightState.TrueAirspeed = string.IsNullOrWhiteSpace(val)
                            ? newFlightState.TrueAirspeed
                            : float.Parse(val);
                        break;
                }
            }

            newFlightState.Location = coordinate;
            
            // TODO: logic after updating the flight state, but before assigning the global variable to it.

            FlightState = newFlightState;

            FlightStateUpdated(new object(), EventArgs.Empty);
        }
    }
}