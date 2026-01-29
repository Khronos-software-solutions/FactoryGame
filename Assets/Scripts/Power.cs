#nullable enable
using UnityEngine;

public enum EnergyType { Steam, Electricity };

public abstract class Power
{
    public abstract EnergyType type { get; }
    public float amount;
}

public class SteamPower : Power
{
    public override EnergyType type => EnergyType.Steam;
}