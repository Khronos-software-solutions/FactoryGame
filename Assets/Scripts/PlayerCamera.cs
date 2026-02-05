using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    public InputActionReference zoomAction;
    private Camera _camera;
    public float zoomMin = 5f;
    public float zoomMax = 1f;
    public float zoomStep = 0.2f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        var input = zoomAction.action.ReadValue<float>();
        if (input == 0) return;
        // input/abs(input) gives either +1 or -1 depending on the scroll direction
        var newSize = input / Mathf.Abs(input) * zoomStep;
        _camera.orthographicSize = Mathf.Clamp(newSize, zoomMin, zoomMax);
    }
}
