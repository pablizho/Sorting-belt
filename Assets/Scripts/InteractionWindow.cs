using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the area on screen where the player can interact with trash items.
/// Tracks which items are currently inside the interaction window so that the
/// PlayerInput can determine which item to flick.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class InteractionWindow : MonoBehaviour
{
    // A list of trash items currently inside this window. The first item is considered the active item.
    private readonly List<TrashItem> itemsInWindow = new List<TrashItem>();

    /// <summary>
    /// Called when a collider enters the trigger zone. If it is a trash item, add it to the list.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        TrashItem item = other.GetComponent<TrashItem>();
        if (item != null && !itemsInWindow.Contains(item))
        {
            itemsInWindow.Add(item);
        }
    }

    /// <summary>
    /// Called when a collider exits the trigger zone. Remove the item from the list if present.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        TrashItem item = other.GetComponent<TrashItem>();
        if (item != null)
        {
            itemsInWindow.Remove(item);
        }
    }

    /// <summary>
    /// Returns the first trash item in the window, or null if none.
    /// </summary>
    public TrashItem GetCurrentItem()
    {
        if (itemsInWindow.Count > 0)
        {
            // Remove any null references (objects destroyed without OnTriggerExit invocation)
            itemsInWindow.RemoveAll(item => item == null);
            return itemsInWindow.Count > 0 ? itemsInWindow[0] : null;
        }
        return null;
    }

    /// <summary>
    /// Removes a specific item from the window's list. This should be called when the item is successfully sorted.
    /// </summary>
    public void RemoveItem(TrashItem item)
    {
        itemsInWindow.Remove(item);
    }
}