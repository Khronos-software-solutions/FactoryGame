using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MachineType", menuName = "Machine Type")]
public class MachineType : ScriptableObject
{
    public string machineName;
    public GameObject prefab;
    public Vector2Int footPrint;
    public float powerConsumption = 0f; // Temp

    
}

[System.Serializable]
public class MachineRecipe
{
    public Dictionary<Resource, int> inputs = new();
    public Dictionary<Resource, int> outputs = new();
    public float processingTime = 1f;
    public string nameOverride;
}