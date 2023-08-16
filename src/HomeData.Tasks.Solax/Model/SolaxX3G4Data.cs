namespace HomeData.Tasks.Solax.Model;

public class SolaxX3G4Data
{
    public DateTime Time { get; set; }

    public string Version { get; set; }

    public string SerialNumber { get; set; }

    /// <summary>
    /// In Volts.
    /// </summary>
    public decimal Grid1Voltage { get; set; }

    /// <summary>
    /// In Volts.
    /// </summary>
    public decimal Grid2Voltage { get; set; }

    /// <summary>
    /// In Volts.
    /// </summary>
    public decimal Grid3Voltage { get; set; }

    /// <summary>
    /// In Ampere.
    /// </summary>
    public decimal Grid1Current { get; set; }

    /// <summary>
    /// In Ampere.
    /// </summary>
    public decimal Grid2Current { get; set; }

    /// <summary>
    /// In Ampere.
    /// </summary>
    public decimal Grid3Current { get; set; }


    /// <summary>
    /// In Watts.
    /// </summary>
    public int Grid1Power { get; set; }

    /// <summary>
    /// In Watts.
    /// </summary>
    public int Grid2Power { get; set; }

    /// <summary>
    /// In Watts.
    /// </summary>
    public int Grid3Power { get; set; }

    /// <summary>
    /// In Hz.
    /// </summary>
    public decimal? Grid1Frequency { get; set; }

    /// <summary>
    /// In Hz.
    /// </summary>
    public decimal? Grid2Frequency { get; set; }

    /// <summary>
    /// In Hz.
    /// </summary>
    public decimal? Grid3Frequency { get; set; }

    /// <summary>
    /// in Watts.
    /// </summary>
    public int PowerPv1 { get; set; }

    /// <summary>
    /// in Watts.
    /// </summary>
    public int PowerPv2 { get; set; }

    /// <summary>
    /// in Watts.
    /// </summary>
    public long FeedInPower { get; set; }

    /// <summary>
    /// In kWh.
    /// </summary>
    public decimal FeedInEnergy { get; set; }

    /// <summary>
    /// In kWh
    /// </summary>
    public decimal ConsumedEnergy { get; set; }

    /// <summary>
    /// in celsius degree.
    /// </summary>
    public int? RadiatorTemperature { get; set; }

    /// <summary>
    /// In kWh.
    /// </summary>
    public decimal YieldToday { get; set; }

    /// <summary>
    /// In kWh.
    /// </summary>
    public decimal YieldTotal { get; set; }

    /// <summary>
    /// In %.
    /// </summary>
    public int? BatteryCapacity { get; set; }

    /// <summary>
    /// In degree of celsius.
    /// </summary>
    public int? BatteryTemperature { get; set; }

    /// <summary>
    /// in Watts.
    /// </summary>
    public int? BatteryPower { get; set; }

    /// <summary>
    /// in Watts.
    /// </summary>
    public int? InverterPower { get; set; }

    /// <summary>
    /// In Watts
    /// </summary>
    public int? CurrentHousePower { get; set; }

}