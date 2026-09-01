using UnityEngine;

/// <summary>
/// Camera scrolls continuously forward along X. Y position is locked to keep vertical viewport bounds stable.
/// </summary>
public class SideScrollCamera : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 5f;

    [Header("Fixed Position Settings")]
    [SerializeField] private float fixedYPosition = 0f; // Fixed vertical height
    [SerializeField] private float zDistance = -15f;

    private void LateUpdate()
    {
        // Advance camera strictly along X axis
        float currentX = transform.position.x + (scrollSpeed * Time.deltaTime);

        // Maintain fixed Y and Z coordinates
        transform.position = new Vector3(currentX, fixedYPosition, zDistance);
    }
}