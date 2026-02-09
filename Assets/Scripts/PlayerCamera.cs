using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    public InputActionReference zoomAction;
    private Camera _camera;
    public float zoomMin = 25f;
    public float zoomMax = 5f;
    public float zoomStep = 1f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        // Input can be +1 or -1, or exceptionally higher or lower if the user scrolls more than once per frame,
        // however we do not have to account for this.
        // We still have to use the float type, even if the output is guaranteed to be an int.
        var input = zoomAction.action.ReadValue<float>();
        if (input == 0) return;
        // -= will invert scrolling
        _camera.orthographicSize -= input * zoomStep;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, zoomMax, zoomMin);
    }
}
