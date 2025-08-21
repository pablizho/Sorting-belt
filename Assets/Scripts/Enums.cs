using UnityEngine;

/// <summary>
/// Defines the possible types of trash items in the game.
/// </summary>
public enum ItemType
{
    Paper,
    Plastic,
    Organic
}

/// <summary>
/// Defines the three possible directions a player can flick an item.
/// Left and Right map to the outer bins, Up maps to the middle bin.
/// </summary>
public enum ThrowDirection
{
    Left,
    Up,
    Right
}
