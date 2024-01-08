using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    private const float DELETE_INTERVAL = 20f;

    [SerializeField] private float maxMovementSpeed = 5f;
    [SerializeField] private LayerMask blockingLayer;

    [SerializeField] private GameObject stopSignals;

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
            else if (movementSpeed < 0)
            {
                movementSpeed = 0;
            }
        }
    }

    private bool haveObstacleInFront = false;

    private Player player;
    private Rigidbody2D rb2D;
    private BoxCollider2D carCollider;

    private void Start()
    {
        player = GameManager.instance.player;
        rb2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponent<BoxCollider2D>();


        stopSignals.SetActive(false);

        MovementSpeed = maxMovementSpeed;


        StartCoroutine(CheckDirectionsCoroutine());
    }

    private void Update()
    {
        Move();
        Delete();
    }

    private IEnumerator CheckDirectionsCoroutine() 
    {
        float delay = 0.4f;
        while (true)
        {
            CheckForwardDirection();
            yield return new WaitForSeconds(delay);
        }
    }

    private void CheckForwardDirection() 
    {
        carCollider.enabled = false;

        if (Physics2D.Raycast(transform.position, transform.up, MovementSpeed, blockingLayer))
        {
            // Have something in front
            haveObstacleInFront = true;

            stopSignals.SetActive(true);
        }
        else
        {
            //Have NOT something in front
            haveObstacleInFront = false;

            stopSignals.SetActive(false);
        }

        carCollider.enabled = true;
    }

    private void Accelerate() 
    {
        MovementSpeed += maxMovementSpeed / 10f * Time.deltaTime; // <-- acceleration speed
    }

    private void Brake()
    {
        MovementSpeed -= maxMovementSpeed / 4f * Time.deltaTime; // <-- braking speed
    }

    private void Move() 
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)(transform.position + transform.up), MovementSpeed * Time.deltaTime);

        if (haveObstacleInFront)
        {
            Brake();
        }
        else
        {
            Accelerate();
        }
    }
    private void Delete() 
    {
        if (transform.position.y + DELETE_INTERVAL < player.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
