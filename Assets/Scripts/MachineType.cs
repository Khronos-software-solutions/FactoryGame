using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineType", menuName = "Scriptable Objects/MachineType")]
public class MachineType : ScriptableObject
{
    public string machineName;
    public GameObject prefab;
    public Vector2Int size;
    public float powerConsumption = 0f; // Temp

}

[System.Serializable]
public class MachineRecipe
{
    public Dictionary<Resource, int> inputs;
    public Dictionary<Resource, int> outputs;
    public float processingTime;
    public string nameOverride;
}