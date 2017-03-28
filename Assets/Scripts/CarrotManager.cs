using UnityEngine;

[System.Serializable]
public struct CarrotManagerConfig
{
    public float minSpawnX;
    public float maxSpawnX;
    public float spawnY;
    public float spawnRate;
    public GameObject carrotPrefab;
}

public class CarrotManager : MonoBehaviour
{
    public CarrotManagerConfig config;

    float nextSpawnTime = 0f;

    void Awake()
    {
        nextSpawnTime = Time.time + config.spawnRate;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime) {
            nextSpawnTime += config.spawnRate;

            float x = Random.Range(config.minSpawnX, config.maxSpawnX);
            Vector3 position = new Vector3(x, config.spawnY, 0f);

            Instantiate(config.carrotPrefab, position, Quaternion.identity);
        }
    }
}