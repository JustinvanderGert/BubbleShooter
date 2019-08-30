using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    PlayerController Player;
    Text ScoreText;


    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        ScoreText = GetComponent<Text>();
        ScoreText.text = "Score: 0";
    }
    
    void Update()
    {
        ScoreText.text = "Score: " + Player.SpawnHeight;
    }
}