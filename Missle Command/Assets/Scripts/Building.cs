using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public SpriteRenderer sprite;
    public ParticleSystem explosion;
    public bool isDestroyed;

    public void DestroyBuilding()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation, transform.parent);
        sprite.enabled = false;
        isDestroyed = true;
    }

    public void RepairBuilding()
    {
        isDestroyed = false;
        sprite.enabled = true;
    }
}
