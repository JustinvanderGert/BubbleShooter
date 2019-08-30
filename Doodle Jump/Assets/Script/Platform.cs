using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float BoostChance;
    public bool Boost;


    void Start()
    {
        var Chance = Random.Range(0, BoostChance);

        if (Chance >= BoostChance / 2)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            Boost = true;
        }
    }
    
    void Update()
    {
        
    }
}