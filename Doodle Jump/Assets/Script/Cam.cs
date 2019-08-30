using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform Player;
    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Player.position.y + 15, transform.position.z);
    }
}