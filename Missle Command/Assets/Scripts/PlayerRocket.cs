using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : Flyable
{
    public void FixedUpdate()
    {
        Movement();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlane(collision))
        {
            DestroyItself(collision);
            collision.GetComponent<Plane>().OnPlaneDestroyed();
            ScoreManager.Instance.Score += 100;
        }
        if (collision.CompareTag("Target"))
            DestroyItself(collision);
    }
}