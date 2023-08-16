namespace HomeData.Tasks.Solax.Model;

public class SolaxX3G4Data
{
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
}