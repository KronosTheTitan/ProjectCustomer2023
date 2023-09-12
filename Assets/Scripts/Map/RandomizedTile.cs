using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class RandomizedTile : MonoBehaviour
    {
        /// <summary>
        /// List of positions (as GameObjects) where trees should be able to spawn.
        /// </summary>
        [SerializeField] private List<GameObject> objectSpawnPoints;

        /// <summary>
        /// Prefab to spawn on GameObjects.
        /// </summary>
        [SerializeField] private GameObject objectPrefab;

        [SerializeField] private float minimumScale = 0.6f;
        [SerializeField] private float maximumScale = 1.4f;

        [SerializeField][Range(0.0f, 0.3f)] private float randomOffset = 0.1f;

        [SerializeField][Range(0.1f, 1.0f)] private float objectSpawnChance = 0.8f;

        void Start()
        {
            if (objectSpawnPoints == null || objectSpawnPoints.Count == 0)
                return;
            
            foreach (GameObject spawnPoint in objectSpawnPoints)
            {
                if (Random.Range(0.0f, 1.0f) < objectSpawnChance)
                {
                    Vector3 spawnPosition = spawnPoint.transform.position;
                    objectPrefab.transform.position = spawnPosition + new Vector3(randomOffset, 0, randomOffset);
                    objectPrefab.transform.Rotate(Vector3.up, Random.Range(0, 360));
                    objectPrefab.transform.localScale = Vector3.one * Random.Range(minimumScale, maximumScale);
                    GameObject generatedObject = Instantiate(objectPrefab, gameObject.transform, true);
                }

                Destroy(spawnPoint);
            }
            objectSpawnPoints.Clear();
        }
    }
}
