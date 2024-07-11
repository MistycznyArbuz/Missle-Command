using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform rocket;

    public void Update()
    {
        if (rocket == null)
            Destroy(gameObject);
    }
}
