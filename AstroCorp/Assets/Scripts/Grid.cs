using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridChunk
{
    public Vector2Int size;
    public Dictionary<Vector2Int, Machine> machines;

    public void GenerateChunk()
    {
        
    }
}

[System.Serializable]
public class Grid : MonoBehaviour
{
    public Vector2Int size;
    public Dictionary<Vector2Int, GridChunk> chunks;
}