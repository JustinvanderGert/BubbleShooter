using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public List<Color> PossibleBallColors;
    public List<GameObject> SpawnedBalls;

    public GameObject BallPrefab;

    public float SpawnTimer;

    public int BallsToSend;


    void Start()
    {
        PossibleBallColors.Add(Color.blue);
        PossibleBallColors.Add(Color.red);
        PossibleBallColors.Add(Color.green);

        StartCoroutine(Spawner());
    }

    public IEnumerator Spawner()
    {
        int i = Random.Range(0, PossibleBallColors.Count);
        BallsToSend--;

        yield return new WaitForSeconds(SpawnTimer);
        GameObject BallToSetUp = Instantiate(BallPrefab, transform.position, transform.rotation);
        BallToSetUp.GetComponent<Renderer>().material.SetColor("_Color", PossibleBallColors[i]);
        SpawnedBalls.Add(BallToSetUp);

        if (BallsToSend >= 1)
            StartCoroutine(Spawner());
    }

    public void SpeedUp(GameObject HitBall)
    {
        bool FirstMove = true;
        int Index = 0;

        foreach (GameObject Ball in SpawnedBalls)
        {
            if (Ball == HitBall)
            {
                //CheckForTripples(Hitball, Index);

                for (int i = Index; i >= 0; i--)
                {
                    if (FirstMove)
                    {
                        Index++;
                        FirstMove = false;
                        continue;
                    }
                    if (i < 0)
                        break;

                    StartCoroutine(SpawnedBalls[i].GetComponent<Balls>().StartSpeedUp());
                }
                break;
            }
            else
                Index++;
        }
    }

    public void CheckForTripples(GameObject HitBall, int HitballSpot)
    {
        Color CurrentColor;
        Color PreviousColor = Color.white;
        int SameColors = 0;

        for (int i = 0; i < SpawnedBalls.Count; i++)
        {
            CurrentColor = SpawnedBalls[i].GetComponent<Renderer>().material.color;

            if(PreviousColor != null && CurrentColor == PreviousColor)
            {
                SameColors++;
            }

            if(SameColors >= 3)
            {
                Debug.Log("3 or more next to each other");
            }

            PreviousColor = CurrentColor;
        }
    }
}