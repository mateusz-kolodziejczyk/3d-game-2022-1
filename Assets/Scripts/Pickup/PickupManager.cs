using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickupManager : MonoBehaviour
{
    private List<Spawner> ammoSpawners = new ();
    private List<Spawner> healthSpawners = new ();

    [SerializeField] float minPickup = 0.25f;
    [SerializeField] float maxPickup = 0.65f;

    [SerializeField] private float timeBetweenChecks = 5;
    [SerializeField] private float chanceToSpawn = 0.2f;
    private float currentTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        populateSpawnerList(ref ammoSpawners, "ammospawner");
        populateSpawnerList(ref healthSpawners, "healthspawner");
        currentTimer = timeBetweenChecks;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        // Randoming list using code frmo https://stackoverflow.com/questions/273313/randomize-a-listt
        //Go through list randomly each time an update happens.
        if(currentTimer > timeBetweenChecks){
            currentTimer = 0;
            ammoSpawners = ammoSpawners.OrderBy(a => Random.Range(0,100)).ToList();
            healthSpawners = healthSpawners.OrderBy(a => Random.Range(0,100)).ToList();
            checkAndSpawn(ammoSpawners);
            checkAndSpawn(healthSpawners);
        }

    }

    private void populateSpawnerList(ref List<Spawner> list, string tag){
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();
        list = objects.Select(o => o.GetComponent<Spawner>()).ToList();
    }

    private void checkAndSpawn(List<Spawner> spawners){
                   
            int counter = 0;
            var emptySpawners = new List<Spawner>();
            // Count active pickups, put empty pickups in a list
            foreach(var spawner in spawners){
                if(spawner.IsSpawned()){
                    counter++;
                }
                else{
                    emptySpawners.Add(spawner);
                }
            }
            foreach(var spawner in emptySpawners){
                // Check if min threshold is not reached
                if(counter < spawners.Count * minPickup){
                    spawner.Spawn();
                    counter++;
                }
                else if(counter < spawners.Count * maxPickup){
                    var rnd = Random.value;
                    if(rnd > 1-chanceToSpawn){
                        spawner.Spawn();
                        counter++;
                    }
                }
            }
    }
}
