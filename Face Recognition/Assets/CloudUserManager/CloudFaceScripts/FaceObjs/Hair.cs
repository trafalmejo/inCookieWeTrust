using System;

/// <summary>
/// Represents length of moustache, beard and sideburn
/// </summary>
[Serializable]
public class Hair
{
	
    /// <summary>
    /// Gets or sets the moustache value. Represents the length of moustache.
    /// </summary>
    /// <value>
    /// The moustache value.
    /// </value>
	public float bald;

    /// <summary>
    /// Gets or sets the invisible value. Represents the invisible.
    /// </summary>
    /// <value>
    /// The invisible value.
    /// </value>
	public bool invisible;

	public HairColor[] hairColor;

}

