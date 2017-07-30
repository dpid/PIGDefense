﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform path;

    public int health = 1;
    public float speed = 1.0f;
    public float turnSpeed = 1.0f;
    public float hopStrength = 0.05f;
    public int attackStrength = 1;

    private int pathNodeIndex = -1;
    private Transform pathNode;

    private float hopDamper = 0.1f;
	private float hopOffset = 0.0f;
	private float hopPositionOffset = 0.0f;

    private Vector3 lastMovePosition;

    private Castle targetCastle;

	void Start () 
    {
        SetRigidbodiesIsKinematic(true);
        SetNextPathNode();
	}
	
	void Update () {
        if (health > 0)
        {
			if (pathNode != null)
			{
				Turn();
				Move();
			}

			Hop();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        Castle castle = other.GetComponentInParent<Castle>();
        if (castle != null)
        {
            StartAttack(castle);
        }
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


    private void SetNextPathNode()
    {
        pathNodeIndex += 1;

        if (pathNodeIndex < path.childCount)
        {
            pathNode = path.GetChild(pathNodeIndex);
        }
        else
        {
            pathNode = null;
        }
    }

    private void Turn()
    {
		float turnStep = turnSpeed * Time.deltaTime;
		Vector3 direction = pathNode.position - transform.localPosition;
        direction.y = 0;
		Quaternion targetRotation = Quaternion.LookRotation(direction);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, turnStep);
    }

    private void Move()
    {

		Vector3 transformPosition = transform.position;
		transformPosition.y = 0;

		Vector3 pathNodePosition = pathNode.position;
		pathNodePosition.y = 0;

		float step = speed * Time.deltaTime;
		Vector3 position = Vector3.MoveTowards(transformPosition, pathNodePosition, step);


        Vector3 raycastPosition = position + Vector3.up;
		RaycastHit hit;

		if (Physics.Raycast(raycastPosition, Vector3.down, out hit))
        {
            position.y = hit.point.y;
        }

        transform.position = position;
        lastMovePosition = position;

		float distance = Vector3.Distance(transformPosition, pathNodePosition);
		if (distance <= step)
		{
			SetNextPathNode();
		}
    }


    private void Hop()
    {
        if (hopPositionOffset <= 0.0f)
        {
            hopOffset = hopStrength;
        }
        else
        {
            hopOffset -= hopStrength * hopDamper;
		}

        hopPositionOffset += hopOffset;

        Vector3 hopPosition = lastMovePosition;
        hopPosition.y += hopPositionOffset;

        transform.position = hopPosition;
    }

    public void Die()
    {
        health = 0;
        SetRigidbodiesIsKinematic(false);
        DetachRigidbodies();
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
