using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

/*
Credit - MetalStorm Games
https://www.youtube.com/watch?v=zWy29yeFNX8
*/
public class ScreenWrap : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the screen position of object in Pixels
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        
        // Get the right side of the screen in world units
        float rightSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        float leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;

        // Moving through left side of screen
        if (screenPos.x <= 0 && myRigidBody.velocity.x < 0) {
            transform.position = new Vector2(rightSideOfScreenInWorld, transform.position.y);
        } 
        // Moving through right side of screen
        else if (screenPos.x >= Screen.width && myRigidBody.velocity.x > 0) {
            transform.position = new Vector2(leftSideOfScreenInWorld, transform.position.y);
        }
    }
}
