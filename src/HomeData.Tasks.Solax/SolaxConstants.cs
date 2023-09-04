namespace HomeData.Tasks.Solax;

public static class SolaxConstants
{
    public static class Attributes
    {
        public const string Version = "Version";
        public const string SerialNumber = "SerialNumber";

        public const string Grid1Voltage = "Grid1Voltage";
        public const string Grid2Voltage = "Grid2Voltage";
        public const string Grid3Voltage = "Grid3Voltage";

        public const string Grid1Current = "Grid1Current";
        public const string Grid2Current = "Grid2Current";
        public const string Grid3Current = "Grid3Current";

        public const string Grid1Power = "Grid1Power";
        public const string Grid2Power = "Grid2Power";
        public const string Grid3Power = "Grid3Power";

        public const string InverterPower = "InverterPower";
        public const string CurrentHousePower = "CurrentHousePower";

        public const string Grid1Frequency = "Grid1Frequency";
        public const string Grid2Frequency = "Grid2Frequency";
        public const string Grid3Frequency = "Grid3Frequency";

        public const string PowerPv1 = "PowerPv1";
        public const string PowerPv2 = "PowerPv2";

        public const string FeedInPower = "FeedInPower";
        public const string FeedInEnergy = "FeedInEnergy";
        public const string ConsumedEnergy = "ConsumedEnergy";

        public const string YieldTotal = "YieldTotal";
        public const string YieldToday = "YieldToday";

        public const string RadiatorTemperature = "RadiatorTemperature";

        public const string BatteryCapacity = "BatteryCapacity";
        public const string BatteryTemperature = "BatteryTemperature";
        public const string BatteryPower = "BatteryPower";

        public const string TodayGridInEnergy = "TodayGridInEnergy";
        public const string TodayGridOutEnergy = "TodayGridOutEnergy";

        public const string BatteryOperationMode = "BatteryOperationMode";
        public const string InverterOperationMode = "InverterOperationMode";
    }

    public static class BatteryOperationModes
    {
        public const string SelfUseMode = "Self use mode";
        public const string ForceTimeUse = "Force time use";
        public const string BackUpMode = "Back up mode";
        public const string FeedInPriority = "Feed in priority";
        public const string Unknown = "Unknown";
    }

    public static class InverterOperationModes
    {
        public const string Waiting = "Waiting";
        public const string Unknown = "Unknown";
        public const string Checking = "Checking";
        public const string Normal = "Normal";
        public const string Off = "Off";
        public const string PermanentFault = "Permanent Fault";
        public const string Updating = "Updating";
        public const string EpsCheck = "EPS check";
        public const string EpsMode = "EPS mode";
        public const string SelfTest = "Self test";
        public const string Idle = " Idle";
        public const string Standby = "Standby";
    }
}