using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player;
        if (collision.TryGetComponent<Player>(out player))
        {
            gameObject.SetActive(false);
        }
    }
}
