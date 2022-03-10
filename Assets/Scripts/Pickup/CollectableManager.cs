using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableManager : MonoBehaviour
{

    [SerializeField] private int numToWin = 5;

    private int counter = 0;

    private float timer = 0;

    [SerializeField] private float maxTime = 180;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxTime)
        {
            // Reset if max time pass
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Collect()
    {
        counter++;
        if (counter >= numToWin)
        {
            SceneManager.LoadScene("Win");
        }
    }
    
    
}
