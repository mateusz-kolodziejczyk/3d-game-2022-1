using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public class ControlSniper : MonoBehaviour
{
    private NPCState npcState = NPCState.Idle;
    private HandleDestination handleDestination;

    private GameObject player;

    private HandleAnimationController handleAnimationController;
    private Health health;

    private Animator animator;

    private GameObject ambushSite;

    public bool GoToAmbush { get; set; } = false;

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        var position = transform.position;
        startPosition = new Vector3(position.x, position.y, position.z);
        ambushSite = GameObject.FindWithTag("AmbushSite");
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

        // If the npc is dead, do not do any more animation
        if (npcState == NPCState.Dead)
        {
            handleDestination.Destination = destination;
            return;
        }

        // Flee to start
        handleDestination.SetDestinationWithPosition(startPosition);
        npcState = NPCState.Fleeing;

        if (GoToAmbush)
        {
            npcState = NPCState.GoToAmbush;
            destination = ambushSite.transform;
            if (Vector3.Distance(transform.position, ambushSite.transform.position) <= 15)
            {
                npcState = NPCState.ThrowGrenade;
                destination = transform;
                GoToAmbush = false;
            }
        }

        if (handleAnimationController.AnimState == AnimationState.ThrowGrenade)
        {
            npcState = NPCState.ThrowGrenade;
            destination = transform;

        }
        if (health != null)
        {


            if (health.HP <= health.MaxHP * 0.2 )
            {
                npcState = NPCState.LookingForHealth;
                // Search for all healthpacks and go towards the nearest one.
                var healthPacks = GameObject.FindGameObjectsWithTag("health");
                var minDistance = float.MaxValue;
                var closestHealthPack = gameObject;
                foreach (var healthPack in healthPacks)
                {
                    var distance = Vector3.Distance(transform.position, healthPack.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestHealthPack = healthPack;
                    }
                }

                // If the ammo pack has been found, assign it.
                if (closestHealthPack != gameObject)
                {
                    destination = closestHealthPack.transform;
                }
            }
        }
        
        if (health.HP <= 0)
        {
            npcState = NPCState.Dead;
        }

        if (destination != gameObject.transform)
        {
            handleDestination.Destination = destination;
        }

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
                handleAnimationController.AnimState = AnimationState.Walking;
                break;
            case NPCState.Dead:
                handleAnimationController.AnimState = AnimationState.Dying;
                break;
            case NPCState.Shooting:
                handleAnimationController.AnimState = AnimationState.Shooting;
                break;
            case NPCState.Fleeing:
                handleAnimationController.AnimState = AnimationState.Fleeing;
                break;
            case NPCState.ThrowGrenade:
                handleAnimationController.AnimState = AnimationState.ThrowGrenade;
                break;
            case NPCState.GoToAmbush:
                handleAnimationController.AnimState = AnimationState.GoToAmbush;
                break;
            default:
                break;
        }
    }

}