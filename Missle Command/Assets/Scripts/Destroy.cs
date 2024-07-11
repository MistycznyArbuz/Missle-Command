using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float time;

    public void Start()
    {
        Destroy(gameObject, time);
    }
}
