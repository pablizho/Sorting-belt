using UnityEngine;

/// <summary>
/// Handles swipe input from the player. When a valid swipe is detected, the script determines
/// the swipe direction and instructs the GameManager to evaluate the throw for the current item
/// present in the interaction window.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    [Tooltip("Reference to the interaction window used to detect which item is being sorted.")]
    public InteractionWindow interactionWindow;
    
    [Tooltip("Reference to the game manager that controls scoring and spawning.")]
    public GameManager gameManager;

    [Tooltip("Minimum swipe distance (in pixels) required to register a throw.")]
    public float minSwipeDistance = 50f;

    private Vector2 swipeStart;
    private bool swiping;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Start tracking a potential swipe on mouse down (or finger touch)
        if (Input.GetMouseButtonDown(0))
        {
            swiping = true;
            swipeStart = Input.mousePosition;
        }

        // Complete the swipe when the button is released
        if (Input.GetMouseButtonUp(0) && swiping)
        {
            Vector2 swipeEnd = Input.mousePosition;
            Vector2 delta = swipeEnd - swipeStart;

            if (delta.magnitude >= minSwipeDistance)
            {
                ThrowDirection direction = DetermineDirection(delta);
                TrashItem current = interactionWindow != null ? interactionWindow.GetCurrentItem() : null;
                
                if (current != null && gameManager != null)
                {
                    // Remove the item from the window immediately to avoid duplicate throws
                    interactionWindow.RemoveItem(current);
                    gameManager.EvaluateThrow(current, direction);
                }
                else if (gameManager != null)
                {
                    // No item to sort: consider this a miss with a small penalty
                    gameManager.RegisterMissWithoutItem();
                }
            }

            swiping = false;
        }
    }

    /// <summary>
    /// Determines the direction of a swipe based on its delta vector.
    /// If the horizontal movement is greater than the vertical, the swipe is left or right.
    /// Otherwise it is considered an upward swipe.
    /// </summary>
    private ThrowDirection DetermineDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return delta.x > 0 ? ThrowDirection.Right : ThrowDirection.Left;
        }
        else
        {
            return ThrowDirection.Up;
        }
    }
}