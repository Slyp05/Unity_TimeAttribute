using UnityEngine;

/*

    TimeAttribute created by Lucas Sarkadi.

    Creative Commons Zero v1.0 Universal licence, 
    meaning it's free to use in any project with no need to ask permission or credits the author.

    Check out the github page for more informations:
    https://github.com/Slyp05/Unity_TimeAttribute

*/
public class TimeAttribute : PropertyAttribute
{
    // custom types
    public enum Unit
    {
        Seconds = 0,
        Minutes = 1,
        Hours = 2,
    }

    // properties
    public Unit unit { get; private set; } = Unit.Seconds;

    public bool defaultToFrame { get; private set; } = false;

    // constructors

    /// <summary>
    /// Use this attribute to make a float field representing time editable by frames or with another unit than seconds.
    /// </summary>
    /// <param name="unit">Unit to use only for display in the inspector. The float will still represent seconds in your script.</param>
    /// <param name="defaultToFrame">Should we default to choosing seconds by frames or directly.</param>
    public TimeAttribute(Unit unit, bool defaultToFrame)
    {
        this.unit = unit;
        this.defaultToFrame = defaultToFrame;
    }

    /// <summary>
    /// Use this attribute to make a float field representing time editable by frames or with another unit than seconds.
    /// </summary>
    /// <param name="unit">Unit to use only for display in the inspector. The float will still represent seconds in your script.</param>
    public TimeAttribute(Unit unit)
    {
        this.unit = unit;
    }

    /// <summary>
    /// Use this attribute to make a float field representing time editable by frames or with another unit than seconds.
    /// </summary>
    /// <param name="defaultToFrame">Should we default to choosing seconds by frames or directly.</param>
    public TimeAttribute(bool defaultToFrame)
    {
        this.defaultToFrame = defaultToFrame;
    }

    /// <summary>
    /// Use this attribute to make a float field representing time editable by frames or with another unit than seconds.
    /// </summary>
    public TimeAttribute()
    {

    }
}