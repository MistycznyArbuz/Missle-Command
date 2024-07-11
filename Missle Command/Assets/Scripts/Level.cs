using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public int index;
    public int enemyMissiles;
    public float missilesSpeed;
    public int enemyPlanes;
    public float planesSpeed;
    public float chanceOfTripleMissile;

    public static bool SpecialEvent(float chance)
    {
        if (Random.value < chance)
            return true;

        return false;
    }
}
