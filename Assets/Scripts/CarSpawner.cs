using System.Collections;
using UnityEngine;
using System.Linq;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] carsToSpawn;

    [SerializeField] private LayerMask carsLayer;

    private float intervalToSpawn = 5f;

    private RoadManager roadManager;

    private void Start()
    {
        roadManager = GameManager.instance.roadManager;
        StartCoroutine(SpawnCars());

    }


    private IEnumerator SpawnCars() 
    {
        float delayToSpawnCars = 2f;

        while (true)
        {
            yield return new WaitForSeconds(delayToSpawnCars);

            GameObject carToSpawn = carsToSpawn[Random.Range(0, carsToSpawn.Length)];
            Vector3 positionToSpawn;

            GameObject newCar;

            RoadSO currentRoad = roadManager.CurrentRoad;

            int lineIndex = Random.Range(0, currentRoad.lineXPositions.Length);

            positionToSpawn = new Vector3(currentRoad.lineXPositions[lineIndex], roadManager.LastYPosition, 0);

            if (CheckIfCanSpawnCar(positionToSpawn))
            {
                newCar = Instantiate(carToSpawn, positionToSpawn, Quaternion.identity);


                if (positionToSpawn.x > 0)
                {
                    //by movement
                }
                else
                {
                    //against the movement
                    newCar.transform.localEulerAngles = new Vector3(0, 0, 180f);
                }
            }
        }
    }

    private bool CheckIfCanSpawnCar(Vector3 positionToSpawn) 
    {
        if (Physics2D.Linecast(positionToSpawn - Vector3.up * intervalToSpawn, positionToSpawn + Vector3.up * intervalToSpawn, carsLayer))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
