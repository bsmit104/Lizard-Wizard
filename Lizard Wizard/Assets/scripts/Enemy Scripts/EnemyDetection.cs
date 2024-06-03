using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private Vector3 defaultYPosition = new Vector3(0f,-10f,0f);
    public EnemyBehaviour behaviour;

    void Start(){
        behaviour = GetComponent<EnemyBehaviour>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            //add to player score
            PlayerManager.Instance.ChangePlayerHealth(-1);
            behaviour.changeBehaviour(0);
            behaviour.changePosition(defaultYPosition);
        }
    }
}
