using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public class ControlMovementPatroller : MonoBehaviour
{
    [SerializeField] private AnimationClip shootingAnimation;
    private NPCState npcState = NPCState.Idle;
    private HandleDestination handleDestination;

    private GameObject player;

    private HandleAnimationController handleAnimationController;
    private Senses senses;
    private Shooting shooting;
    private Health health;

    private Animator animator;

    private ReactAttacked reactAttacked;

    private WaypointMovement waypointMovement;

    private MeleeAttack meleeAttack;
    // Start is called before the first frame update
    void Start()
    {
        meleeAttack = GetComponent<MeleeAttack>();
        reactAttacked = GetComponent<ReactAttacked>();
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
        
        if (waypointMovement != null)
        {
            destination = waypointMovement.GetNextDestination();
            npcState = NPCState.FollowingWaypoints;
        }
        // Check each of the scripts to see what the npc state should update to.
        // Check senses
        if (senses != null && senses.CanSensePlayer() || reactAttacked.WasAttacked)
        {
            destination = player.transform;
            npcState = NPCState.FollowingPlayer;

            // Check if the enemy is in range and ready to melee attack
            if(Vector3.Distance(transform.position, destination.position) <= 3){
                npcState = NPCState.Punching;
            }
            // Check if the player can shoot, if the player not already in shooting animation then reset shot timer.
            if (shooting != null && shooting.Ammo > 0 && npcState != NPCState.Punching)
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

        if (health != null)
        {
            if (health.HP <= 0)
            {
                npcState = NPCState.Dead;
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

 
    private void SyncAttackTime(float attackDelay)
    {
        animator.SetFloat("AttackSpeedMultiplier",
            1 / (shootingAnimation.length * 1 / (shootingAnimation.length * attackDelay)));
    }
}
