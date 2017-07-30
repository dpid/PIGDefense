using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int health = 10;


    public void TakeDamage(int damage = 1)
    {
        if (health > 0)
        {
			health -= damage;
			if (health <= 0)
			{
				Die();
			}
        }
    }

    [ContextMenu("Die")]
    public void Die()
    {
        health = 0;

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody childRigidbody in rigidbodies){
            childRigidbody.isKinematic = false;
            childRigidbody.AddExplosionForce(5.0f, transform.position, 5.0f, 1.0f, ForceMode.Impulse);
        }
    }
	
}
