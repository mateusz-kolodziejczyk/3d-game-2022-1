// using System.Collections;
// using System.Collections.Generic;
// using JetBrains.Annotations;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.Rendering;

// [RequireComponent(typeof(NavMeshAgent))]
// public class ControlNPC : MonoBehaviour
// {
//     [SerializeField] private GameObject target;

//     [SerializeField] private List<GameObject> paths;
//     [SerializeField] private GameObject breadCrumb;

//     public GameObject BreadCrumb
//     {
//         get => breadCrumb;
//         set => breadCrumb = value;
//     }

//     private List<List<GameObject>> _waypoints = new List<List<GameObject>>();
//     private int _currentPath = 0;
//     private int _wpCount = 0;

//     private bool _isWandering = false;

//     private bool _isFollowingWaypoints = false;
//     private GameObject _wanderingTarget;
//     private float _timer;
//     private Animator _animator;

//     private GameObject _player;
//     private float _patrolTimer = 0;

//     private float _counter = 0;

//     private Vector3 _startPosition;

//     private float shootTimer = 0;

//     private Vector3 _previousPosiiton;
//     // Start is called before the first frame update
//     void Start()
//     {
        
//         _player = GameObject.FindWithTag("Player");
//         _startPosition = transform.position;
//         _previousPosiiton = _startPosition;
//         if (_isWandering)
//         {
//             _wanderingTarget = new GameObject
//             {
//                 transform =
//                 {
//                     position = new Vector3(20, 0, 20)
//                 }
//             };
//             target = _wanderingTarget;
//             GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
//         }

//         for (int i = 0; i < paths.Count; i++)
//         {
//             _waypoints.Add(new List<GameObject>());
//             foreach (Transform child in paths[i].transform) 
//             {
//                 _waypoints[i].Add(child.gameObject);
//             }
//         }

//         _animator = GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Smell();
//         Listen();
//         Look();
//         _animator.SetInteger("ammos", GetComponent<ManageAmmo>().getAmmos());
//         if (_isWandering)
//         {
//             _timer += Time.deltaTime;
//             if (_timer > 4)
//             {
//                 _timer = 0;
//                 Wander();
//                 Move();
//             }
//         }

//         var currentState = _animator.GetCurrentAnimatorStateInfo(0);
//         if (currentState.IsName("LookForAmmos"))
//         {
//             target = GameObject.FindWithTag("ammo");
//         }
//         if (currentState.IsName("Idle"))
//         {
//             _patrolTimer += Time.deltaTime;
//             if (_patrolTimer > 5)
//             {
//                 _patrolTimer = 0;
//                 _animator.SetTrigger("patrol");
//             }

//             var navMeshAgent = GetComponent<NavMeshAgent>();
//             navMeshAgent.SetDestination(_startPosition);
//         }
//         if (currentState.IsName("Patrol"))
//         {
//             target = _waypoints[_currentPath][_wpCount];
//             if (Vector3.Distance(transform.position, target.transform.position) < 1.0)
//             {
//                 MoveToNextWp();
//                 //MoveToRandomWP();
//             }
//             Move();
//         }

//         if (currentState.IsName("FollowPlayer"))
//         {
//             shootTimer += Time.deltaTime;
//             if (shootTimer > 3)
//             {
//                 if (_animator.GetBool("canSeePlayer") && GetComponent<ManageAmmo>().getAmmos() > 0)
//                 {
                    
//                     shootTimer = 0;
//                     _animator.SetBool("shoot", true);
//                     GetComponent<ManageAmmo>().decreaseAmmo();
//                 }
//             }
//             target = _player;
//             Move();
//         }

//         if (currentState.IsName("Die"))
//         {
//             target = gameObject;
//             Move();
//         }

//         if (currentState.IsName("LookForHealth"))
//         {
//             var healthPickups = GameObject.FindGameObjectsWithTag("health");
//             float lowestDistance = float.MaxValue;
//             GameObject closestHealth = null;
//             foreach (var health in healthPickups)
//             {
//                 var distance = Vector3.Distance(transform.position, health.transform.position);
//                 if (distance < lowestDistance)
//                 {
//                     lowestDistance = distance;
//                     closestHealth = health;
//                 }
//             }

