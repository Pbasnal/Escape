using UnityEngine;
using System.Collections;

public class GiveGoldToPlayer : MonoBehaviour
{
    public int goldAmount;
    public PlayerStats playerStats;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerStats.UpdateGold(goldAmount);
            Destroy(gameObject);
        }
    }
}
