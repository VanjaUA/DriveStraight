using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] carsToSpawn;
    [SerializeField]
    private float[] spawnCarPositionsX_Road2x2;
    [SerializeField]
    private float[] spawnCarPositionsX_Road3x3;

    private RoadManager roadManager;

    private void Start()
    {
        roadManager = GameManager.instance.roadManager;
        StartCoroutine(SpawnCars());
    }


    private IEnumerator SpawnCars() 
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            GameObject carToSpawn = carsToSpawn[Random.Range(0, carsToSpawn.Length)];
            Vector3 positionToSpawn;
            // TODO: change this fucking system make struct or scriptable object
            if (roadManager.CurrentRoad == RoadManager.RoadCode.Road3x3)
            {
                 positionToSpawn = new Vector3(spawnCarPositionsX_Road3x3[Random.Range(0, spawnCarPositionsX_Road3x3.Length)], roadManager.LastYPosition, 0);
            }
            else
            {
                 positionToSpawn = new Vector3(spawnCarPositionsX_Road2x2[Random.Range(0, spawnCarPositionsX_Road2x2.Length)], roadManager.LastYPosition, 0);
            }
            GameObject car = Instantiate(carToSpawn,positionToSpawn, Quaternion.identity);

            if (positionToSpawn.x > 0)
            {
                //by movement
            }
            else
            {
                //against the movement
                car.transform.localEulerAngles = new Vector3(0, 0, 180f);
            }
        }
    }
}
