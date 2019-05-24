using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHolder : MonoBehaviour
{
    Turret turret;

    public List<GameObject> WaitingBalls;
    public List<Color> PossibleBallColors;

    public Transform LastBallSpot;
    public Transform FirstBallSpot;

    public GameObject BallPrefab;


    void Start()
    {
        turret = FindObjectOfType<Turret>();

        PossibleBallColors.Add(Color.blue);
        PossibleBallColors.Add(Color.red);
        PossibleBallColors.Add(Color.green);

        for(int i = 0; i <= 4; i++)
        {
            int ColorI = Random.Range(0, PossibleBallColors.Count);

            GameObject BallToColor = Instantiate(BallPrefab, FirstBallSpot.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            BallToColor.GetComponent<Renderer>().material.SetColor("_Color", PossibleBallColors[ColorI]);
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
        WaitingBalls[0].transform.parent = turret.ReadiedBallSpot;
        WaitingBalls[0].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        WaitingBalls[0].transform.localScale = new Vector3(1, 1, 1);
        WaitingBalls[0].transform.position = new Vector3(0, 0, 0);

        //Give ball to the turret.
        turret.ReadiedBall = WaitingBalls[0];
        WaitingBalls.Remove(WaitingBalls[0]);

        //Create new third ball.
        GameObject BallToColor = Instantiate(BallPrefab, LastBallSpot.position, transform.rotation);
        BallToColor.GetComponent<Renderer>().material.SetColor("_Color", PossibleBallColors[Random.Range(0, PossibleBallColors.Count)]);

        WaitingBalls.Add(BallToColor);

        //Reposition the second and third ball.
        for (int i = 0; i < WaitingBalls.Count; i++)
        {
            GameObject BallToAdjust = WaitingBalls[i];
            BallToAdjust.transform.position = new Vector3(BallToAdjust.transform.position.x, BallToAdjust.transform.position.y, BallToAdjust.transform.position.z + 0.5f);
        }

    }
}