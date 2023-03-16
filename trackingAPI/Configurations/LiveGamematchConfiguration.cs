namespace trackingAPI.Configurations
{
    public class LiveGamematchConfiguration
    {
        public static int GamematchLengthInSeconds { get; set; } = 5400;
        public static int HalfTimeBreakLengthInSeconds { get; set; } = 1800;
        public static int OvertimeLengthInSeconds { get; set; } = 1800;
        public static int PenaltyShootoutTimeIntervalInSeconds { get; set; } = 60;

    }
}
