using UnityEngine;

public class CatLogic : MonoBehaviour
{
    private const string DIE_TRIGGER_NAME = "Died";

    [SerializeField] private float moveSpeed;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector3 moveDirection;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (spriteRenderer.flipX == true)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
        }
    }

    private void Update()
    {
        Move(moveDirection,moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger(DIE_TRIGGER_NAME);
        Destroy(this);
    }

    private void Move(Vector3 moveDirection,float speed) 
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
