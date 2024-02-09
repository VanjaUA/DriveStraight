using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MIN_MOVEMENT_SPEED = 1f;

    [SerializeField] private float maxMovementSpeed;

    [SerializeField] private float maxFuelCapacity;

    [SerializeField] private LayerMask roadLayerMask;

    [SerializeField] private GameObject stopSignals;

    private BoxCollider2D carCollider;

    [SerializeField] private SoundManager.Sound coinPickUpSound;
    [SerializeField] private SoundManager.Sound crashSound;

    [SerializeField] private SoundManager.Sound carEngineSound;
    private AudioSource carAudio;

    private Coroutine engineSoundCoroutine;

    private bool isDead = false;

    private float currentFuel;

    public float CurrentFuel
    {
        get { return currentFuel; }
        private set 
        {
            currentFuel = value;

            if (currentFuel > maxFuelCapacity)
            {
                currentFuel = maxFuelCapacity;
            }
            if (currentFuel < 0)
            {
                currentFuel = 0;
            }

            GameManager.instance.uiManager.UpdateFuelFillAmount(maxFuelCapacity,currentFuel);
        }
    }


    private int coinsTaken;

    public int CoinsTaken
    {
        get { return coinsTaken; }
        private set
        {
            coinsTaken = value;
            GameManager.instance.uiManager.UpdateCoinsText(CoinsTaken);
        }
    }


    public EventHandler OnMovementSpeedChanged;


    private float movementSpeed;

    public float MovementSpeed
    {
        get { return movementSpeed; }
        private set 
        {
            movementSpeed = value;

            OnMovementSpeedChanged?.Invoke(this,EventArgs.Empty);

            if (movementSpeed > maxMovementSpeed)
            {
                movementSpeed = maxMovementSpeed;
            }
            else if (movementSpeed < MIN_MOVEMENT_SPEED)
            {
                if (CurrentFuel > 0)
                {
                    movementSpeed = MIN_MOVEMENT_SPEED;
                }
                else
                {
                    if (movementSpeed < 0)
                    {
                        movementSpeed = 0;
                    }
                }
            }
        }
    }


    private void Start()
    {
        MovementSpeed = MIN_MOVEMENT_SPEED;

        isDead = false;

        CoinsTaken = 0;

        CurrentFuel = maxFuelCapacity;

        carAudio = GetComponent<AudioSource>();

        engineSoundCoroutine = StartCoroutine(PlayEngineSound(carEngineSound));

        carCollider = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        //Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 inputVector = GameManager.instance.uiManager.GetInputVector();

        Move(inputVector);
        Slide(inputVector);
    }

    private void Move(Vector2 inputVector)
    {
        if (CurrentFuel <= 0)
        {
            if (engineSoundCoroutine != null)
            {
                StopCoroutine(engineSoundCoroutine);
                engineSoundCoroutine = null;

                carAudio.Stop();
            }


            stopSignals.SetActive(true);

            MovementSpeed -=  Time.deltaTime * maxMovementSpeed / 10f /*<-- slowing down speed */;

            if (inputVector.y < 0)
            {
                MovementSpeed += inputVector.y * Time.deltaTime * maxMovementSpeed / 2 /*<-- acceleration speed */;
            }

            if (MovementSpeed <= 0)
            {
                GameOver();
            }


            return;
        }


        MovementSpeed += inputVector.y * Time.deltaTime * maxMovementSpeed / 2 /*<-- acceleration speed */;
        if (inputVector.y < 0)
        {
            stopSignals.SetActive(true);
        }
        else
        {
            stopSignals.SetActive(false);
        }

        CurrentFuel -= ((MovementSpeed / maxMovementSpeed) * Time.deltaTime) * 10f; /*<-- how much fuel you use */
    }

    private void Slide(Vector2 inputVector) 
    {
        transform.position += TryToMove(new Vector3(inputVector.x * MovementSpeed / 3 /*<-- slide speed */ * Time.deltaTime,
            MovementSpeed * Time.deltaTime, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (engineSoundCoroutine != null)
        {
            StopCoroutine(engineSoundCoroutine);
            engineSoundCoroutine = null;

            carAudio.Stop();
        }

        SoundManager.instance.PlaySound(crashSound);

        GameOver();
    }


    private Vector3 TryToMove(Vector3 movePosition) 
    {

        if (Physics2D.OverlapBox((Vector2)transform.position + (Vector2)movePosition + carCollider.offset,carCollider.size,0f,roadLayerMask) == false)
        {
            return movePosition;
        }
        return new Vector3(0,movePosition.y);
    }

    private IEnumerator PlayEngineSound(SoundManager.Sound engineSound) 
    {
        while (true)
        {
            carAudio.PlayOneShot(engineSound.audioClip, engineSound.volume);
            yield return new WaitForSeconds(engineSound.audioClip.length);
        }
    }

    private void GameOver() 
    {
        isDead = true;

        GameManager.instance.GameOver();
    }



    public void PickObject(PickableObject.ObjectType objectType) 
    {

        switch (objectType)
        {
            case PickableObject.ObjectType.Coin:
                CoinsTaken++;
                SoundManager.instance.PlaySound(coinPickUpSound);
                break;
            case PickableObject.ObjectType.Fuel:
                CurrentFuel += maxFuelCapacity / 3f; //Change in future
                if (engineSoundCoroutine == null)
                {
                    engineSoundCoroutine = StartCoroutine(PlayEngineSound(carEngineSound));
                }
                break;
        }
    }

    public MinMax<float> GetMovementSpeedBounds() 
    {
        return new MinMax<float> {min = MIN_MOVEMENT_SPEED,max = maxMovementSpeed };
    }
}