using System.Dynamic;
using Shared.Utility;

namespace Shared.Types.ConnectAPI
{
    public class FlightState
    {
        public float AltitudeAgl { get; set; }
        public float AltitudeMsl { get; set; }
        public float IndicatedAirspeed { get; set; }
        public float IndicatedAirspeedKts => Numbers.MpsToKts(IndicatedAirspeed);
        public float MachNumber { get; set; }
        public float VerticalSpeed { get; set; }
        public float VerticalSpeedFpm => Numbers.MpsToFpm(VerticalSpeed);
        public float GroundSpeed { get; set; }
        public float GroundSpeedKts => Numbers.MpsToKts(GroundSpeed);
        public float Pitch { get; set; }
        public float Bank { get; set; }
        public float HeadingTrue { get; set; }
        public float CourseTrue { get; set; }
        public GearState GearState { get; set; }
        public bool IsAutopilotOn { get; set; }
        public bool IsOnGround => AltitudeAgl == 0;
        public bool IsOnRunway { get; set; }
        public Coordinate Location { get; set; }
        public float TrueAirspeed { get; set; }
        public float TrueAirspeedKts => Numbers.MpsToKts(TrueAirspeed);

    }
}