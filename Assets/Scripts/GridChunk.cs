using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridChunk : MonoBehaviour
{
    public Vector2Int size;
    public Dictionary<Vector2Int, Machine> machines = new();

    private void Start()
    {
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        machines = new Dictionary<Vector2Int, Machine>();
        gameObject.transform.localScale = new Vector3(size.x, size.y, 1);
        var temp = GameObject.CreatePrimitive(PrimitiveType.Quad);
        gameObject.GetComponent<MeshFilter>().mesh = temp.GetComponent<MeshFilter>().mesh;
        Destroy(temp);
    }

    public void Generate()
    {
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