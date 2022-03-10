using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public class ControlTeam : MonoBehaviour
{
    private NPCState npcState = NPCState.Idle;
    private HandleDestination handleDestination;

    private GameObject player;

    private GameObject enemyTarget;

    private HandleAnimationController handleAnimationController;
    private Health health;


    // Start is called before the first frame update
    void Start()
    {
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

        destination = player.transform;
        npcState = NPCState.FollowingPlayer;

        if (enemyTarget != null)
        {
            destination = enemyTarget.transform;
            npcState = NPCState.AttackingEnemies;
            // Check if the enemy is in range and ready to melee attack
            Debug.Log(Vector3.Distance(transform.position, destination.position));
            if(Vector3.Distance(transform.position, destination.position) <= 5){
                npcState = NPCState.Punching;
            }
        }
        
        if (health.HP <= 0)
        {
            npcState = NPCState.Dead;
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
            case NPCState.AttackingEnemies:
                handleAnimationController.AnimState = AnimationState.Walking;
                break;
            case NPCState.Dead:
                handleAnimationController.AnimState = AnimationState.Dying;
                break;
            case NPCState.Shooting:
                handleAnimationController.AnimState = AnimationState.Shooting;
                break;
            case NPCState.Punching:
                handleAnimationController.AnimState = AnimationState.Punching;
                break;
            default:
                break;
        }
    }
    
    // This targets the closest enemy found.
    public void TargetEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("enemy");
        var minDist = float.MaxValue;
        GameObject closestEnemy = null;

        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < minDist)
            {
                closestEnemy = enemy;
            }
        }

        enemyTarget = closestEnemy;
    }

    public void StopTargettingEnemy()
    {
        enemyTarget = null;
    }
}