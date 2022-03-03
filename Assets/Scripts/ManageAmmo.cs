using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ManageAmmo : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    int ammos = 20;
    public void decreaseAmmo()
    {
        ammos -= 5;
        if (ammos <= 0) ammos = 0;
        anim.SetInteger("ammos", ammos);
    }
    public void setAmmos(int newAMmos)
    {
        ammos = newAMmos;
    }
    public int getAmmos()
    {
        return ammos;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
    }
}
