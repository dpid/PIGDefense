using System.Collections;
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

    private float lifetime = 5.0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        initialScale = transform.localScale;

        explosion = Instantiate(explosionPrefab) as ParticleSystem;
        explosion.Stop();
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        Invoke("Disable", lifetime); 
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Disable();
        MakeExplosion();    
    }

    private void Disable()
    {
        gameObject.SetActive(false);
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
