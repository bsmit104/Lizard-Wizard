using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject player;
    private float distanceToPlayer;
    private int behaviourIndex;  // determine how this enemy should behave
    private Vector3 origin;
    private Vector3 currentPosition;
    private Vector3 originSize;
    private Vector3 temp;
    public float moveSpeed = 2f;
    private Vector3 verticalEnd = Vector3.up * 4f;
    private Vector3 horizontalEnd = Vector3.right * 4f;
    private bool movingPositive = true; // determing if moving right or up
    public float growFactor = 1.25f;    // how big growingEnemies can grow
    public GameObject bullet;
    public Transform bulletPos;
    private float timeToFire;
    public float shootInterval;       // how much time between bullet shots
    public float detectionRadius;



    void Start(){
        behaviourIndex = 0;
        origin = transform.position;
        currentPosition = transform.position;
        verticalEnd = origin + verticalEnd;
        horizontalEnd = origin + horizontalEnd;
        originSize = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player");
        timeToFire = shootInterval / 2;
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
            case 3:
                growingBehaviour();
                break;
            case 4:
                shootingBehaviour();
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

    private void growingBehaviour(){
        if (movingPositive){
            temp = transform.localScale;
            temp.x += Time.deltaTime;
            temp.y += Time.deltaTime;
        }else{
            temp = transform.localScale;
            temp.x -= Time.deltaTime;
            temp.y -= Time.deltaTime;
        }
        transform.localScale = temp;

        // if size reaches a threshold, invert growth
        if (transform.localScale.x >= originSize.x*growFactor || transform.localScale.x <= originSize.x){
            movingPositive = !movingPositive;
        }
    }


    private void shootingBehaviour(){
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (player.transform.position.y > transform.position.y + 5){
            changeBehaviour(0);
        }
        if (distanceToPlayer < detectionRadius){
            timeToFire += Time.deltaTime;

            if (timeToFire > shootInterval){
            timeToFire = 0;
            Instantiate(bullet, bulletPos.position, Quaternion.identity);       // creates the bullet which will know its direction and velocity
        }
        }
    }

    public void changeBehaviour(int newIndex){
        behaviourIndex = newIndex;
        transform.localScale = originSize;
        if(behaviourIndex == 2){
            movingPositive = origin.x >= 0 ? false : true;
        }
    }

    public void changePosition(Vector3 newPos){
        transform.localScale = originSize;
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
