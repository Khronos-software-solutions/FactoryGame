using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Generation;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GridChunk : MonoBehaviour
{
    public Vector2Int size;
    public Vector2Int relativePosition;
    
    public int seed;
    public float noiseScale;
    public Tilemap tilemap;
    public Dictionary<Vector2Int, Machine> machines = new();
    
    public List<TileHeightProfile> _tiles;
    private GameObject _tilemapObject;
    private FastNoiseLite _random; // Random generator to 
    private UnityEngine.Grid _ownGrid;

    public void Initialize()
    {
        _tilemapObject = new GameObject("Tilemap");
        _tilemapObject.transform.parent = transform;
        _tilemapObject.AddComponent<Tilemap>();
        _tilemapObject.AddComponent<TilemapRenderer>();
        tilemap = _tilemapObject.GetComponent<Tilemap>();
        _tiles = Resources.LoadAll<TileHeightProfile>("Tiles").ToList();
        _ownGrid = this.AddComponent<UnityEngine.Grid>();
        machines = new Dictionary<Vector2Int, Machine>();
        _random = new FastNoiseLite(seed);
        _random.SetFrequency(0.03f);
        _random.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        _random.SetFractalType(FastNoiseLite.FractalType.Ridged);
        Generate();
    }

    private void GenerateTilemap()
    {
        tilemap.ClearAllTiles();
    
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var height = _random.GetNoise((x + relativePosition.x * size.x) * noiseScale, (y + relativePosition.y * size.x) * noiseScale);
                height = (height + 1) / 2; 
                tilemap.SetTile(new Vector3Int(x, y, 0), _tiles.Find(thp => thp.maxHeight >= height && thp.minHeight <= height).tile);
            }
        }
    }

    private void Generate()
    {
        GenerateTilemap();
        foreach (var machine in machines)
        {
            machine.Value.transform.parent = transform;
            machine.Value.instance = Instantiate(machine.Value.type.prefab, machine.Value.transform, true);
            machine.Value.instance.transform.localPosition = new Vector3(
                machine.Key.x + 0.5f, machine.Key.y + 0.5f, 0.1f);
        }
    }

    public void PlaceMachine(Vector2Int localPos, Machine machine)
    {
        machines.Add(localPos, machine);
        machine.gameObject.transform.parent = transform;
        machine.gameObject.transform.localPosition = new Vector3(localPos.x + 0.5f, localPos.y + 0.5f, 0);
        machine.gameObject.name = machine.name;
        machine.gameObject.transform.localPosition = new Vector3(0, 0, -0.1f);
        machine.instance = Instantiate(machine.type.prefab, machine.transform, true);
    }
}