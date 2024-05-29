using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MortarShell : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float explosionRadius = 8f;
    public int damage = 10;
    public static event Action OnHitEnemy;
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Explode();
            // Print distance from player 
            Debug.Log(Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position));
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosion, 1.5f);
        }
    }

    private void Explode()
    {
        // Find all objects with "Enemy" tag in radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                OnHitEnemy?.Invoke();
                Destroy(hitCollider.gameObject, 0.2f);
            }
        }
    }
}
