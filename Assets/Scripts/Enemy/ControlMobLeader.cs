using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public class ControlMobLeader : MonoBehaviour
{

    private NPCState npcState = NPCState.Idle;
    private HandleDestination handleDestination;

    private GameObject player;

    private HandleAnimationController handleAnimationController;
    private Health health;
    private Senses senses;
    private Animator animator;

    [SerializeField] private GameObject mobMemberPrefab;
    [SerializeField] private int maxMobMembers;
    private List<GameObject> mobMembers = new List<GameObject>();
    private WaypointMovement waypointMovement;
    private ReactAttacked reactAttacked;
    // Start is called before the first frame update
    void Start()
    {
        reactAttacked = GetComponent<ReactAttacked>();
        waypointMovement = GetComponent<WaypointMovement>();
        senses = GetComponent<Senses>();
        spawnMobMembers();
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
        if (waypointMovement != null)
        {
            destination = waypointMovement.GetNextDestination();
            npcState = NPCState.FollowingWaypoints;
        }

        Debug.Log(mobMembers.Contains(null));
        Debug.Log(mobMembers.Count);
        // Check senses
        if (senses != null)
        {
            for (int i = 0; i < mobMembers.Count; i++)
            {
                var mobMember = mobMembers[i];
                if (mobMember == null)
                {
                    mobMembers.RemoveAt(i);
                    i--;
                    continue;
                }

                mobMember.GetComponent<ControlMob>().IsFollowingPlayer = false;

            }
            foreach (var mobMember in mobMembers)
            {
                mobMember.GetComponent<ControlMob>().IsFollowingPlayer = false;
            }
            
            if (!senses.CanSensePlayer() || mobMembers.Count < maxMobMembers )
            {

            }
            else if(senses.CanSensePlayer()  || reactAttacked.WasAttacked)
            {
                destination = player.transform;
                npcState = NPCState.FollowingPlayer;
                foreach (var mobMember in mobMembers)
                {
                    mobMember.GetComponent<ControlMob>().IsFollowingPlayer = true;
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
    
    // Taken from https://answers.unity.com/questions/1661755/how-to-instantiate-objects-in-a-circle-formation-a.html
    // Instantiates members around the mob leader
    private void spawnMobMembers()
    {
        for (int i = 0; i < maxMobMembers; i++){
         
            /* Distance around the circle */  
            var radians = 2 * Math.PI / maxMobMembers * i;
         
            /* Get the vector direction */ 
            var vertical = MathF.Sin((float)radians);
            var horizontal = MathF.Cos((float)radians); 
         
            var spawnDir = new Vector3 (horizontal, 0, vertical);
         
            /* Get the spawn position */ 
            var spawnPos = transform.position + spawnDir * 2; // Radius is just the distance away from the point
         
            /* Now spawn */
            var mobMember = Instantiate (mobMemberPrefab, spawnPos, Quaternion.identity);

            // Make the leader this game object.
            mobMember.GetComponent<ControlMob>().Leader = gameObject;
            
            // Add member to list
            mobMembers.Add(mobMember);
        }
    }
}