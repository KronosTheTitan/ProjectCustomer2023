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

    public class RandomizedTile : MonoBehaviour
    {
        /// <summary>
        /// List of spawn points for objects.
        /// </summary>
        [SerializeField] private List<GameObject> objectSpawnPoints;

        /// <summary>
        /// List of object spawn information.
        /// </summary>
        [SerializeField] private List<ObjectSpawnInfo> objectSpawnInfoList;

        /// <summary>
        /// Called when the script starts.
        /// </summary>
        void Start()
        {
            if (objectSpawnPoints == null || objectSpawnPoints.Count == 0 || objectSpawnInfoList == null || objectSpawnInfoList.Count == 0)
                return;

            foreach (GameObject spawnPoint in objectSpawnPoints)
            {
                foreach (ObjectSpawnInfo spawnInfo in objectSpawnInfoList)
                {
                    if (Random.Range(0.0f, 1.0f) > spawnInfo.spawnChance)
                        return;

                    Vector3 spawnPosition = spawnPoint.transform.position;
                    GameObject objectPrefab = spawnInfo.objectPrefab;
                    objectPrefab.transform.position = spawnPosition + new Vector3(spawnInfo.randomOffset, 0, spawnInfo.randomOffset);
                    objectPrefab.transform.Rotate(Vector3.up, Random.Range(0, 360));
                    objectPrefab.transform.localScale = Vector3.one * Random.Range(spawnInfo.minimumScale, spawnInfo.maximumScale);
                    GameObject generatedObject = Instantiate(objectPrefab, gameObject.transform, true);
                    break;
                }

                Destroy(spawnPoint);
            }
            objectSpawnPoints.Clear();
        }
    }
}
