using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Player/Stats", order = 52)]
public class PlayerStats : ScriptableObject
{
    public int maxHealth;
    public int maxStamina;
    public float health;
    public float stamina;

    public int gold = 0;

    public void Reset()
    {
        health = 100;
        stamina = 100;
        gold = 0;
    }

    public void UpdateHealth(float updateBy)
    {
        health = Mathf.Clamp(health + updateBy, 0, maxHealth);

        Debug.Log("health: " + health);
    }

    public void UpdateStamina(float updateBy)
    {
        stamina = Mathf.Clamp(stamina + updateBy, 0, maxStamina);
    }

    public void UpdateGold(int updateBy)
    {
        if (gold + updateBy < 0)
        {
            return;
        }

        gold += updateBy;
        Debug.Log("gold: " + gold);
    }
}
