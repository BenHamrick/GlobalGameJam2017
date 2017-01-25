using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;

public class SpawnPoolSettings : ScriptableObject
{
    public List<SpawnItem> spawnItemList;
}

[System.Serializable]                                                          
public class SpawnItem
{
    public GameObject prefab;
    public int amount;
}
