using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRestorer : MonoBehaviour {

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    private Transform[] initialChildren;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
        initialChildren = new Transform[transform.childCount];
        for (int i = 0; i < initialChildren.Length; i++)
        {
            initialChildren[i] = transform.GetChild(i);
        }
    }

    public void Restore()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        if(initialChildren != null)
        {
			foreach (Transform child in initialChildren)
			{
				if (child != null && child != transform)
				{
					child.parent = transform;
					TransformRestorer childTransformRestorer = child.GetComponent<TransformRestorer>();
					if (childTransformRestorer != null)
					{
						childTransformRestorer.Restore();
					}
				}
			}
        }

    }

}
