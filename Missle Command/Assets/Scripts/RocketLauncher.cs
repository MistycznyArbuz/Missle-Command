using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Building
{
    [Header("General")]
    [Tooltip("Audio clip of shooting")]
    public AudioSource shootSound;
    [Tooltip("Current ammunition of this launcher")]
    public int currentAmmunition;
    [Tooltip("Ammunition which player starts the game")]
    public int initAmmunition;
    [Tooltip("Is the launcher able to schoot ?")]
    public bool ableToShoot = true;

    void Start()
    {
        RenewAmmo();
    }

    public void Shoot()
    {
        shootSound.PlayOneShot(shootSound.clip);
        currentAmmunition--;
        if (currentAmmunition <= 0)
            ableToShoot = false;
    }

    public void RenewAmmo()
    {
        currentAmmunition = initAmmunition;
        ableToShoot = true;
    }
}
