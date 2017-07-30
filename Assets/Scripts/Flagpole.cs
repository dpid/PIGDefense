using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flagpole : MonoBehaviour {

    public float flagSpeed = 0.1f;

    public Transform flag;
    public Transform poleTop;
    public Transform poleBottom;

    private Vector3 targetFlagPosition;

    private void Start()
    {
        targetFlagPosition = flag.position;
    }

	private void Update()
	{
        if (targetFlagPosition != flag.position)
        {
            float step = flagSpeed * Time.deltaTime;
            flag.position = Vector3.Lerp(flag.position, targetFlagPosition, step);
        }
	}


	public void SetFlagRaisedPercentage(float percentage)
    {
        targetFlagPosition = Vector3.Lerp(poleBottom.position, poleTop.position, percentage);
    }


}
