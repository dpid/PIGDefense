using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Castle : MonoBehaviour
{
    public int health = 10;
    public Flagpole flagpole;

    public float deathDelaySeconds;
    public UnityEvent OnDeath;

    public bool isDead
    {
        get
        {
            return health <= 0;    
        }
    }

    private int initialHealth;
    private TransformRestorer transformRestorer;

    private void Start()
    {
        InitHealth();
        InitTransformRestorer();
    }

	private void InitTransformRestorer()
	{
		transformRestorer = gameObject.AddComponent<TransformRestorer>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			child.gameObject.AddComponent<TransformRestorer>();
		}
	}

    private void SetIsKinematic(bool isKinemenatic)
    {
        Rigidbody[] rigidbodyChildren = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody childRigidbody in rigidbodyChildren)
        {
            childRigidbody.isKinematic = isKinemenatic;
        }
    }

    private void InitHealth()
    {
		initialHealth = health;
        SetHealth(health);
    }

    private void AddHealth(int value)
    {
        health += value;
        SetFlagpoleRaisedPercentage();
    }

    private void SetHealth(int value)
    {
        health = value;
        SetFlagpoleRaisedPercentage();
    }

	private void SetFlagpoleRaisedPercentage()
	{
		float percentage = health * 1.0f / initialHealth;
		flagpole.SetFlagRaisedPercentage(percentage);
	}

    public void Restore()
    {
        SetIsKinematic(false);
        transformRestorer.Restore();
        SetIsKinematic(true);

        SetHealth(initialHealth);
        flagpole.SetFlagToTop();
        flagpole.enabled = true;
    }

    public void TakeDamage(int damage = 1)
    {
        if (health > 0)
        {
            AddHealth(-damage);

			if (health <= 0)
			{
				Die();
			}
        }
    }

    public void Die()
    {
        health = 0;

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody childRigidbody in rigidbodies){
            childRigidbody.isKinematic = false;
            childRigidbody.AddExplosionForce(5.0f, transform.position, 5.0f, 1.0f, ForceMode.Impulse);
        }

        flagpole.SetFlagToBottom();
        flagpole.enabled = false;

        Invoke("CallDeathEvent", deathDelaySeconds);
    }

    private void CallDeathEvent()
    {
        OnDeath.Invoke();
    }
	
}
