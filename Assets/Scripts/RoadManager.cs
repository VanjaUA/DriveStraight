using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public enum RoadCode
    {
        Road2x2,
        Road2x2To3x3,
        Road3x3,
        Road3x3To2x2,
    }

    private const float INTERVAL_BETWEEN_ROADS = 17f;
    private const int INTERVAL_TO_CHANGE_ROAD_MIN = 5;
    private const int INTERVAL_TO_CHANGE_ROAD_MAX = 8;
    [SerializeField]
    private GameObject[] roadPrefabs;
    [SerializeField]
    private float startYPosition;

    private Player player;

    public float LastYPosition { get; private set; }
    public RoadCode CurrentRoad { get; private set; }
    private int amountToChangeRoad;

    private List<GameObject> roads = new List<GameObject>();

    private void Start()
    {
        player = GameManager.instance.player;

        CurrentRoad = RoadCode.Road2x2;
        amountToChangeRoad = Random.RandomRange(INTERVAL_TO_CHANGE_ROAD_MIN, INTERVAL_TO_CHANGE_ROAD_MAX);

        LastYPosition = startYPosition;
        SpawnRoad(roadPrefabs[(int)CurrentRoad], LastYPosition);
    }

    private void Update()
    {
        if (LastYPosition - player.transform.position.y < INTERVAL_BETWEEN_ROADS * 2)
        {
            SpawnRoad(roadPrefabs[(int)CurrentRoad],LastYPosition);
            DeleteRoad();
        }
    }

    private void DeleteRoad() 
    {
        foreach (var road in roads)
        {
            if (player.transform.position.y - road.transform.position.y > INTERVAL_BETWEEN_ROADS * 2)
            {
                roads.Remove(road);
                Destroy(road);
                return;
            }
        }
    }

    private void SpawnRoad(GameObject road,float yPosition) 
    {
        roads.Add(Instantiate(road, new Vector3(0, yPosition, 0), Quaternion.identity, this.transform));
        LastYPosition += INTERVAL_BETWEEN_ROADS;

        amountToChangeRoad--;
        if (amountToChangeRoad <= 0)
        {
            ChangeRoadType();
        }
    }

    private void ChangeRoadType() 
    {
        switch (CurrentRoad)
        {
            case RoadCode.Road2x2:
                CurrentRoad = RoadCode.Road2x2To3x3;
                break;
            case RoadCode.Road2x2To3x3:
                CurrentRoad = RoadCode.Road3x3;
                amountToChangeRoad = Random.RandomRange(INTERVAL_TO_CHANGE_ROAD_MIN, INTERVAL_TO_CHANGE_ROAD_MAX);
                break;
            case RoadCode.Road3x3:
                CurrentRoad = RoadCode.Road3x3To2x2;
                break;
            case RoadCode.Road3x3To2x2:
                CurrentRoad = RoadCode.Road2x2;
                amountToChangeRoad = Random.RandomRange(INTERVAL_TO_CHANGE_ROAD_MIN, INTERVAL_TO_CHANGE_ROAD_MAX);
                break;
            default:
                break;
        }
    }
}
