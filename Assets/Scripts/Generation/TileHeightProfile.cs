using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generation
{
    [CreateAssetMenu(fileName = "NewTileHeightProfile", menuName = "Tile Height Profile", order = 1)]
    public class TileHeightProfile : ScriptableObject
    {
        public string name; // A named description of the tile
        public float minHeight; // 0-1f
        public float maxHeight; // 0-1f
        public TileBase tile;
    }
}
