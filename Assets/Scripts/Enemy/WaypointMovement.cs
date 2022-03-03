using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private List<GameObject> paths;

    private List<List<GameObject>> _waypoints = new List<List<GameObject>>();
    private int _currentPath = 0;
    private int _wpCount = 0;

    [SerializeField] private bool followsRandomWaypoints;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < paths.Count; i++)
        {
            _waypoints.Add(new List<GameObject>());
            foreach (Transform child in paths[i].transform) 
            {
                _waypoints[i].Add(child.gameObject);
            }
        }
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        target = _waypoints[_currentPath][_wpCount];
        if (Vector3.Distance(transform.position, target.transform.position) < 1.0)
        {
            MoveToNextWp();
        }
        Move();
    }

    private void Move()
    {
        var navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.SetDestination(target.transform.position);
    }

    private void MoveToNextWp()
    {
        _wpCount++;
        if (_wpCount >= _waypoints[_currentPath].Count)
        {
            _wpCount = 0;
            MoveToRandomPath();
            _animator.SetBool("patrol", false);
        }
    }

    private void MoveToRandomPath()
    {
        if(_waypoints.Count <= 1){
            _currentPath = 0;
            return;
        }
        int random = 0;
        random = Random.Range(0, _waypoints.Count);
        while (random == _currentPath)
        {
            random = Random.Range(0, _waypoints.Count);
        }

        _currentPath = random;
    }

    private void MoveToRandomWP()
    {
        int previous = _wpCount;
        int random = 0;
        do
        {
            random = Random.Range(0, _waypoints[_currentPath].Count);
        } while (random == previous);

        _wpCount = random;
    }

}
