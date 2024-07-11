using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Flyable
{
    [Header("General")]
    [Tooltip("Missile prefab")]
    public GameObject missilePrefab;

    private Vector3 pos;
    private Coroutine coroutine;

    public void Start()
    {
        pos = new Vector3(-transform.position.x, transform.position.y, 0);
        coroutine = StartCoroutine(WaitForThrow(LevelGenerator.Instance.GetPlaneSpeed()));
    }

    public void Update()
    {
        Vector3 heading = pos - transform.position;
        Vector3 direction = heading;
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    public void ThrowMissile()
    {
        if (GetComponent<Plane>()) 
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, missilePrefab.transform.rotation, transform.parent);
            LevelGenerator.Instance.enemyMissiles.Add(missile.GetComponent<Flyable>());
            missile.GetComponent<EnemyRocket>().isTripled = LevelGenerator.Instance.IsTripled();
            missile.GetComponent<Flyable>().target = LevelGenerator.Instance.RandomBuilding();
            missile.GetComponent<Flyable>().speed = LevelGenerator.Instance.GetMissileSpeed() * 1.5f;
        }
    }

    public void OnPlaneDestroyed()
    {
        StopCoroutine(coroutine);
        Destroy(gameObject);
    }

    public IEnumerator WaitForThrow(float speed)
    {
        yield return new WaitForSeconds(Random.Range(5f, 10f) / speed);
        ThrowMissile();
    }

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}