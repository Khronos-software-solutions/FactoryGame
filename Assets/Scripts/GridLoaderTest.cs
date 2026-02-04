using UnityEngine;


[System.Serializable]
[RequireComponent(typeof(GridLoader))]
public class GridLoaderTest : MonoBehaviour
{ 
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var globalPos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));
        var machine = new GameObject().AddComponent<Machine>();
        machine.name = "TestMachine";
        machine.GetComponent<Machine>().type = Resources.Load<MachineType>("Machines/Fabricator");
        Debug.Log($"type: {machine.GetComponent<Machine>().type}");
        GetComponent<GridLoader>().grid.PlaceMachine(globalPos, machine);
        Debug.Log($"Placed machine at {globalPos}");
    }
}