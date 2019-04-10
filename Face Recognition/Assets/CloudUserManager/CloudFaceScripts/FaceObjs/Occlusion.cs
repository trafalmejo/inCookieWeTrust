using System;

/// <summary>
/// Represents length of moustache, beard and sideburn
/// </summary>
[Serializable]
public class Occlusion
{
	
    /// <summary>
    /// Gets or sets the moustache value. Represents the length of moustache.
    /// </summary>
    /// <value>
    /// The moustache value.
    /// </value>
	public bool foreheadOccluded;

    /// <summary>
    /// Gets or sets the beard value. Represents the length of beard.
    /// </summary>
    /// <value>
    /// The beard value.e
    /// </value>
	public bool eyeOccluded;

    /// <summary>
    /// Gets or sets the sideburns value. Represents the length of sideburns.
    /// </summary>
    /// <value>
    /// The sideburns value.
    /// </value>
	public bool mouthOccluded;

}

