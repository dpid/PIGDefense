﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cannonball : MonoBehaviour
{

    public ParticleSystem explosionPrefab;

    public float explosionRadius = 0.5f;
	public float explosionForce = 5.0f;
	public float explosionUpwardForce = 1.0f;

    private new Rigidbody rigidbody;
    private Vector3 initialScale;

    private ParticleSystem explosion;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        initialScale = transform.localScale;

        explosion = Instantiate(explosionPrefab) as ParticleSystem;
        explosion.Stop();
    }

    public void Activate()
    {
		gameObject.transform.localScale = initialScale;
        rigidbody.isKinematic = false;
    }

    public void Deactivate()
    {
		gameObject.transform.localScale = Vector3.zero;
		rigidbody.isKinematic = true;
    }

    public bool GetIsActive()
    {
        return rigidbody.isKinematic == false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        MakeExplosion();
        Deactivate();
    }

    private void MakeExplosion()
    {
        MakeParticleSystem();
        AddExplosionForce();
    }

    private void MakeParticleSystem()
    {
        if (explosion != null)
        {
            explosion.transform.position = transform.position;
            explosion.Play();
        }
    }

    private void AddExplosionForce()
	{

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			Enemy enemy = collider.gameObject.GetComponentInParent<Enemy>();
			if (enemy != null)
			{
                enemy.Die();
			}

			Rigidbody rigidBody = collider.GetComponentInParent<Rigidbody>();
			if (rigidBody != null)
			{
				rigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpwardForce, ForceMode.Impulse);
			}
		}
	}


}
