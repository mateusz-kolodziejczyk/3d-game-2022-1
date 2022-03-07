using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public class ControlMovement : MonoBehaviour
{
    [SerializeField] private AnimationClip shootingAnimation;
    private NPCState npcState = NPCState.Idle;
    private HandleDestination handleDestination;

    private GameObject player;

    private HandleAnimationController handleAnimationController;
    private Senses senses;
    private Shooting shooting;

    private Animator animator;

    private WaypointMovement waypointMovement;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        handleDestination = GetComponent<HandleDestination>();
        handleAnimationController = GetComponent<HandleAnimationController>();
        if (TryGetComponent(out Senses s))
        {
            senses = s;
        }
        if (TryGetComponent(out WaypointMovement wp))
        {
            waypointMovement = wp;
        }

        if (TryGetComponent(out Shooting shoot))
        {
            shooting = shoot;
            SyncAttackTime(shoot.AttackSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var destination = gameObject.transform;
        if (waypointMovement != null)
        {
            destination = waypointMovement.GetNextDestination();
            npcState = NPCState.FollowingWaypoints;
        }
        // Check each of the scripts to see what the npc state should update to.
        // Check senses
        if (senses != null && senses.CanSensePlayer())
        {
            destination = player.transform;
            npcState = NPCState.FollowingPlayer;

            // Check if the player can shoot, if the player not already in shooting animation then reset shot timer.
            if (shooting != null && shooting.Ammo > 0)
            {
                Debug.Log(shooting.Ammo);
                if (handleAnimationController.AnimState != AnimationState.Shooting)
                {
                    shooting.ResetShootingTimer();
                }

                if (shooting.ReadyToShoot())
                {
                    shooting.Shoot();
                }
                npcState = NPCState.Shooting;
            }
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

    private void SyncAttackTime(float attackDelay)
    {
        animator.SetFloat("AttackSpeedMultiplier", shootingAnimation.length / attackDelay);
    }
}
