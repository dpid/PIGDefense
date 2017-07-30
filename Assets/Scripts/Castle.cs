using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int health = 10;
    public Flagpole flagpole;

    private int initialHealth;

    private void Start()
    {
        initialHealth = health;
        SetFlagpoleRaisedPercentage();
    }

    public void TakeDamage(int damage = 1)
    {
        if (health > 0)
        {
			health -= damage;

			if (health <= 0)
			{
				Die();
			}

            SetFlagpoleRaisedPercentage();
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

    public void SetFlagpoleRaisedPercentage()
    {
        float percentage = health * 1.0f / initialHealth;
        flagpole.SetFlagRaisedPercentage(percentage);
    }
	
}
