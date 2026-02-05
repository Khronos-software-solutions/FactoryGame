using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
     private float moveForce = 25f; 
     private Rigidbody2D rb;
    
 void Start()
     {
         rb = GetComponent<Rigidbody2D>();
         if (rb == null) Debug.LogError("PlayerMovementScript not attached to a Rigidbody2D GameObject");
         else
         {
             rb.gravityScale = 0;
             rb.angularDamping = 0f;
             rb.linearDamping = 10f;
         }
         
         
     }
 void Update()
 {
     float moveX = Input.GetAxis("Horizontal");
     float moveY = Input.GetAxis("Vertical");
     Vector2 force = new Vector2(moveX, moveY);
     force *= moveForce * Time.deltaTime;
     rb.AddForce(force, ForceMode2D.Force);
     if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveY) > 0.1f) 
         Debug.Log($"Moving: X={moveX}, Y={moveY}, Force={force}");
     if  (rb.linearVelocity.magnitude > 10f) 
         rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, 10f);
 }
}
