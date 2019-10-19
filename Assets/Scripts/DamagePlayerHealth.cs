using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerHealth : MonoBehaviour
{
    public float damage;
    public PlayerStats playerStats;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStats.UpdateHealth(-damage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exit");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStats.UpdateHealth(-damage * Time.deltaTime);
        }
    }
}
