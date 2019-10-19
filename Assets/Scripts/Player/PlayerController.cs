using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;
    public float fallGravity;
    public Vector3 velocity;
    public LayerMask groundLayer;
    public int extraJumps = 1;

    private float disstanceToTheGround;
    private new Rigidbody2D rigidbody;
    private int jumpCount = 0;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        disstanceToTheGround = GetComponent<BoxCollider2D>().bounds.extents.y;
    }

    private void Start()
    {
    }

    private void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        rigidbody.velocity = new Vector3(h * moveSpeed, rigidbody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || jumpCount < extraJumps))
        {
            jumpCount++;
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed);
        }

        if (rigidbody.velocity.y > 0)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - (gravity * Time.deltaTime));
        }
        else if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - (fallGravity * Time.deltaTime));
        }

        velocity = rigidbody.velocity;
        DebugCasts();
    }

    private void DebugCasts()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
    }

    private bool IsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, disstanceToTheGround + 0.1f);
        if (hit.collider == null)
        {
            jumpCount = 0;
        }

        return hit.collider != null;
    }


    private void FixedUpdate()
    {
    }
}


