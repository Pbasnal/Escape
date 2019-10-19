using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;
    public float fallGravity;
    public Vector3 velocity;

    private new Rigidbody2D rigidbody;
    private bool hasJumped = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        rigidbody.velocity = new Vector3(h * moveSpeed, rigidbody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            hasJumped = !hasJumped;
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
        else
        {
            hasJumped = true;
        }

        velocity = rigidbody.velocity;
    }

    private void FixedUpdate()
    {
    }
}


