using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    //Funkcja licząca istniejące budynki
    public int CountRemainingBuildings()
    {
        int value = 0;
        LevelGenerator level = LevelGenerator.Instance;

        for (int i = 0; i < level.buildings.Count; i++)
        {
            if (level.buildings[i].isDestroyed == false) value++;
        }

        return value;
    }

    //Funkcja licząca pozostałą amunicję
    public int CountRemainingAmmunition()
    {
        int value = 0;
        RocketShooting shooting = RocketShooting.Instance;

        for (int i = 0; i < shooting.rocketLaunchers.Count; i++)
        {
            if (shooting.rocketLaunchers[i].ableToShoot && !shooting.rocketLaunchers[i].isDestroyed)
                value += shooting.rocketLaunchers[i].currentAmmunition;
        }

        return value;
    }
}
