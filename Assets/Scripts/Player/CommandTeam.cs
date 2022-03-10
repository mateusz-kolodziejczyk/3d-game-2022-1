using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTeam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var teamMembers = findTeamMembers();
            foreach (var member in teamMembers)
            {
                member.GetComponent<ControlTeam>().TargetEnemy();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            var teamMembers = findTeamMembers();
            foreach (var member in teamMembers)
            {
                member.GetComponent<ControlTeam>().StopTargettingEnemy();
            }
        }
    }

    private GameObject[] findTeamMembers()
    {
        return GameObject.FindGameObjectsWithTag("friendly");
    } 
}
