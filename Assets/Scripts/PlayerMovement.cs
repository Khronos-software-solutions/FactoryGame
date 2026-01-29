using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
     private float moveForce = 5f; // Kracht per seconde
     private Rigidbody2D rb;
    
 void Start()
     {
         rb = GetComponent<Rigidbody2D>();
     }
 void FixedUpdate()
 {
     float moveX = Input.GetAxis("Horizontal");
     float moveY = Input.GetAxis("Vertical");
     Vector2 force = new Vector2(moveX, moveY);
     force *= moveForce * Time.deltaTime;
     rb.AddForce(force, ForceMode2D.Impulse);
 }
}
