using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    [Serializable]
    public class ObjectSpawnInfo
    {
        /// <summary>
        /// The prefab of the object to be spawned.
        /// </summary>
        public GameObject objectPrefab;

        /// <summary>
        /// The chance of spawning the object (0.0 to 1.0).
        /// </summary>
        [Range(0.0f, 1.0f)] public float spawnChance = 0.8f;

        /// <summary>
        /// The minimum scale for the spawned object.
        /// </summary>
        public float minimumScale = 0.6f;

        /// <summary>
        /// The maximum scale for the spawned object.
        /// </summary>
        public float maximumScale = 1.4f;

        /// <summary>
        /// The random x z offset applied to the spawn position.
        /// </summary>
        [Range(0.0f, 0.3f)] public float randomOffset = 0.1f;
    }

    /// <summary>
    /// A class for managing randomized object spawning on tiles.
    /// </summary>
    public class RandomizedTile : MonoBehaviour
    {
        /// <summary>
        /// Base tile.
        /// </summary>
        [SerializeField] private GameObject baseTile;

        /// <summary>
        /// List of spawn points for objects.
        /// </summary>
        [SerializeField] private List<GameObject> objectSpawnPoints;

        /// <summary>
        /// List of object spawn information.
        /// </summary>
        [SerializeField] private List<ObjectSpawnInfo> objectSpawnInfoList;

        // List to store the spawned objects on this tile.
        [SerializeField] private List<GameObject> spawnedObjects = new List<GameObject>();

        private List<GameObject> preserveList = new List<GameObject>();

        private void Start()
        {
            GenerateObjects();
        }

        /// <summary>
        /// Generates objects on the tile based on spawn information and chances.
        /// </summary>
        public void GenerateObjects()
        {
            // Clear existing spawned objects
            ClearSpawnedObjects();

            // Check if there are valid spawn points and spawn info.
            if (objectSpawnPoints == null || objectSpawnPoints.Count == 0 ||
                objectSpawnInfoList == null || objectSpawnInfoList.Count == 0)
                return;

            foreach (GameObject spawnPoint in objectSpawnPoints)
            {
                bool spawnPointUsed = false; // Flag to track if spawn point was used

                foreach (ObjectSpawnInfo spawnInfo in objectSpawnInfoList)
                {
                    if (Random.Range(0.0f, 1.0f) < spawnInfo.spawnChance && !spawnPointUsed)
                    {
                        Vector3 spawnPosition = spawnPoint.transform.position;
                        GameObject objectPrefab = spawnInfo.objectPrefab;

                        // Apply random offset, rotation, and scale to the spawned object.
                        objectPrefab.transform.position = spawnPosition + new Vector3(spawnInfo.randomOffset, 0, spawnInfo.randomOffset);
                        objectPrefab.transform.Rotate(Vector3.up, Random.Range(0, 360));
                        objectPrefab.transform.localScale = Vector3.one * Random.Range(spawnInfo.minimumScale, spawnInfo.maximumScale);

                        // Instantiate the object as a child of this tile and add it to the list of spawned objects.
                        GameObject generatedObject = Instantiate(objectPrefab, gameObject.transform, true);
                        spawnedObjects.Add(generatedObject);

                        spawnPointUsed = true;
                    }
                }

                // Hide the spawn point
                spawnPoint.SetActive(false);
            }
        }

        /// <summary>
        /// Clears all spawned objects on the tile and resets the spawn points.
        /// </summary>
        public void ClearSpawnedObjects()
        {
            preserveList.AddRange(objectSpawnPoints);
            preserveList.Add(baseTile);

            foreach (GameObject spawnPoint in objectSpawnPoints)
            {
                spawnPoint.SetActive(true);
            }

            // Get all child GameObjects of the parent object
            Transform parentTransform = transform;
            List<GameObject> childrenToRemove = new List<GameObject>();

            foreach (Transform child in parentTransform)
            {
                // Check if the child is not in the preserve list
                if (!preserveList.Contains(child.gameObject))
                {
                    childrenToRemove.Add(child.gameObject);
                }
            }

            foreach (GameObject childToRemove in childrenToRemove)
            {
                DestroyImmediate(childToRemove);
            }

            preserveList.Clear();
            spawnedObjects.Clear();
        }
    }
}