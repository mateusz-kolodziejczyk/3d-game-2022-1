using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public class ControlIntelligentPatroller : MonoBehaviour
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

    private WaypointMovement waypointMovement;
    private ReactAttacked reactAttacked;

    // Start is called before the first frame update
    void Start()
    {
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
        if (senses != null && senses.CanSensePlayer()  || reactAttacked.WasAttacked)
        {
            destination = player.transform;
            npcState = NPCState.FollowingPlayer;

            // Check if the player can shoot, if the player not already in shooting animation then reset shot timer.
            if (shooting != null && shooting.Ammo > 0)
            {
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

        if (shooting != null && shooting.Ammo <= shooting.MaxAmmo * 0.2)
        {
            npcState = NPCState.LookingForAmmo;
            var ammoPacks = GameObject.FindGameObjectsWithTag("ammo");
            var minDistance = float.MaxValue;
            var closestAmmoPack = gameObject;
            foreach (var ammoPack in ammoPacks)
            {
                var distance = Vector3.Distance(transform.position, ammoPack.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAmmoPack = ammoPack;
                }
            }

            // If the ammo pack has been found, assign it.
            if (closestAmmoPack != gameObject)
            {
                destination = closestAmmoPack.transform;
            }
        }

        if (health != null)
        {
            if (health.HP <= 0)
            {
                npcState = NPCState.Dead;
            }

            if (health.HP <= health.MaxHP * 0.2)
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
        animator.SetFloat("AttackSpeedMultiplier",
            1 / (shootingAnimation.length * 1 / (shootingAnimation.length * attackDelay)));
    }
}
