using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs = new ();

    private double timer = 0;

    private double timePerSpawn = 1;

    private double spawnTimeMultiplier = 0.9;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timePerSpawn)
        {
            timer = 0;
            timePerSpawn *= spawnTimeMultiplier;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var val = Random.Range(0, enemyPrefabs.Count+0.5f);
        for (int i = 1; i <= enemyPrefabs.Count; i++)
        {
            if (val <= i)
            {
                Instantiate(enemyPrefabs[i - 1], transform.position, transform.rotation);
                return;
            }
        }
    }
}
