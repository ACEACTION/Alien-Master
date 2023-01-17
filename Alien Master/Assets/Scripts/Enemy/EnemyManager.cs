using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    public static List<Enemy> enemiesList = new List<Enemy>();
    public static List<GameObject> deadEnemies = new List<GameObject>();
    public static bool EnemiesListIsEmpty()
    {
        return enemiesList.Count == 0;
    }


}
