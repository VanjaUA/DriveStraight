using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MIN_MOVEMENT_SPEED = 1f;

    [SerializeField]
    private float maxMovementSpeed;
    [SerializeField]
    private LayerMask roadLayerMask;
    [SerializeField]
    private GameObject stopSignals;


    private float movementSpeed;

    public float MovementSpeed
    {
        get { return movementSpeed; }
        private set 
        {
            movementSpeed = value;

            if (movementSpeed > maxMovementSpeed)
            {
                movementSpeed = maxMovementSpeed;
            }
            else if (movementSpeed < MIN_MOVEMENT_SPEED)
            {
                movementSpeed = MIN_MOVEMENT_SPEED;
            }
        }
    }



    private void Start()
    {
        MovementSpeed = MIN_MOVEMENT_SPEED;
    }

    private void Update()
    {
        transform.position += TryToMove(new Vector3(Input.GetAxis("Horizontal") * MovementSpeed / 3 /*<-- slide speed */ * Time.deltaTime,
            MovementSpeed * Time.deltaTime, 0));

        MovementSpeed += Input.GetAxis("Vertical") * Time.deltaTime * maxMovementSpeed / 2 /*<-- acceleration speed */;
        if (Input.GetAxis("Vertical") < 0)
        {
            stopSignals.SetActive(true);
        }
        else
        {
            stopSignals.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.GameOver();
    }


    private Vector3 TryToMove(Vector3 movePosition) 
    {
        if (!Physics2D.Raycast(transform.position,new Vector2(movePosition.x,0),0.25f,roadLayerMask)) //if player can slide to side
        {
            return movePosition;
        }
        return new Vector3(0,movePosition.y);
    }
}