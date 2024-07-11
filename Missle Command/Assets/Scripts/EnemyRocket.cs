using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocket : Flyable
{
    public AudioSource tripledSound;
    public bool isTripled, exploded;

    public Coroutine coroutine;

    public void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        if (exploded)
        {
            StopCoroutine(coroutine);
        }
    }

    public void OnBecameVisible()
    {
        if (isTripled)
        {
            coroutine = StartCoroutine(WaitForExplosion());
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            DestroyItself(collision);
            ScoreManager.Instance.Score += 25;
        }
        else if (collision.CompareTag("Building"))
            DestroyBuilding(collision);
    }

    public void DestroyBuilding(Collider2D collision)
    {
        collision.GetComponent<Building>().DestroyBuilding();
        Destroy(gameObject);
    }

    public IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(Random.Range(3, 6));
        LevelGenerator levelGen = LevelGenerator.Instance;

        tripledSound.PlayOneShot(tripledSound.clip);

        EnemyRocket prevRocket = Instantiate(levelGen.missilePrefab, transform.position, LevelGenerator.Instance.missilePrefab.transform.rotation, transform.parent).GetComponent<EnemyRocket>();
        prevRocket.isTripled = false;
        prevRocket.target = levelGen.PrevoriusTarget(target);
        prevRocket.GetComponent<Flyable>().speed = levelGen.GetMissileSpeed();
        levelGen.enemyMissiles.Add(prevRocket.GetComponent<Flyable>());

        EnemyRocket nextRocket = Instantiate(levelGen.missilePrefab, transform.position, LevelGenerator.Instance.missilePrefab.transform.rotation, transform.parent).GetComponent<EnemyRocket>();
        nextRocket.isTripled = false;
        nextRocket.target = levelGen.NextTarget(target);
        nextRocket.GetComponent<Flyable>().speed = levelGen.GetMissileSpeed();
        levelGen.enemyMissiles.Add(nextRocket.GetComponent<Flyable>());

        isTripled = false;
        exploded = true;
        yield break;
    }
}