//             if (closestHealth != null)
//             {
//                 target = closestHealth;
//                 Move();
//             }
//         }

//     }

//     private void Smell()
//     {
//         GameObject[] allBCs = GameObject.FindGameObjectsWithTag("breadcrumb");
//         float minDistance = 10;
//         bool detectedBC = false;
//         for (int i = 0; i < allBCs.Length; i++)
//         {
//             if (Vector3.Distance(transform.position, allBCs[i].transform.position) < minDistance)
//             {
//                 detectedBC = true;
//                 break;
//             }
//         }
//         if (detectedBC)
//         {
//             _animator.SetBool("canSmellPlayer", true);
//         }
//         else _animator.SetBool("canSmellPlayer", false);
//     }

//     private void Move()
//     {
//         var navMeshAgent = GetComponent<NavMeshAgent>();
//         navMeshAgent.SetDestination(target.transform.position);
//     }

//     private void Listen()
//     {
//         float distance = Vector3.Distance(transform.position, _player.transform.position);
//         if (distance < -1)
//         {
//             _animator.SetBool("canHearPlayer", true);
//         }
//     }

//     private void Look()
//     {
//         var ray = new Ray();
//         RaycastHit hit;
//         float castingDistance = 20;
//         ray.origin = transform.position + Vector3.up * 0.7f;
//         ray.direction = transform.forward * castingDistance;
//         Debug.DrawRay(ray.origin, ray.direction, Color.red);
//         if (Physics.Raycast(ray.origin, ray.direction, out hit, castingDistance))
//         {
//             if (hit.collider.gameObject.CompareTag("Player"))
//             {
//                 _animator.SetBool("canSeePlayer", true);
//             }
//         }
//         else
//         {
//         }
//     }
//     private void Wander()
//     {
//         var ray = new Ray();
//         RaycastHit hit;
//         ray.origin = transform.position + Vector3.up * 0.7f;
//         float distanceToObstacle = 0;
//         float castingDistance = 20;

//         while (distanceToObstacle < 1.0f)
//         {
//             float dirX = Random.Range(-1, 1);
//             float dirY = Random.Range(-1, 1);

//             var t = transform;
//             ray.direction = t.forward * dirX + t.right * dirY;

//             if (Physics.Raycast(ray.origin, ray.direction, out hit, castingDistance))
//             {
//                 distanceToObstacle = hit.distance;
//             }
//             else
//             {
//                 distanceToObstacle = castingDistance;
//             }

//             _wanderingTarget.transform.position = ray.origin + ray.direction * (distanceToObstacle - 1);
//             target = _wanderingTarget;
//         }
        
//         Debug.DrawRay(ray.origin, ray.direction, Color.red);
//     }

//     private void MoveToNextWp()
//     {
//         _wpCount++;
//         if (_wpCount >= _waypoints[_currentPath].Count)
//         {
//             _wpCount = 0;
//             MoveToRandomPath();
//             _animator.SetBool("patrol", false);
//         }
//     }

//     private void MoveToRandomPath()
//     {
//         int random = 0;
//         random = Random.Range(0, _waypoints.Count);
//         while (random == _currentPath)
//         {
//             random = Random.Range(0, _waypoints.Count);
//         }

//         _currentPath = random;
//     }

//     private void MoveToRandomWP()
//     {
//         int previous = _wpCount;
//         int random = 0;
//         do
//         {
//             random = Random.Range(0, _waypoints[_currentPath].Count);
//         } while (random == previous);

//         _wpCount = random;
//     }

//     private void CheckAhead()
//     {
//         var ray = new Ray();
//         RaycastHit hit;
//         float castingDistance = 2;
//         ray.origin = transform.position + Vector3.up * 0.7f;
//         ray.direction = transform.forward * castingDistance;
//         Debug.DrawRay(ray.origin, ray.direction, Color.red);
//         if (Physics.Raycast(ray.origin, ray.direction, out hit, castingDistance))
//         {
//             Debug.Log("Object in Sight!!!!!!!!!");
//         }
//         else
//         {
//         }
//     }
// }
