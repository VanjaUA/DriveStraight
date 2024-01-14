using UnityEngine;

[CreateAssetMenu()]
public class RoadSO : ScriptableObject
{
    public RoadManager.RoadCode roadCode;
    public GameObject roadPrefab;
    public float[] lineXPositions;
}
