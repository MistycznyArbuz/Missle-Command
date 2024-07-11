using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Flyable : MonoBehaviour
{
    public enum Type { player, enemy, plane}
    public Type type;

    public Transform target;
    public ParticleSystem explosion;

    public float speed;

    public void Movement()
    {
        if (target != null)
        {
            Vector3 heading = target.position - transform.position;
            Vector3 direction = heading;
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }
    }

    public void DestroyOnCollision(Collider2D collision)
    {
        Instantiate(explosion, collision.transform.position, Quaternion.identity, transform.parent);
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    public void DestroyItself(Collider2D collision)
    {
        Instantiate(explosion, collision.transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    public void DestroyOnExplosion(Collider2D collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    public bool IsPlane(Collider2D collision)
    {
        if (collision.GetComponent<Plane>())
            if (collision.GetComponent<Plane>().type == Type.plane) return true;
        return false;
    }
}