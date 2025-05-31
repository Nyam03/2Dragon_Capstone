using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slashanim : MonoBehaviour
{
    public GameObject SlashPrefab;
    private Transform Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.C))
        {
            GameObject slashInstance = Instantiate(SlashPrefab, Player.transform);
            Destroy(slashInstance, 0.5f);   

        }


    }
}
