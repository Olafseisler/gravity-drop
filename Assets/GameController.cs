using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int maxHealth = 100;
    public int currentHealth;
    

    public int score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        Pickup.OnPickup += HandlePickup;
        Spikes.OnDamage += TakeDamage;
    }
    
    private void OnDisable()
    {
        Pickup.OnPickup -= HandlePickup;
        Spikes.OnDamage -= TakeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log("Player took " + damage + " damage!");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        Debug.Log("Player healed " + amount + " health!");
        
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    
    public void Die()
    {
        Debug.Log("Player died!");
    }
    
    private void HandlePickup(int scoreValue)
    {
        score += scoreValue;
        Debug.Log("Score: " + score);
    }
}
