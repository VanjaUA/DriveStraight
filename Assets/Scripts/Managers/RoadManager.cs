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

    [SerializeField] private RoadSO[] roads;
    [SerializeField] private MinMax<int> intervalToChangeRoad;

    [SerializeField] private float startYPosition;

    [SerializeField] private float intervalBetweenLines;

    private Player player;

    public float LastYPosition { get; private set; }
    public RoadSO CurrentRoad { get; private set; }

    private List<GameObject> roadsOnScene = new List<GameObject>();


    private int amountToChangeRoad;


    private void Start()
    {
        player = GameManager.instance.player;

        CurrentRoad = roads[0];

        amountToChangeRoad = Random.Range(intervalToChangeRoad.min, intervalToChangeRoad.max + 1);

        LastYPosition = startYPosition;
        SpawnRoad(CurrentRoad.roadPrefab, LastYPosition);
    }

    private void Update()
    {
        if (LastYPosition - player.transform.position.y < INTERVAL_BETWEEN_ROADS * 2)
        {
            SpawnRoad(CurrentRoad.roadPrefab, LastYPosition);
            DeleteRoad();
        }
    }

    private void DeleteRoad()
    {
        foreach (var road in roadsOnScene)
        {
            if (player.transform.position.y - road.transform.position.y > INTERVAL_BETWEEN_ROADS * 2)
            {
                roadsOnScene.Remove(road);
                Destroy(road);
                return;
            }
        }
    }

    private void SpawnRoad(GameObject road, float yPosition)
    {
        roadsOnScene.Add(Instantiate(road, new Vector3(0, yPosition, 0), Quaternion.identity, this.transform));
        LastYPosition += INTERVAL_BETWEEN_ROADS;

        amountToChangeRoad--;
        if (amountToChangeRoad <= 0)
        {
            ChangeRoadType();
        }
    }

    private void ChangeRoadType()
    {
        switch (CurrentRoad.roadCode)
        {
            case RoadCode.Road2x2To3x3:
                amountToChangeRoad = Random.Range(intervalToChangeRoad.min, intervalToChangeRoad.max + 1);
                break;

            case RoadCode.Road3x3To2x2:
                amountToChangeRoad = Random.Range(intervalToChangeRoad.min, intervalToChangeRoad.max + 1);
                break;
        }

        if ((int)CurrentRoad.roadCode + 1 >= roads.Length)
        {
            CurrentRoad = roads[0];
        }
        else
        {
            CurrentRoad = roads[(int)CurrentRoad.roadCode + 1];
        }
    }


    public float GetIntervalBetweenLines() 
    {
        return intervalBetweenLines;
    }

}