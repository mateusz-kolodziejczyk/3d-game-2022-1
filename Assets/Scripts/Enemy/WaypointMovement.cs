using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> paths;
    private HandleDestination handleDestination;

    private List<List<GameObject>> waypoints = new List<List<GameObject>>();
    private int currentPath = 0;
    private int wpCount = 0;

    [SerializeField] private bool followsRandomWaypoints;
    [SerializeField] private bool followsRandomPaths;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        handleDestination = GetComponent<HandleDestination>();
        for (int i = 0; i < paths.Count; i++)
        {
            waypoints.Add(new List<GameObject>());
            foreach (Transform child in paths[i].transform) 
            {
                waypoints[i].Add(child.gameObject);
            }
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Transform GetNextDestination()
    {
        var destination = waypoints[currentPath][wpCount].transform;
        if (Vector3.Distance(transform.position, destination.position) < 1.0)
        {
            if (followsRandomWaypoints)
            {
                MoveToRandomPath();
                MoveToRandomWP();
            }
            else
            {
                MoveToNextWp();
            }
        }

        return destination;
    }

    private void MoveToNextWp()
    {
        wpCount++;
        if (wpCount >= waypoints[currentPath].Count)
        {
            wpCount = 0;
            if (followsRandomPaths)
            {
                MoveToRandomPath();
            }
            else
            {
                MoveToNextPath();
            }
        }
    }

    private void MoveToNextPath()
    {
        currentPath++;
        if (currentPath >= waypoints.Count)
        {
            currentPath = 0;
        }
    }

    private void MoveToRandomPath()
    {
        if(waypoints.Count <= 1){
            currentPath = 0;
            return;
        }
        int random = 0;
        random = Random.Range(0, waypoints.Count);
        while (random == currentPath)
        {
            random = Random.Range(0, waypoints.Count);
        }

        currentPath = random;
    }

    private void MoveToRandomWP()
    {
        int previous = wpCount;
        int random = 0;
        do
        {
            random = Random.Range(0, waypoints[currentPath].Count);
        } while (random == previous);

        wpCount = random;
    }

}
