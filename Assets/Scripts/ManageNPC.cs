using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ManageNPC : MonoBehaviour
{
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < 1.5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
