using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns asteroid prefabs continuously ahead of the player in a designated side-scrolling spawn window.
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [Header("Prefab & Target References")]
    [SerializeField] private GameObject[] asteroidPrefabs; // Array supports multiple asteroid designs
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnInterval = 1.5f;   // Time between spawns
    [SerializeField] private float initialDelay = 1.0f;

    [Header("Designated Spawn Bounds")]
    [SerializeField] private float spawnXDistance = 25f;   // Distance ahead of camera/player
    [SerializeField] private Vector2 spawnYRange = new Vector2(-6f, 6f); // Top and bottom bounds
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 2.2f); // Size variety

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        StartCoroutine(SpawnRoutine());
    }

    /// <summary>
    /// Coroutine loop that continuously instantiates asteroids while the game is active.
    /// </summary>
    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsGameOver)
            {
                SpawnObstacle();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Custom method to instantiate an asteroid within designated random boundaries.
    /// </summary>
    public void SpawnObstacle()
    {
        if (asteroidPrefabs == null || asteroidPrefabs.Length == 0) return;

        // Select random prefab from array
        GameObject selectedPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Calculate spawn position ahead of the camera view
        float spawnX = mainCamera != null ? mainCamera.transform.position.x + spawnXDistance : transform.position.x + spawnXDistance;
        float spawnY = Random.Range(spawnYRange.x, spawnYRange.y);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f); // Grounded on Z = 0 gameplay plane

        // Instantiate and apply random scale
        GameObject newAsteroid = Instantiate(selectedPrefab, spawnPos, Random.rotation);
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        newAsteroid.transform.localScale = Vector3.one * randomScale;

        Debug.Log("Obstacle Spawned: " + newAsteroid.name + " at " + spawnPos);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizes spawn bounds in Editor
        Gizmos.color = Color.red;
        Vector3 center = transform.position + new Vector3(spawnXDistance, (spawnYRange.x + spawnYRange.y) / 2f, 0f);
        Vector3 size = new Vector3(2f, Mathf.Abs(spawnYRange.y - spawnYRange.x), 2f);
        Gizmos.DrawWireCube(center, size);
    }
}