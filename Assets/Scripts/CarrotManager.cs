using UnityEngine;

[System.Serializable]
public struct CarrotManagerConfig
{
    public int spawnPointCount;
    public float spawnPointStartX;
    public float spawnPointSpacing;
    public float spawnY;
    public float spawnRate;
    public GameObject carrotPrefab;
}

public class CarrotManager : MonoBehaviour
{
    public CarrotManagerConfig config;

    private Transform[] spawnPoints;
    private bool[] occupiedPoints;

    float nextSpawnTime = 0f;

    void Awake()
    {
        spawnPoints = new Transform[config.spawnPointCount];
        for (int i = 0; i < spawnPoints.Length; ++i) {
            GameObject go = new GameObject("carrot_spawn_point");
            go.transform.SetParent(transform, false);
            float x = config.spawnPointStartX + i * config.spawnPointSpacing;
            go.transform.position = new Vector3(x, config.spawnY, 0f);
            spawnPoints[i] = go.transform;
        }
        occupiedPoints = new bool[spawnPoints.Length];

        nextSpawnTime = Time.time + config.spawnRate;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime) {
            if (spawnCarrot()) {
                nextSpawnTime = Time.time + config.spawnRate;
            }
        }
    }

    private bool spawnCarrot()
    {
        int nextIndex = getNextSpawnPointIndex();
        if (nextIndex < 0) {
            return false;
        }

        occupiedPoints[nextIndex] = true;
        Transform parent = spawnPoints[nextIndex];
        GameObject carrotObj = Instantiate(config.carrotPrefab) as GameObject;
        carrotObj.transform.SetParent(parent, false);
        PickupController pickup = carrotObj.GetComponent<PickupController>();
        pickup.onPickup += () => { occupiedPoints[nextIndex] = false; };

        return true;
    }

    private int getNextSpawnPointIndex()
    {
        int start = GameManagerLocator.GameManager.Random.NextInt(occupiedPoints.Length);
        for (int i = 0; i < occupiedPoints.Length; ++i) {
            int index = (i + start) % occupiedPoints.Length;
            if (!occupiedPoints[index]) {
                return index;
            }
        }
        return -1;
    }
}