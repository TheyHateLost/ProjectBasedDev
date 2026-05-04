using System;

[Flags]
public enum ApplianceOrientation
{
    None = 0,
    Right = 1 << 0,
    Left = 1 << 1,
    Front = 1 << 2,
    Back = 1 << 3,
    Any = Right | Left | Front | Back
}
