#nullable enable
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Grid : MonoBehaviour
{
    public Vector2Int chunkSize = new(16, 16);
    public Dictionary<Vector2Int, GridChunk> chunks = new();

    // Get chunk coordinate from global
    public Vector2Int GlobalToChunkCoord(Vector2Int globalPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt((float)globalPos.x / chunkSize.x),
            Mathf.FloorToInt((float)globalPos.y / chunkSize.y)
        );
    }

    // Get local coordinate from global
    public Vector2Int GlobalToLocalCoord(Vector2Int globalPos)
    {
        return new Vector2Int(
            Mathf.Abs(globalPos.x % chunkSize.x),
            Mathf.Abs(globalPos.y % chunkSize.y)
        );
    }

    // Gets chunk info and creates one if not present
    public GridChunk LoadChunk(Vector2Int chunkCoord)
    {
        if (chunks.TryGetValue(chunkCoord, out var loadChunk)) return loadChunk;
        var chunkObj = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
        var chunk = chunkObj.AddComponent<GridChunk>();
        chunk.size = chunkSize;
        chunkObj.transform.parent = transform;
        chunk.Generate();
        chunks.Add(chunkCoord, chunk);
        return chunks[chunkCoord];
    }

    // Place machine at coordinate
    public void PlaceMachine(Vector2Int globalPos, Machine machine)
    {
        var chunkCoord = GlobalToChunkCoord(globalPos);
        var localCoord = GlobalToLocalCoord(globalPos);
        var chunk = LoadChunk(chunkCoord);
        chunk.PlaceMachine(localCoord, machine);
    }

    public Machine? GetMachine(Vector2Int globalPos)
    {
        var chunkCoord = GlobalToChunkCoord(globalPos);
        var localCoord = GlobalToLocalCoord(globalPos);
        return chunks.TryGetValue(chunkCoord, out var chunk) ? chunk.machines.GetValueOrDefault(localCoord) : null;
    }
}