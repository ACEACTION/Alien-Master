using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour 
{
    [SerializeField] float alertRadius;


    public Transform t;

    public static Alert Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            //AlertToNearEnemies(t.position);
            EnemyAlertPatrolling(EnemyManager.enemiesList[0]);

        if (Input.GetKeyDown(KeyCode.F))
        {
            List<Enemy> el = EnemyManager.enemiesList;
            
            foreach (Enemy e in EnemyManager.enemiesList)
            {
                print(e.name);
            }
        }
    }

    public void AlertToNearEnemies(Vector3 alertPos)
    {
        List<Enemy> nearEnemies = CheckNearEnemies(alertPos);
        for (int i = 0; i < nearEnemies.Count; i++)
        {
            nearEnemies[i].SetAlert(alertPos);
        }
    }

    public void EnemyAlertToNearEnemies(Vector3 alertPos, Enemy alertSender)
    {
        List<Enemy> nearEnemies = CheckNearEnemies(alertPos);

        foreach (Enemy e in nearEnemies)
        {
            if (e == alertSender) continue;
            e.SetAlert(alertPos);
        }

    }

    List<Enemy> CheckNearEnemies(Vector3 alertPos)
    {
        List<Enemy> nearestEnemies = new List<Enemy>();
        //for (int i = 0; i < nearestEnemies.Count; i++)
        //{
        //    if (Vector3.Distance(nearestEnemies[i].transform.position, alertPos) > alertRadius)
        //    {
        //        nearestEnemies.RemoveAt(i);
        //    }
        //}
        foreach (Enemy e in EnemyManager.enemiesList)
        {
            if (Vector3.Distance(e.transform.position, alertPos) < alertRadius)
                nearestEnemies.Add(e);
        }

        return nearestEnemies;
    }

    public void EnemyAlertPatrolling(Enemy alertSender)
    {
        foreach (Enemy e in EnemyManager.enemiesList)
        {
            if (e == alertSender) continue;
            e.SetAlertPatrollingState();
        }
    }

}
