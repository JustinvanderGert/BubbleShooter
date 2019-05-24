using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHolder : MonoBehaviour
{
    Turret turret;

    public List<GameObject> WaitingBalls;
    public List<Color> BallColors;

    public Transform NewBallSpot;
    public Transform NextUpBallSpot;

    public GameObject BallPrefab;


    void Start()
    {
        turret = FindObjectOfType<Turret>();

        BallColors.Add(Color.blue);
        BallColors.Add(Color.red);
        BallColors.Add(Color.green);

        for(int i = 0; i <= 4; i++)
        {
            int ColorI = Random.Range(0, BallColors.Count);

            GameObject BallToColor = Instantiate(BallPrefab, NextUpBallSpot.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            BallToColor.GetComponent<Renderer>().material.SetColor("_Color", BallColors[ColorI]);
            BallToColor.transform.position = new Vector3(BallToColor.transform.position.x, BallToColor.transform.position.y, BallToColor.transform.position.z - 0.5f * i);

            WaitingBalls.Add(BallToColor);
            i++;
        }
        for(int i = 0; i< 3; i++)
        {
            OnClick();
        }
    }
    
    public void OnClick()
    {
        //Set balls new parent, rotation, size and position.
        WaitingBalls[0].transform.parent = turret.ReadiedBallSpot.transform;
        WaitingBalls[0].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        WaitingBalls[0].transform.localScale = new Vector3(1, 1, 1);
        WaitingBalls[0].transform.position = new Vector3(0, 0, 0);

        //Give ball to the turret.
        turret.ReadiedBall = WaitingBalls[0];
        WaitingBalls.Remove(WaitingBalls[0]);

        //Create new third ball.
        GameObject BallToColor = Instantiate(BallPrefab, NewBallSpot.position, transform.rotation);
        BallToColor.GetComponent<Renderer>().material.SetColor("_Color", BallColors[Random.Range(0, BallColors.Count)]);

        WaitingBalls.Add(BallToColor);

        //Reposition the second and third ball.
        for (int i = 0; i < WaitingBalls.Count; i++)
        {
            GameObject BallToAdjust = WaitingBalls[i];
            BallToAdjust.transform.position = new Vector3(BallToAdjust.transform.position.x, BallToAdjust.transform.position.y, BallToAdjust.transform.position.z + 0.5f);
        }

    }
}