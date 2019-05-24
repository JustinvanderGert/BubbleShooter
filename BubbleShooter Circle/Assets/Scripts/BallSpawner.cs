using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public List<Color> BallColors;
    public List<GameObject> SpawnedBalls;

    public GameObject Ball;

    public float SpawnTimer;

    public int BallsToSend;


    void Start()
    {
        BallColors.Add(Color.blue);
        BallColors.Add(Color.red);
        BallColors.Add(Color.green);

        StartCoroutine(Spawner());
    }

    public IEnumerator Spawner()
    {
        int i = Random.Range(0, BallColors.Count);
        BallsToSend--;

        yield return new WaitForSeconds(SpawnTimer);
        GameObject BallToSetUp = Instantiate(Ball, transform.position, transform.rotation);
        BallToSetUp.GetComponent<Renderer>().material.SetColor("_Color", BallColors[i]);
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
}