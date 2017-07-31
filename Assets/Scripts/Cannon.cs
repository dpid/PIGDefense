﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public CannonballPool cannonballPool;
    public float strength;

    public void Fire()
    {
        if (isActiveAndEnabled)
        {
			Cannonball cannonball = cannonballPool.GetCannonball();

			if (cannonball != null)
			{
				cannonball.gameObject.SetActive(true);
				cannonball.transform.position = transform.position;
				cannonball.transform.localRotation = Quaternion.identity;

				Vector3 force = transform.forward * strength;
				Rigidbody cannonballRigidbody = cannonball.GetComponent<Rigidbody>();
				cannonballRigidbody.AddForce(force, ForceMode.Impulse);
			}
        }
    }
}
