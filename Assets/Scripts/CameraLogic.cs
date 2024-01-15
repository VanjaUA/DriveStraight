using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();

        objectToFollow = GameManager.instance.player.transform;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = new Vector3(objectToFollow.position.x,objectToFollow.position.y,-10);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,newPosition,0.5f);
    }
}
