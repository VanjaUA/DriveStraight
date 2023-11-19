using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    private const float DELETE_INTERVAL = 20f;

    [SerializeField]
    private float speed = 5f;

    private Player player;
    private Rigidbody2D rb2D;

    private void Start()
    {
        player = GameManager.instance.player;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Delete();
    }

    private void Move() 
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)(transform.position + transform.up), speed * Time.deltaTime);
    }
    private void Delete() 
    {
        if (transform.position.y + DELETE_INTERVAL < player.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
