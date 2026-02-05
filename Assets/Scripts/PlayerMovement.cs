using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveForce = 25f;
    private Rigidbody2D rb;
    public InputActionReference moveAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        rb.gravityScale = 0;
        rb.angularDamping = 0f;
        rb.linearDamping = 10f;
    }

    private void Update()
    {
        var input = moveAction.action.ReadValue<Vector2>();
        var force = new Vector2(input.x, input.y);
        force *= moveForce * Time.deltaTime;
        rb.AddForce(force, 0);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, 10f);
    }
}