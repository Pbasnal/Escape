using UnityEngine;
using System.Collections;

public class Enemy_Basic : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask groundLayer;

    private int direction = 1;
    private new Rigidbody2D rigidbody;
    private float disstanceToTheGround;
    private Vector3 width;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        disstanceToTheGround = GetComponent<BoxCollider2D>().bounds.extents.y;
        width = new Vector3(0.5f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        IsBlocked();
        IsGrounded();
    }
    
    private bool IsBlocked()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.right * direction, disstanceToTheGround + 0.1f, groundLayer);
        if (hit.collider != null)
        {
            direction *= -1;
            return true;
        }

        return false;
    }

    private bool IsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position + width * direction, Vector2.down, disstanceToTheGround + 0.1f, groundLayer);
        if (hit.collider == null)
        {
            direction *= -1;
            return false;
        }

        return true;
    }

    private void Patrol()
    {
        rigidbody.velocity = new Vector3(moveSpeed * direction, rigidbody.velocity.y);
    }
}
