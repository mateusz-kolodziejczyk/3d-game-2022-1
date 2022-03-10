using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using Random = UnityEngine.Random;


public class ControlMob : MonoBehaviour
{
    private NPCState npcState = NPCState.Idle;
    private HandleDestination handleDestination;

    private GameObject player;
    private GameObject leader;
    public GameObject Leader
    {
        get => leader;
        set => leader = value;
    }

    public bool IsFollowingPlayer { get; set; } = false;

    private HandleAnimationController handleAnimationController;
    private Health health;

    private Animator animator;

    private float maxPathTime = 0.5f;

    private float pathTime = 4;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        handleDestination = GetComponent<HandleDestination>();
        handleAnimationController = GetComponent<HandleAnimationController>();

        if (TryGetComponent(out Health h))
        {
            health = h;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var destination = gameObject.transform;
        if (health.HP <= 0)
        {
            npcState = NPCState.Dead;
            handleAnimationController.AnimState = AnimationState.Dying;
            return;
        }
        
        if (leader == null)
        {
            pathTime += Time.deltaTime;
            if (pathTime >= maxPathTime)
            {
                handleDestination.ClearDestination();

                pathTime = 0;
                handleDestination.SetDestinationWithPosition(Wander());
                handleAnimationController.AnimState = AnimationState.Walking;
            }
            return;
        }

        if (Vector3.Distance(transform.position, Leader.transform.position) >= 2)
        {
            destination = Leader.transform;
            npcState = NPCState.FollowingLeader;
        }

        if (IsFollowingPlayer)
        {
            npcState = NPCState.FollowingPlayer;
            destination = player.transform;
        }
        
        handleDestination.Destination = destination;

        // Go through each of the supported scripts to check which should run based on the npc state
        switch (npcState)
        {
            case NPCState.Idle:
                handleAnimationController.AnimState = AnimationState.Idle;
                break;
            case NPCState.FollowingPlayer:
            case NPCState.FollowingWaypoints:
            case NPCState.LookingForAmmo:
            case NPCState.LookingForHealth:
            case NPCState.FollowingLeader:
                handleAnimationController.AnimState = AnimationState.Walking;
                break;
            case NPCState.Dead:
                handleAnimationController.AnimState = AnimationState.Dying;
                break;
            case NPCState.Shooting:
                handleAnimationController.AnimState = AnimationState.Shooting;
                break;
            default:
                break;
        }
    }

    private Vector3 Wander()
    {

        npcState = NPCState.Wandering;
        // Code from https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/
        // // Taken from https://answers.unity.com/questions/1661755/how-to-instantiate-objects-in-a-circle-formation-a.html

        
        var rnd = Random.value;
        /* Distance around the circle */  
        var radians = 10 * Math.PI / rnd;
         
        /* Get the vector direction */ 
        var vertical = MathF.Sin((float)radians);
        var horizontal = MathF.Cos((float)radians); 
         
        var spawnDir = new Vector3 (horizontal, 0, vertical);
         
        /* Get the spawn position */ 
        var randomDirection = transform.position + spawnDir * 2; // Radius is just the distance away from the point

        NavMeshHit navHit;
           
        NavMesh.SamplePosition (randomDirection, out navHit, 2, -1);
        return navHit.position;
    }
}