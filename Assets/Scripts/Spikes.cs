using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spikes : MonoBehaviour
{
    public int damage = 1;
    public static event Action<int> OnDamage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnDamage?.Invoke(damage);
        }
    }
}
