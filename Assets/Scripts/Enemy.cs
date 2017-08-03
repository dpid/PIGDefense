﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class Enemy : MonoBehaviour {

    public int health = 1;
    public int attackStrength = 1;

    private int initialHealth;
    private PathFollower pathFollower;
    private TransformRestorer transformRestorer;
    private Castle targetCastle;

	void Awake () 
    {
        InitHealth();
        InitPathFollower();
        InitTransformRestorer();
    }

    private void InitHealth()
    {
        initialHealth = health;    
    }

    private void InitPathFollower()
    {
        pathFollower = GetComponent<PathFollower>();
    }

    private void InitTransformRestorer()
    {
        transformRestorer = gameObject.GetComponent<TransformRestorer>();
        if (transformRestorer == null)
        {
            transformRestorer = gameObject.AddComponent<TransformRestorer>();
        }		
    }

    private void OnTriggerEnter(Collider other)
    {
        if (health > 0)
        {
			Castle castle = other.GetComponentInParent<Castle>();
			if (castle != null)
			{
				StartAttack(castle);
			}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopAttack();
    }

    private void OnDisable()
    {
        StopAttack();
        Invoke("RestoreTransforms", 0.1f);
    }

    private void RestoreTransforms()
    {
        transformRestorer.Restore();
    }

    private void StartAttack(Castle castle)
    {
		targetCastle = castle;
		StopAttack();
		InvokeRepeating("Attack", 1.0f, 1.0f);
    }

    private void StopAttack()
    {
        CancelInvoke("Attack");
    }

    private void Attack()
    {
        targetCastle.TakeDamage(attackStrength);
    }

    public void RestartPath()
    {
        Reset();
        pathFollower.RestartPath();
    }

	public void StartPath(Transform path)
	{
        Reset();
		pathFollower.StartPath(path);
	}

    private void Reset()
    {
        health = initialHealth;
        pathFollower.enabled = true;
		transformRestorer.Restore();
		SetRigidbodiesIsKinematic(true);
		StopAttack();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        health = 0;
        pathFollower.enabled = false;
        SetRigidbodiesIsKinematic(false);
        DetachRigidbodies();
        StopAttack();
    }

	private void SetRigidbodiesIsKinematic(bool isKinematic)
	{
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in rigidbodies)
		{
			rigidbody.isKinematic = isKinematic;
		}
	}

    private void DetachRigidbodies()
    {
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in rigidbodies)
		{
            rigidbody.transform.parent = null;
		}
    }


}
