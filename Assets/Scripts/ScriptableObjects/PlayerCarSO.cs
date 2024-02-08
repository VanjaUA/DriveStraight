using UnityEngine;

[CreateAssetMenu()]
public class PlayerCarSO : ScriptableObject
{
    public Sprite sprite;
    public int cost;
    public bool boughtByPlayer;
    public GameObject carObject;
}
