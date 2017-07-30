using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballPool : MonoBehaviour {

    public Cannonball cannonballPrefab;
    public int instanceCount = 10;

    private Cannonball[] cannonballs;

    private void Awake()
    {
        cannonballs = new Cannonball[instanceCount];

        for (int i = 0; i < instanceCount; i++)
        {
            Cannonball cannonball = Instantiate(cannonballPrefab) as Cannonball;
            cannonball.transform.parent = transform;
            cannonball.Deactivate();

            cannonballs[i] = cannonball;
        }
    }

    public Cannonball GetCannonball()
    {
        Cannonball foundCannonball = null;
        foreach(Cannonball cannonball in cannonballs)
        {
            if (cannonball.GetIsActive() == false)
            {
                foundCannonball = cannonball;
                break;
            }
        }

        return foundCannonball;
    }

}
