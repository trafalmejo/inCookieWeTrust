using System;

/// <summary>
/// Represents length of moustache, beard and sideburn
/// </summary>
[Serializable]
public class HairColor
{
	
    /// <summary>
    /// Gets or sets the moustache value. Represents the length of moustache.
    /// </summary>
    /// <value>
    /// The moustache value.
    /// </value>
	public string color;

    /// <summary>
    /// Gets or sets the beard value. Represents the length of beard.
    /// </summary>
    /// <value>
    /// The beard value.
    /// </value>
	public float confidence;

}

