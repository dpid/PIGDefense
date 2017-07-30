using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
}
