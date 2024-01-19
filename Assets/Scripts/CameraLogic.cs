using UnityEngine;
using System;

public class CameraLogic : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    private Player player;

    [SerializeField] private MinMax<float> cameraFOVBounds;

    [SerializeField] private MinMax<float> cameraYOffsetBounds;
    private float cameraYOffset;

    [SerializeField] private MinMax<float> cameraXBounds;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();

        objectToFollow = GameManager.instance.player.transform;

        player = GameManager.instance.player;

        GameManager.instance.player.OnMovementSpeedChanged += Camera_OnMovementSpeedChanged;

        cameraYOffset = cameraYOffsetBounds.min;

    }

    private void Update()
    {
        MoveCamera();

    }

    private void MoveCamera() 
    {
        float interpolationValue = 10f;

        float newXPosition = objectToFollow.position.x;
        newXPosition = Mathf.Clamp(newXPosition, cameraXBounds.min, cameraXBounds.max);

        Vector3 newPosition = new Vector3(newXPosition, objectToFollow.position.y + cameraYOffset, -10);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newPosition, interpolationValue * Time.deltaTime);
    }

    private void Camera_OnMovementSpeedChanged(object Sender, EventArgs e) 
    {
        ChangeCameraFOV();
        ChangeCameraYOffset();
    }

    private void ChangeCameraFOV()
    {
        float interpolationValue = 0.2f;

        float deltaFOVBounds = cameraFOVBounds.max - cameraFOVBounds.min;
        float deltaPlayerSpeed = player.GetMovementSpeedBounds().max - player.GetMovementSpeedBounds().min;
        float ratio = deltaFOVBounds / deltaPlayerSpeed;

        float newCameraFOV = cameraFOVBounds.min + ((player.MovementSpeed - player.GetMovementSpeedBounds().min) * ratio);

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize,newCameraFOV, interpolationValue);
    }

    private void ChangeCameraYOffset() 
    {
        float interpolationValue = 0.2f;

        float deltaYOffsetBounds = cameraYOffsetBounds.max - cameraYOffsetBounds.min;
        float deltaPlayerSpeed = player.GetMovementSpeedBounds().max - player.GetMovementSpeedBounds().min;
        float ratio = deltaYOffsetBounds / deltaPlayerSpeed;

        float newCameraYOffset = cameraYOffsetBounds.min + ((player.MovementSpeed - player.GetMovementSpeedBounds().min) * ratio);

        cameraYOffset = Mathf.Lerp(cameraYOffset, newCameraYOffset, interpolationValue);
    }

}
