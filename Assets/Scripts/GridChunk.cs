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
    public Tilemap oreTilemap;
    public Dictionary<Vector2Int, Machine> machines = new();
    public float oreThreshold = 0.80f;
    public List<TileHeightProfile> _tiles;
    public List<OreProfile> _ores;
    private GameObject _tilemapObject;
    private GameObject _oreTilemapObject;
    private FastNoiseLite _random;
    private FastNoiseLite _randomOre;
    private System.Random _oreRandom;
    private UnityEngine.Grid _ownGrid;

    public void Initialize()
    {
        machines = new Dictionary<Vector2Int, Machine>();
        _ownGrid = this.AddComponent<UnityEngine.Grid>();
        _oreRandom = new System.Random(seed);
        _tilemapObject = new GameObject("Tilemap") { transform = { parent = transform } };
        _tilemapObject.AddComponent<Tilemap>();
        _tilemapObject.AddComponent<TilemapRenderer>();
        _oreTilemapObject = new GameObject("Ore Tilemap") { transform = { parent = transform } };
        _oreTilemapObject.AddComponent<Tilemap>();
        _oreTilemapObject.AddComponent<TilemapRenderer>();
        tilemap = _tilemapObject.GetComponent<Tilemap>();
        oreTilemap = _oreTilemapObject.GetComponent<Tilemap>();
        _tiles = Resources.LoadAll<TileHeightProfile>("Tiles").ToList();
        _ores = Resources.LoadAll<OreProfile>("Ores").ToList();
        _random = new FastNoiseLite(seed);
        _random.SetFrequency(0.03f);
        _random.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        _random.SetFractalType(FastNoiseLite.FractalType.Ridged);
        _randomOre = new FastNoiseLite(seed);
        _randomOre.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        _randomOre.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance2Sub);
        Generate();
    }

    private void GenerateTilemap()
    {
        tilemap.ClearAllTiles();
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var height = _random.GetNoise((x + relativePosition.x * size.x) * noiseScale,
                    (y + relativePosition.y * size.x) * noiseScale);
                height = (height + 1) / 2;
                tilemap.SetTile(new Vector3Int(x, y, 0),
                    _tiles.Find(thp => thp.maxHeight >= height && thp.minHeight <= height).tile);
            }
        }

        tilemap.RefreshAllTiles(); // Oof
    }

    private void GenerateOres()
    {
        oreTilemap.ClearAllTiles();
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var position = new Vector3Int(x, y, 0);
                var value = _random.GetNoise((x + relativePosition.x * size.x) * noiseScale,
                    (y + relativePosition.y * size.x) * noiseScale);
                if (!(value > oreThreshold)) continue;

                // Check sides if an ore tile has already been placed
                if (oreTilemap.HasTile(new Vector3Int(x - 1, y, 0)))
                {
                    oreTilemap.SetTile(position,
                        _ores.Find(i => i.tile == oreTilemap.GetTile(new Vector3Int(x - 1, y, 0))).tile);
                }
                else if (oreTilemap.HasTile(new Vector3Int(x, y - 1, 0)))
                {
                    oreTilemap.SetTile(position,
                        _ores.Find(i => i.tile == oreTilemap.GetTile(new Vector3Int(x, y - 1, 0))).tile);
                }
                else if (oreTilemap.HasTile(new Vector3Int(x - 1, y - 1, 0)))
                {
                    oreTilemap.SetTile(position,
                        _ores.Find(i => i.tile == oreTilemap.GetTile(new Vector3Int(x - 1, y - 1, 0))).tile);
                }
                else
                {
                    var oreIndex = _oreRandom.Next(0, _ores.Count - 1);
                    oreTilemap.SetTile(new Vector3Int(x, y, 0), _ores[oreIndex].tile);
                }
            }
        }
    }

    private void Generate()
    {
        GenerateTilemap();
        GenerateOres();
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
        machine.instance = Instantiate(machine.type.prefab, gameObject.transform, true);
        machine.instance.transform.parent = transform;
        machine.instance.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
        machine.instance.name = machine.name;
    }
}