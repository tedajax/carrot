using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PrefabRegistryItem
{
    public string name;
    public GameObject prefab;
}

[Serializable]
public struct GameConfig
{
    public PrefabRegistryItem[] prefabRegistry;
    public float gravity;
}

public class PrefabRegistry
{
    private Dictionary<string, GameObject> prefabs;

    public PrefabRegistry(PrefabRegistryItem[] items)
    {
        prefabs = new Dictionary<string, GameObject>();

        foreach (var item in items) {
            if (!prefabs.ContainsKey(item.name)) {
                prefabs.Add(item.name, item.prefab);
            }
        }
    }

    public GameObject GetPrefab(string name)
    {
        if (prefabs.ContainsKey(name)) {
            return prefabs[name];
        }
        return null;
    }
}

public class GameManager : MonoBehaviour
{
    public GameConfig gameConfig;
    public RockManager rockManager;
    public CarrotManager carrotManager;
    public HUDController hudController;
    public RandomGen Random { get; private set; }

    private PrefabRegistry prefabRegistry;

    void Awake()
    {
        // TODO: seeding
        Random = new RandomGen(0);
        prefabRegistry = new PrefabRegistry(gameConfig.prefabRegistry);
    }

    public GameObject GetPrefab(string name)
    {
        return prefabRegistry.GetPrefab(name);
    }
}