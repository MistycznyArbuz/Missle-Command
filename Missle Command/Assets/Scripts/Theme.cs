using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Theme", order = 1)]
public class Theme : ScriptableObject
{
    public Color player;
    public Color enemy;
    public Color buildings;
    public Color rocketLaunchers;
    public Color ground;
    public Color background;
}
