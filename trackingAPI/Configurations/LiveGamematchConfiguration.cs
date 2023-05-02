namespace trackingAPI.Configurations
{
    public class LiveGamematchConfiguration
    {
        //public static int GamematchLengthInSeconds { get; set; } = 5400;
        //public static int HalfTimeBreakLengthInMilliSeconds { get; set; } = 180000;
        //public static int OvertimeLengthInSeconds { get; set; } = 1800;
        //public static int PenaltyShootoutTimeIntervalMilliInSeconds { get; set; } = 60;


        /////////////////FOR TESTING////////////////////
        public static int GamematchLengthInSeconds { get; set; } = 45*60;
        public static int HalfTimeBreakLengthInMilliSeconds { get; set; } = 2000;
        public static int OvertimeLengthInSeconds { get; set; } = 10;
        public static int PenaltyShootoutTimeIntervalMilliInSeconds { get; set; } = 2000;

    }
}
