using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public enum ObjectType
    {
        Coin,
        Fuel,
    }

    [SerializeField] private ObjectType objectType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player;
        if (collision.TryGetComponent<Player>(out player))
        {
            player.PickObject(objectType);

            gameObject.SetActive(false);
        }
    }
}
