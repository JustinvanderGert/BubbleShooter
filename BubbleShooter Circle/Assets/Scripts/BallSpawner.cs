using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public List<GameObject> SameColoredBalls;
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

    public void SpeedUp(GameObject HitBall, GameObject ShotBall)
    {
        bool FirstMove = true;
        int Index = 0;

        foreach (GameObject Ball in SpawnedBalls)
        {
            if (Ball == HitBall)
            {

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
                CheckForTripples(ShotBall, Index);
                break;
            }
            else
                Index++;
        }
    }

    public void CheckForTripples(GameObject ShotBall, int HitballSpot)
    {
        int TempInt = HitballSpot;
        int MaxDistanceAlowed = TempInt -4;
        int SameColors = 0;
        
        Color PreviousColor = ShotBall.GetComponent<Renderer>().material.color;

        if (MaxDistanceAlowed <= 0)
            MaxDistanceAlowed = 0;

        for (int i = HitballSpot; i >= MaxDistanceAlowed; i--)
        {
            Color BallColor = SpawnedBalls[i].GetComponent<Renderer>().material.color;
            Color CurrentColor = BallColor;

            if (CurrentColor == PreviousColor)
            {
                Debug.Log("Has same Color next to it");
                BallColor = Color.white;
                SameColoredBalls.Add(SpawnedBalls[i]);
                SameColors++;
            }
            else
            {
                Debug.Log("Does not have same Color next to it");
                BallColor = Color.black;
            }

            SpawnedBalls[i].GetComponent<Renderer>().material.color = BallColor;

            if (SameColors >= 3)
            {
                Debug.Log("3 or more next to each other");
                foreach(GameObject SameColoredBall in SameColoredBalls)
                {
                    SpawnedBalls.Remove(SameColoredBall);

                    Destroy(SameColoredBall);
                }
            }
        }
        SameColoredBalls.Clear();
    }
}