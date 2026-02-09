using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generation
{
    [CreateAssetMenu(fileName = "NewOreProfile", menuName = "Ore Generation Profile")]
    public class OreProfile : ScriptableObject
    {
        public int weight; // Chance weight of generation
        public Tile tile;
    }
}