using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    protected SpriteRenderer sr;
    private float moveH, moveV;

    protected float localScaleX;

    [SerializeField] private float moveSpeed;

    //OPTIONAL
    public float playerPosX, playerPosY;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        localScaleX = transform.localScale.x;
    }

    private void FixedUpdate()
    {
        moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveV = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = new Vector2(moveH, moveV);
        ChangeDirection();
    }

    protected virtual void ChangeDirection ()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal")*localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }

}
