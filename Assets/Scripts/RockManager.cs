using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockManagerConfig
{
    public int rockCount;
    public float rockSpacing;
    public float leftMostRockSpawn;
    public GameObject rockPrefab;
}

public class RockManager : MonoBehaviour
{
    public RockConfig rockConfig;
    public RockManagerConfig managerConfig;

    List<RockController> rocks = new List<RockController>();

    void Awake()
    {
        for (int i = 0; i < managerConfig.rockCount; ++i) {
            var rockObject = Instantiate(managerConfig.rockPrefab) as GameObject;
            var rock = rockObject.GetComponent<RockController>();
            rocks.Add(rock);
            rock.Init(rockConfig, new Vector3(managerConfig.leftMostRockSpawn + i * managerConfig.rockSpacing, 0f));
        }
    }
}