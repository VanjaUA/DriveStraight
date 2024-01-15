using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [Header("Cracks")]
    [SerializeField] private GameObject[] cracksObjects;
    [SerializeField] private MinMax<float> timeToSpawnCracks;

    [Header("Coins")]
    [SerializeField] private GameObject coinObject;
    [SerializeField] private MinMax<float> timeToSpawnCoins;

    [Header("Fuel")]
    [SerializeField] private GameObject fuelObject;
    [SerializeField] private float timeToSpawnFuel;

    [Header("Cats")]
    [SerializeField] private GameObject catObject;
    [SerializeField] private float xIntervalToSpawnCats;
    [SerializeField] private MinMax<float> timeToSpawnCats;

    [Header("OtherObjects")]
    [SerializeField] private GameObject[] Road2x2Objects;
    [SerializeField] private GameObject[] Road3x3Objects;
    [SerializeField] private MinMax<float> timeToSpawnRoadObjects;

    [Header("DeleteSettings")]
    [SerializeField] private float deleteDistance;
    [SerializeField] private float deleteDelay;


    private List<GameObject> objectsOnScene = new List<GameObject>();

    private RoadManager roadManager;

    private void Start()
    {
        roadManager = GameManager.instance.roadManager;

        StartCoroutine(SpawnCracksCoroutine());
        StartCoroutine(SpawnCoinsCoroutine());
        StartCoroutine(SpawnFuelCoroutine());
        StartCoroutine(SpawnOtherObjectsCoroutine());
        StartCoroutine(SpawnCatsCoroutine());


        StartCoroutine(DeleteObjectsCoroutine());
    }

    private IEnumerator SpawnCracksCoroutine() 
    {
        while (true)
        {
            float delay = Random.Range(timeToSpawnCracks.min,timeToSpawnCracks.max);
            yield return new WaitForSeconds(delay);

            Vector3 positionToSpawn = new Vector3(Random.Range(roadManager.CurrentRoad.lineXPositions[0],
                roadManager.CurrentRoad.lineXPositions[roadManager.CurrentRoad.lineXPositions.Length - 1]),roadManager.LastYPosition);

            GameObject objectToSpawn = cracksObjects[Random.Range(0, cracksObjects.Length)];

            objectsOnScene.Add(Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity, this.transform));

        }
    }

    private IEnumerator SpawnCoinsCoroutine()
    {
        while (true)
        {
            float delay = Random.Range(timeToSpawnCoins.min, timeToSpawnCoins.max);
            yield return new WaitForSeconds(delay);

            Vector3 positionToSpawn = new Vector3(Random.Range(roadManager.CurrentRoad.lineXPositions[0],
                roadManager.CurrentRoad.lineXPositions[roadManager.CurrentRoad.lineXPositions.Length - 1]), roadManager.LastYPosition);

            GameObject objectToSpawn = coinObject;

            objectsOnScene.Add(Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity, this.transform));

        }
    }

    private IEnumerator SpawnFuelCoroutine()
    {
        while (true)
        {
            float delay = timeToSpawnFuel;
            yield return new WaitForSeconds(delay);

            Vector3 positionToSpawn = new Vector3(Random.Range(roadManager.CurrentRoad.lineXPositions[0],
                roadManager.CurrentRoad.lineXPositions[roadManager.CurrentRoad.lineXPositions.Length - 1]), roadManager.LastYPosition);

            GameObject objectToSpawn = fuelObject;

            objectsOnScene.Add(Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity, this.transform));

        }
    }

    private IEnumerator SpawnOtherObjectsCoroutine()
    {
        while (true)
        {
            float delay = Random.Range(timeToSpawnRoadObjects.min, timeToSpawnRoadObjects.max);
            yield return new WaitForSeconds(delay);

            Vector3 positionToSpawn = new Vector3(0, roadManager.LastYPosition);

            GameObject objectToSpawn = null;
            if (roadManager.CurrentRoad.roadCode == RoadManager.RoadCode.Road3x3)
            {
                objectToSpawn = Road3x3Objects[Random.Range(0, Road3x3Objects.Length)];
            }
            else if (roadManager.CurrentRoad.roadCode == RoadManager.RoadCode.Road2x2)
            {
                objectToSpawn = Road2x2Objects[Random.Range(0, Road2x2Objects.Length)];
            }

            if (objectToSpawn != null)
            {
                objectsOnScene.Add(Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity, this.transform));
            }


        }
    }

    private IEnumerator SpawnCatsCoroutine()
    {
        while (true)
        {
            float delay = Random.Range(timeToSpawnCats.min, timeToSpawnCats.max);
            yield return new WaitForSeconds(delay);

            Vector3 positionToSpawn;
            if (Random.Range(0, 2) == 0)
            {
                positionToSpawn = new Vector3(roadManager.CurrentRoad.lineXPositions[0] - xIntervalToSpawnCats, roadManager.LastYPosition);
            }
            else
            {
                positionToSpawn = new Vector3(roadManager.CurrentRoad.lineXPositions[roadManager.CurrentRoad.lineXPositions.Length - 1] + xIntervalToSpawnCats
                    , roadManager.LastYPosition);
            }


            GameObject objectToSpawn = catObject;

            objectsOnScene.Add(Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity, this.transform));

        }
    }

    private IEnumerator DeleteObjectsCoroutine() 
    {
        yield return new WaitForSeconds(deleteDelay);

        float playerYPosition = GameManager.instance.player.transform.position.y;

        for (int i = 0; i < objectsOnScene.Count; i++)
        {
            if (playerYPosition - objectsOnScene[i].transform.position.y > deleteDistance)
            {
                Destroy(objectsOnScene[i]);
                objectsOnScene.RemoveAt(i);
            }
        }
    }
}
