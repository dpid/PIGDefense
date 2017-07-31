using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour {

    public Enemy enemyPrefab;
    public int enemyCount;
    public float intervalSeconds = 5.0f;

    public Transform[] paths;

    public UnityEvent OnEnemiesVanquished;

    public bool isAutoStarting = true;

    private Enemy[] enemyInstances;
    private int enemyInstanceIndex;

    private void Start()
    {
        InitInstances();
        HideEnemyInstances();

        if (isAutoStarting)
        {
            StartSpawning();
        }
    }

    private void InitInstances()
    {
        enemyInstances = new Enemy[enemyCount];
        for (int i = 0; i < enemyCount; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab) as Enemy;
            enemy.transform.parent = transform;
            enemy.transform.position = transform.position;
            enemyInstances[i] = enemy;
        }
    }

    private void HideEnemyInstances()
    {
        for (int i = 0; i < enemyInstances.Length; i++)
		{
            Enemy enemy = enemyInstances[i];
            enemy.gameObject.SetActive(false);
		}
	}

    public void StartSpawning()
    {
        HideEnemyInstances();
        enemyInstanceIndex = -1;

        StopSpawning();
        InvokeRepeating("Spawn", 1.0f, intervalSeconds);
        InvokeRepeating("CheckEnemiesVanquished", 1.0f, 1.0f);
    }

    public void StopSpawning()
    {
		CancelInvoke("Spawn");
		CancelInvoke("CheckEnemiesVanquished");
    }

    private void Spawn()
    {
        Transform path = GetRandomPath();
        if (path != null)
        {
            Enemy enemy = GetNextEnemeyInstance();
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
                enemy.health = 1;
                enemy.StartPath(path);
            }
        }
    }

    private Transform GetRandomPath()
    {
        Transform randomPath = null;

        if (paths.Length > 0)
        {
            int randomIndex = Random.Range(0, paths.Length);
            randomPath = paths[randomIndex];
        }

        return randomPath;
    }

    private Enemy GetNextEnemeyInstance()
    {
        Enemy enemy = null;

        enemyInstanceIndex += 1;

        if(enemyInstanceIndex < enemyInstances.Length)
        {
            enemy = enemyInstances[enemyInstanceIndex];
        }

        return enemy;
    }

    private void CheckEnemiesVanquished()
    {

        bool isAllVanquished = true;

        bool isEnemiesRemaining = enemyInstanceIndex < enemyInstances.Length -1;

        if (isEnemiesRemaining)
        {
            isAllVanquished = false;
        }
        else
        {
			foreach (Enemy enemy in enemyInstances)
			{
				bool isAlive = enemy.isActiveAndEnabled && enemy.health > 0;
				if (isAlive)
				{
					isAllVanquished = false;
				}
			}
        }

        if (isAllVanquished)
        {
            OnEnemiesVanquished.Invoke();    
        }
    }

}
