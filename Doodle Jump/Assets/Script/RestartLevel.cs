using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RestartLevel : MonoBehaviour
{
    bool IsPressed;


    void Update()
    {
        if (IsPressed)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void OnPressed()
    {
        IsPressed = true;
    }
}