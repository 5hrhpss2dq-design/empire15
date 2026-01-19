using UnityEngine;

namespace Empire15.GameLoop
{
    /// <summary>
    /// Manages player spawning at designated spawn points
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject playerPrefab;
        
        private static SpawnManager instance;
        public static SpawnManager Instance => instance;
        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        
        private void Start()
        {
            // Auto-spawn player at start
            SpawnPlayer();
        }
        
        /// <summary>
        /// Spawns a player at a random spawn point
        /// </summary>
        public GameObject SpawnPlayer()
        {
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogWarning("No spawn points defined!");
                return null;
            }
            
            // Select random spawn point
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];
            
            // Spawn player
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            
            Debug.Log($"Player spawned at {spawnPoint.name}");
            return player;
        }
        
        /// <summary>
        /// Respawns the player at a new location
        /// </summary>
        public void RespawnPlayer(GameObject currentPlayer)
        {
            if (currentPlayer != null)
                Destroy(currentPlayer);
            
            SpawnPlayer();
        }
        
        /// <summary>
        /// Adds a spawn point at runtime
        /// </summary>
        public void AddSpawnPoint(Transform point)
        {
            var list = new System.Collections.Generic.List<Transform>(spawnPoints);
            list.Add(point);
            spawnPoints = list.ToArray();
        }
    }
}
