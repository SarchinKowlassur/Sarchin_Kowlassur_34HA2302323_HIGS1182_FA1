using UnityEngine;

/// <summary>
/// Smooth, jitter-free infinite parallax looping using absolute position wrapping.
/// Attach this script to the PARENT object of each layer.
/// </summary>
public class InfiniteParallaxBackground : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("0 = fixed to camera (moves with it), 1 = fixed in world space")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxFactorX = 0.5f;

    [Tooltip("Vertical parallax factor. Set to 0 to keep vertically locked with camera.")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxFactorY = 0f;

    [Header("Tile Width")]
    [Tooltip("Set this to the exact width of ONE child tile in world units.")]
    [SerializeField] private float tileWidth = 30f;

    private Transform cameraTransform;
    private Vector3 initialCameraPos;
    private Vector3 initialBackgroundPos;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            initialCameraPos = cameraTransform.position;
        }

        initialBackgroundPos = transform.position;

        // Auto-detect tile width if left at 0
        if (tileWidth <= 0f)
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                tileWidth = sr.bounds.size.x;
            }
            else
            {
                MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
                if (mr != null) tileWidth = mr.bounds.size.x;
            }
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null || tileWidth <= 0f) return;

        // Total distance camera has traveled from initial start position
        Vector3 cameraDelta = cameraTransform.position - initialCameraPos;

        // Calculate current parallax offset
        float rawParallaxX = cameraDelta.x * (1f - parallaxFactorX);
        float rawParallaxY = cameraDelta.y * (1f - parallaxFactorY);

        // Modulo math for smooth, seamless wrapping without frame-rate jitter
        float wrappedX = Mathf.Repeat(rawParallaxX, tileWidth);

        // Update layer position relative to camera
        transform.position = new Vector3(
            cameraTransform.position.x - wrappedX,
            initialBackgroundPos.y + rawParallaxY,
            initialBackgroundPos.z
        );
    }
}