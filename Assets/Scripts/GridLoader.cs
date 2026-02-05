#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GridLoader : MonoBehaviour
{
    public Grid grid;
    public List<Transform> renderedPositions = new();
    public int renderDistance = 5;
    
    [Header("Generation settings")]
    public float noiseScale;
    public int seed;
    private readonly Dictionary<Vector2Int, GridChunk> _visible = new();

    private void Start()
    {
        grid = gameObject.AddComponent<Grid>();
        grid.noiseScale = noiseScale;
        grid.seed = seed;
    }

    private void Update()
    {
        foreach (var centerChunk in renderedPositions.Select(pos => grid.GlobalToChunkCoord(new Vector2Int((int) Mathf.Floor(pos.position.x), (int) Mathf.Floor(pos.position.y)))))
        {
            for (var x = -renderDistance; x <= renderDistance; x++)
            {
                for (var y = -renderDistance; y <= renderDistance; y++)
                {
                    var chunkCoord = new Vector2Int(centerChunk.x + x, centerChunk.y + y);
                    if (_visible.ContainsKey(chunkCoord)) continue;
                    var chunk = grid.LoadChunk(chunkCoord);
                    _visible[chunkCoord] = chunk;
                }
            }
        }

        foreach (var chunk in _visible)
        {
            var chunkCoord = chunk.Key;
            var chunkData = chunk.Value;
            // Set chunk GameObject position in world space
            chunkData.transform.position = new Vector3(
                chunkCoord.x * grid.chunkSize.x,
                chunkCoord.y * grid.chunkSize.y,
                0
            );
            // Ensure chunk is active in the scene
            if (!chunkData.gameObject.activeInHierarchy)
            {
                chunkData.gameObject.SetActive(true);
            }
        }
    }
}