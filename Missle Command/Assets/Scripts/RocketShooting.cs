using System.Collections.Generic;
using UnityEngine;

public class RocketShooting : MonoBehaviour
{
    //Tworzę Singletona
    public static RocketShooting Instance { get; private set; }

    [Header("General")]
    [Tooltip("Lista wyrzutni rakiet")]
    public List<RocketLauncher> rocketLaunchers;
    [Tooltip("pozostała amunicja z 3 wieżyczek")]
    public int currentAmmo;
    [Tooltip("Gotowy prefab rakiety")]
    public GameObject rocketPrefab;
    [Tooltip("Gotowy prefab krzyżyka")]
    public GameObject cross;

    private bool canShoot;
    private float speed = 4;
    private Vector2 worldPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool GetCanShoot()
    {
        return canShoot;
    }

    public void SetCanShoot(bool value)
    {
        canShoot = value;
    }

    void Update()
    {
        if (canShoot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RocketLauncher rocketLauncher = ClosestRocketLauncher();

                if (rocketLauncher.ableToShoot && !rocketLauncher.isDestroyed)
                {
                    Transform target = Instantiate(cross, worldPosition, Quaternion.identity, transform).transform;
                    GameObject rocket = Instantiate(rocketPrefab, rocketLauncher.transform.position, Quaternion.identity, transform);
                    rocketLauncher.GetComponent<RocketLauncher>().Shoot();
                    rocket.GetComponent<Flyable>().target = target;
                    rocket.GetComponent<Flyable>().speed = speed;
                    if (rocketLauncher == rocketLaunchers[1])
                        rocket.GetComponent<Flyable>().speed = speed * 1.5f;
                    target.GetComponent<Target>().rocket = rocket.transform;
                }
            }
        }

        currentAmmo = CurrentAmmo();
    }

    public int CurrentAmmo()
    {
        int value = 0;

        foreach (RocketLauncher rl in rocketLaunchers)
            if (!rl.isDestroyed)
                value += rl.currentAmmunition;

        return value;
    }

    public void ApplyRocketLauncherTheme(Theme theme)
    {
        foreach (RocketLauncher rl in rocketLaunchers)
            rl.GetComponent<SpriteRenderer>().color = theme.rocketLaunchers;
    }

    //Funkcja odnawiająca amunicję we wszystkich wieżyczkach
    public void RenewAmmo()
    {
        for (int i = 0; i < rocketLaunchers.Count; i++)
        {
            rocketLaunchers[i].RenewAmmo();
        }
    }

    //Funkcja znajduję najbliższą wyrzutnię względem targetu
    public RocketLauncher ClosestRocketLauncher()
    {
        Transform tMin = rocketLaunchers[1].transform;
        float minDist = Mathf.Infinity;
        foreach (RocketLauncher t in rocketLaunchers)
        {
            if (t.ableToShoot && !t.isDestroyed)
            {
                float dist = Vector2.Distance(t.transform.position, worldPosition);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
        }

        return tMin.GetComponent<RocketLauncher>();
    }
}
