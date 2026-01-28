using UnityEngine;

public enum Material
{
    Iron,
    Copper,
    Aluminium,
    Coal,
    Nickel,
    Chrome,
    Uranium,
    Vanadium
}

public class OrePatch
{
    public Material material;
    public float rarity;
    public int patchSize;
}
