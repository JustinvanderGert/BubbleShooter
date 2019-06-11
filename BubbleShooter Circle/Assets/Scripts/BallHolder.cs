using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHolder : MonoBehaviour
{
    Turret turret;

    public List<GameObject> WaitingBalls;

    public Transform LastBallSpot;
    public Transform FirstBallSpot;

    public GameObject BallPrefab;


    public static readonly Color[] Colors =
    {
        new Color(1, 0, 0, 1),
        new Color(0, 1, 0, 1),
        new Color(0, 0, 1, 0),
    };

    void Start()
    {
        turret = FindObjectOfType<Turret>();

        for (int i = 0; i <= 4; i++)
        {
            var randomIndex = Random.Range(0, Colors.Length);
            var color = Colors[randomIndex];
            var BallToSetup = Instantiate(BallPrefab, FirstBallSpot.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            var renderer = BallToSetup.GetComponent<Renderer>();
            var BallPos = BallToSetup.transform.position;

            renderer.material.SetColor("_Color", color);
            BallToSetup.transform.position = new Vector3(BallPos.x, BallPos.y, BallPos.z - 0.5f * i);
            WaitingBalls.Add(BallToSetup);
            i++;
        }
        for (int i = 0; i < 3; i++)
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
        var randomIndex = Random.Range(0, Colors.Length);
        var color = Colors[randomIndex];
        var BallToSetup = Instantiate(BallPrefab, LastBallSpot.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        var renderer = BallToSetup.GetComponent<Renderer>();

        renderer.material.SetColor("_Color", color);
        WaitingBalls.Add(BallToSetup);

        //Reposition the second and third ball.
        for (int i = 0; i < WaitingBalls.Count; i++)
        {
            GameObject BallToAdjust = WaitingBalls[i];
            BallToAdjust.transform.position = new Vector3(BallToAdjust.transform.position.x, BallToAdjust.transform.position.y, BallToAdjust.transform.position.z + 0.5f);
        }
    }
}