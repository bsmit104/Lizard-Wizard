using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int behaviourIndex = 0;  // determine how this enemy should behave
    private Vector3 origin;
    private Vector3 currentPosition;
    public float moveSpeed = 2f;
    private Vector3 verticalEnd = Vector3.up * 4f;
    private Vector3 horizontalEnd = Vector3.right * 4f;
    private bool movingPositive = true; // determing if moving right or up



    void Start(){
        origin = transform.position;
        currentPosition = transform.position;
        verticalEnd = origin + verticalEnd;
        horizontalEnd = origin + horizontalEnd;
    }
    void Update()
    {
        // different functions depending on behaviour index
        switch (behaviourIndex){
            case 1:
                verticalBehaviour();
                break;
            case 2:
                horizontalBehaviour();
                break;
            default:
                break;
        }
    }

    private void verticalBehaviour(){
        if (movingPositive){
            currentPosition = Vector3.MoveTowards(currentPosition, verticalEnd, moveSpeed*Time.deltaTime);
        }else{
            currentPosition = Vector3.MoveTowards(currentPosition, origin , moveSpeed*Time.deltaTime);
        }
        transform.position = currentPosition;
        if (Vector3.Distance(transform.position, verticalEnd) < 0.01f || Vector3.Distance(transform.position, origin) < 0.01f){
            movingPositive = !movingPositive;
        }
    }

    private void horizontalBehaviour(){
        
        if (movingPositive){
            currentPosition = Vector3.MoveTowards(currentPosition, horizontalEnd, moveSpeed*Time.deltaTime);
        }else{
            currentPosition = Vector3.MoveTowards(currentPosition, origin, moveSpeed*Time.deltaTime);
        }
        transform.position = currentPosition;
        if (Vector3.Distance(transform.position, horizontalEnd) < 0.01f || Vector3.Distance(transform.position, origin) < 0.01f){
            movingPositive = !movingPositive;
        }
    }

    public void changeBehaviour(int newIndex){
        behaviourIndex = newIndex;
        if(behaviourIndex == 2){
            movingPositive = origin.x >= 0 ? false : true;
        }
    }

    public void changePosition(Vector3 newPos){
        transform.position = newPos;
        origin = newPos;
        currentPosition = newPos;
        verticalEnd = origin + Vector3.up * 4f;
        horizontalEnd = origin + Vector3.right * 4f;
        if(behaviourIndex == 2){
            movingPositive = origin.x >= 0 ? false : true;
        }
    }
}
