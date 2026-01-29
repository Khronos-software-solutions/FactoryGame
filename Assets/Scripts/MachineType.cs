using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MachineType", menuName = "Machine Type")]
public class MachineType : ScriptableObject
{
    public string id;
    public string machineName; // Name of the machine in UI
    public GameObject prefab;
    public Vector2Int footPrint; // Size of the machine when placed
    public float powerConsumption = 0f; // Temp

    
}
