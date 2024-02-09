using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    private const float DELETE_DISTANCE = 100f;
    private const float SAFE_INTERVAL = 5f;

    [SerializeField] private float maxMovementSpeed = 5f;
    [SerializeField] private LayerMask blockingLayer;

    [Header("Lights")]
    [SerializeField] private GameObject stopSignals;
    [SerializeField] private GameObject rightSignal;
    [SerializeField] private GameObject leftSignal;


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
    private bool switchingLine = false;

    private float intervalBetweenLines;

    private Player player;
    private Rigidbody2D rb2D;
    private BoxCollider2D carCollider;

    private void Start()
    {
        player = GameManager.instance.Player;
        rb2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponent<BoxCollider2D>();


        stopSignals.SetActive(false);

        MovementSpeed = maxMovementSpeed;
        intervalBetweenLines = GameManager.instance.roadManager.GetIntervalBetweenLines();


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

    private IEnumerator BlinkCoroutine(GameObject objectToBlink) 
    {
        float delay = 0.15f;
        objectToBlink.SetActive(false);

        while (true)
        {
            yield return new WaitForSeconds(delay);
            objectToBlink.SetActive(true);
            yield return new WaitForSeconds(delay);
            objectToBlink.SetActive(false);
        }
    }

    private IEnumerator SwitchLineCoroutine(float newXPosition) 
    {
        float timeToSwithLine = 3f;
        float delayToSwitchLine = 1f;
        float timer = timeToSwithLine;

        switchingLine = true;

        Coroutine blinkCoroutine;
        GameObject blinkObject;

        if (Mathf.Abs(newXPosition) - Mathf.Abs(transform.position.x) > 0)
        {
            blinkCoroutine = StartCoroutine(BlinkCoroutine(rightSignal));
            blinkObject = rightSignal;
        }
        else
        {
            blinkCoroutine = StartCoroutine(BlinkCoroutine(leftSignal));
            blinkObject = leftSignal;
        }

        yield return new WaitForSeconds(delayToSwitchLine);


        while (timer > 0)
        {
            float slideSpeed = MovementSpeed / 5 /* <-- slide speed */ * Time.deltaTime;
            Vector3 newPosition = new Vector3(newXPosition,transform.position.y,transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position,newPosition,slideSpeed);


            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
            if (Mathf.Abs(transform.position.x - newXPosition) <= Mathf.Epsilon)
            {
                break;
            }
        }

        StopCoroutine(blinkCoroutine);
        blinkObject.SetActive(false);
        switchingLine = false;
    }

    private void CheckForwardDirection() 
    {
        carCollider.enabled = false;

        if (Physics2D.Raycast(transform.position, transform.up, MovementSpeed, blockingLayer))
        {
            // Have something in front

            haveObstacleInFront = true;
            if (switchingLine == false)
            {
                CheckSideDirections();
            }
        }
        else
        {
            //Have NOT something in front
            haveObstacleInFront = false;
        }

        carCollider.enabled = true;
    }

    private void CheckSideDirections() 
    {
        //!!!
        //Every road must be centered on X -> 0
        //!!!

        if (Physics2D.Linecast(transform.position + transform.right * intervalBetweenLines + Vector3.down * SAFE_INTERVAL,
            transform.position + transform.right * intervalBetweenLines + Vector3.up * SAFE_INTERVAL,blockingLayer) == false)
        {
            // Have free right side

            StartCoroutine(SwitchLineCoroutine(transform.position.x + transform.right.x * intervalBetweenLines));

            return;
        }
        if (Physics2D.Linecast(transform.position + transform.right * -1 * intervalBetweenLines + Vector3.down * SAFE_INTERVAL,
            transform.position + transform.right * -1 * intervalBetweenLines + Vector3.up * SAFE_INTERVAL, blockingLayer) == false)
        {
            // Have free left side
            if (transform.position.x > 0)
            {
                //On right
                if (transform.position.x - intervalBetweenLines < 0)
                {
                    //Can NOT go to left
                    return;
                }
            }
            else
            {
                //On left
                if (transform.position.x + intervalBetweenLines > 0)
                {
                    //Can NOT go to left
                    return;
                }
            }

            // Can go to left
            StartCoroutine(SwitchLineCoroutine(transform.position.x + transform.right.x * -1 * intervalBetweenLines));

            return;
        }
        //Have no free sides
    }

    private void Accelerate() 
    {
        MovementSpeed += maxMovementSpeed / 10f * Time.deltaTime; // <-- acceleration speed

        stopSignals.SetActive(false);
    }

    private void Brake()
    {
        MovementSpeed -= maxMovementSpeed / 2f * Time.deltaTime; // <-- braking speed

        stopSignals.SetActive(true);
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
        if (Vector2.Distance(transform.position,player.transform.position) > DELETE_DISTANCE)
        {
            Destroy(gameObject);
        }
    }

    public void ModifyMaxMovementSpeed(float speedModifier) 
    {
        maxMovementSpeed *= speedModifier;
    }

}
