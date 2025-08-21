using UnityEngine;

/// <summary>
/// Represents a piece of trash that moves along the conveyor belt.
/// Each item has a type (paper, plastic, organic) and a movement speed
/// assigned by the game manager when it is spawned.
/// </summary>
public class TrashItem : MonoBehaviour
{
    [Tooltip("The type of this trash item. Determines which bin it belongs to.")]
    public ItemType itemType;

    // The horizontal speed at which the item travels along the belt.
    private float moveSpeed;

    // Cached reference to the game manager. Assigned on first use to avoid repeated searches.
    private GameManager gameManager;

    // World position beyond which the item will be destroyed if it is not sorted.
    [Tooltip("X position (world coordinates) after which an unsorted item is considered missed and destroyed.")]
    public float destroyX = 10f;

    /// <summary>
    /// Set the movement speed of this trash item.
    /// Called by the GameManager when spawning the item.
    /// </summary>
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void Update()
    {
        // Move the item to the right along the belt.
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);

        // If the item travels beyond the destroy threshold without being sorted, penalise the player.
        if (transform.position.x > destroyX)
        {
            // Lazy initialise the GameManager reference.
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }

            if (gameManager != null)
            {
                gameManager.MissedItem(this);
            }
            else
            {
                // If no manager is found, simply destroy the item.
                Destroy(gameObject);
            }
        }
    }
}