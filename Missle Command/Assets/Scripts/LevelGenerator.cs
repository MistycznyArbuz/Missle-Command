using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //Tworzenie singletona obiektu - może istnieć tylko jedna instancja Level Generatora
    public static LevelGenerator Instance { get; private set; }

    [Header("General")]
    [Tooltip("Gotowe motywy poziomu")]
    public List<Theme> themes;
    [Tooltip("Prefab pocisku gracza")]
    public GameObject playerMissilePrefab;
    [Tooltip("Prefab pocisku wroga")]
    public GameObject missilePrefab;
    [Tooltip("Prefab samolotu")]
    public GameObject planePrefab;
    [Tooltip("Lista budynków")]
    public List<Building> buildings;
    [Tooltip("Lista wyrzutni rakiet")]
    public List<Building> rocketLaunchers;
    [Tooltip("Lista pocisków wroga")]
    public List<Flyable> enemyMissiles;
    [Tooltip("Lista samolotów wroga")]
    public List<Flyable> planes;
    [Tooltip("Lista poziomów")]
    public List<Level> levels;
    [Tooltip("Lista celów")]
    public List<Building> targets;
    [Tooltip("Podłoże i tło")]
    public SpriteRenderer ground, background;

    private int level;

    //Co ile punktów gra ma regenerować budynki
    private int targetScore = 5000;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Start()
    {
        ApplyTheme(RandomTheme());
        NextLevel();
    }

    public void ApplyTheme(Theme theme)
    {
        playerMissilePrefab.GetComponent<SpriteRenderer>().color = theme.player;
        playerMissilePrefab.GetComponent<TrailRenderer>().startColor = theme.player;
        missilePrefab.GetComponent<SpriteRenderer>().color = theme.enemy;
        missilePrefab.GetComponent<TrailRenderer>().startColor = theme.enemy;

        for (int i = 0; i < buildings.Count; i++)
            buildings[i].GetComponent<SpriteRenderer>().color = theme.buildings;

        for (int i = 0; i < rocketLaunchers.Count; i++)
            rocketLaunchers[i].GetComponent<SpriteRenderer>().color = theme.rocketLaunchers;

        ground.color = theme.ground;
        background.color = theme.background;
    }

    public Theme RandomTheme()
    {
        return themes[Random.Range(0, themes.Count)];
    }

    public float GetPlaneSpeed()
    {
        return levels[level].planesSpeed;
    }

    public float GetMissileSpeed()
    {
        return levels[level].missilesSpeed;
    }

    public void CountMissiles()
    {
        for (int i = 0; i < enemyMissiles.Count; i++)
            if (enemyMissiles[i] == null) enemyMissiles.RemoveAt(i);
    }

    public void CountPlanes()
    {
        for (int i = 0; i < planes.Count; i++)
            if (planes[i] == null) planes.RemoveAt(i);
    }

    public void Update()
    {
        CountMissiles();
        CountPlanes();

        if (enemyMissiles.Count <= 0 && planes.Count <= 0)
        {
            RocketShooting.Instance.SetCanShoot(false);
            GameManager.Instance.LevelEnd();
            UIManager.Instance.LevelEndSwitch(true);
        }
        else
        {
            GameManager.Instance.levelEnd = false;
            UIManager.Instance.LevelEndSwitch(false);
            RocketShooting.Instance.SetCanShoot(true);
        }

        if (IsAllBuildingsDestroyed() || IsAllRocketLaunchersDestroyed())
            GameManager.Instance.GameOver();

        if (RocketShooting.Instance.currentAmmo <= 0 || GameManager.Instance.gameOver)
            Time.timeScale = 2;
        else if (GameManager.Instance.levelEnd)
            Time.timeScale = 1;
        else
            Time.timeScale = 1;
    }

    public void SpawnMissiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 position = RandomPosition(-8, 8, 5.5f, 15);
            GameObject missile = Instantiate(missilePrefab, position, missilePrefab.transform.rotation, transform);
            missile.GetComponent<Flyable>().target = RandomBuilding();
            missile.GetComponent<Flyable>().speed = levels[level].missilesSpeed;
            missile.GetComponent<EnemyRocket>().isTripled = IsTripled();
            enemyMissiles.Add(missile.GetComponent<Flyable>());
        }
    }

    public bool IsTripled()
    {
        return Level.SpecialEvent(levels[level].chanceOfTripleMissile);
    }

    public void SpawnPlanes(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 position = RandomPosition(10, 15, 2, 4);
            Plane plane = Instantiate(planePrefab, position, planePrefab.transform.rotation, transform).GetComponent<Plane>();
            plane.speed = levels[level].planesSpeed;
            planes.Add(plane.GetComponent<Flyable>());
        }
    }

    public Transform RandomBuilding()
    {
        return targets[Random.Range(0, targets.Count)].transform;
    }

    public Vector2 RandomPosition(float minX, float maxX, float minY, float maxY)
    {
        return transform.TransformVector(new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)));
    }

    public void RebuildBuildings()
    {
        foreach (Building b in buildings)
            if (b.GetComponent<Building>().isDestroyed)
                b.GetComponent<Building>().RepairBuilding();
    }

    public bool IsAllBuildingsDestroyed()
    {
        int value = 0;

        for (int i = 0; i < buildings.Count; i++)
        {
            if (!buildings[i].isDestroyed) value++;
        }

        if (value == 0) return true;
        else return false;
    }

    public bool IsAllRocketLaunchersDestroyed()
    {
        int value = 0;

        for (int i = 0; i < rocketLaunchers.Count; i++)
            if (!rocketLaunchers[i].isDestroyed) value++;

        if (value == 0) return true;
        else return false;
    }

    public Transform PrevoriusTarget(Transform currentTarget)
    {
        int currentIndex = targets.IndexOf(currentTarget.GetComponent<Building>());

        for (int i = 0; i < targets.Count; i++)
        {
            if (currentIndex == i)
            {
                if (i - 1 > -1)
                    return targets[i - 1].transform;
                else
                    return targets[targets.Count - 1].transform;
            }
        }

        return null;
    }

    public Transform NextTarget(Transform currentTarget)
    {
        int currentIndex = targets.IndexOf(currentTarget.GetComponent<Building>());

        for (int i = 0; i < targets.Count; i++)
        {
            if (currentIndex == i)
            {
                if (i + 1 < targets.Count)
                    return targets[i + 1].transform;
                else
                    return targets[0].transform;
            }
        }

        return null;
    }

    public void NextLevel()
    {
        RocketShooting.Instance.RenewAmmo();
        SpawnPlanes(levels[level].enemyPlanes);
        SpawnMissiles(levels[level].enemyMissiles);
        level++;

        if (ScoreManager.Instance.Score >= targetScore)
        {
            RebuildBuildings();
            targetScore += 5000;
        }
    }
}