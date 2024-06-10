using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    public Animator anim;

    void Start()
    {
        //Debug.Log("C started");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // void Update()
    // {
    //     // Handle horizontal movement
    //     float moveInput = Input.GetAxis("Horizontal");
    //     rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

    //     // Jump automatically if grounded
    //     if (isGrounded && rb.velocity.y <= 0)
    //     {
    //         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    //         isGrounded = false; // Reset isGrounded to prevent continuous jumping
    //     }
    // }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Check if the collision is with a platform from below
    //     if (collision.relativeVelocity.y <= 1)
    //     {
    //         isGrounded = true;
    //     }
    // }
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log("Collision Detected with: " + collision.collider.name);
    //     // Check if the collision is with a platform from below
    //     if (collision.relativeVelocity.y <= 0)
    //     {
    //         isGrounded = true;
    //         Debug.Log("Doodle is grounded and will jump");
    //     }
    //     else
    //     {
    //         Debug.Log("Collision not suitable for jumping: " + collision.relativeVelocity.y);
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform" && collision.contacts[0].normal.y > 0.5)
        {
            isGrounded = true;
            //Debug.Log("Doodle is grounded and will jump");
        }
    }

    void Update()
    {
        // horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jump if grounded
        if (isGrounded && rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false; // Reset isGrounded to prevent continuous jumping
                                //Debug.Log("Doodle jumped");
            anim.Play("Lizard_Jump");
        }
        if (rb.velocity.y<=1){
            anim.Play("Lizard_Idle");
        }
    }
}
// using UnityEngine;

// public class Controller : MonoBehaviour
// {
//     public float moveSpeed = 5f;
//     public float jumpForce = 10f;
//     private Rigidbody2D rb;
//     private bool isGrounded = false;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     void Update()
//     {
//         float moveInput = Input.GetAxis("Horizontal");
//         rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

//         if (isGrounded && rb.velocity.y <= 0)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//             isGrounded = false;
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.relativeVelocity.y <= 0)
//         {
//             isGrounded = true;
//         }
//     }
// }