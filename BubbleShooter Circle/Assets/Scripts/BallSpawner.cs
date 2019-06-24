using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BallSpawner : MonoBehaviour
{
    public static readonly Color[] Colors =
    {
        new Color(1, 0, 0, 1),
        new Color(0, 1, 0, 1),
        new Color(0, 0, 1, 0),
    };


    public List<Balls> SpawnedBalls;

    public Transform SpawnPos;
    public Balls BallPrefab;
    public float SpawnTimer;
    public int BallsToSend;

    bool AllowSpeedUp = true;


    IEnumerator Start()
    {
        for (var i = 0; i < BallsToSend; i++)
        {
            yield return new WaitForSeconds(SpawnTimer);

            var ball = Instantiate(BallPrefab, SpawnPos.position, transform.rotation);
            var renderer = ball.GetComponent<Renderer>();
            var randomIndex = Random.Range(0, Colors.Length);
            var color = Colors[randomIndex];

            ball.GetComponent<Balls>().MyColor = color;
            renderer.material.SetColor("_Color", color);
            SpawnedBalls.Add(ball);
        }
    }


    public GameObject CheckPreviousBall(Balls BallToCheck)
    {
        var index = SpawnedBalls.IndexOf(BallToCheck);
        index++;

        if (index == SpawnedBalls.Count)
            index--;

        var PreviousBall = SpawnedBalls[index];
        return PreviousBall.gameObject;
    }
    public int CheckListPos(Balls BallToCheck)
    {
        var index = SpawnedBalls.IndexOf(BallToCheck);

        if (index == SpawnedBalls.Count)
            index--;

        return index;
    }


    public void SpeedUp(Balls HitBall, Color ShotBall)
    {
        var index = SpawnedBalls.IndexOf(HitBall);
        index--;
        if (index < 0)
            return;

        AllowSpeedUp = true;
        CheckForTripples(ShotBall, index);

        if (AllowSpeedUp)
        {
            for (var i = index - 1; i >= 0; i--)
            {
                StartCoroutine(SpawnedBalls[i].StartSpeedUp());
            }
        }
    }

    public List<Balls> SameColoredBalls;
    public void CheckForTripples(Color ShotBall, int HitballSpot)
    {
        var MyColor = ShotBall;
        HitballSpot -= 1;
        for (var i = HitballSpot; i >= 0; i--)
        {
            if (SpawnedBalls[i].MyColor == MyColor)
            {
                SameColoredBalls.Add(SpawnedBalls[i]);
                continue;
            }
            else
                break;
        }

        HitballSpot += 1;
        if (HitballSpot > 0)
        {
            for (var i = HitballSpot; i <= SpawnedBalls.Count; i++)
            {
                if (SpawnedBalls[i].MyColor == MyColor)
                {
                    SameColoredBalls.Add(SpawnedBalls[i]);
                    continue;
                }
                else
                    break;
            }
        }


        if (SameColoredBalls.Count >= 3)
        {
            AllowSpeedUp = false;

            for (var i = 0; i < SameColoredBalls.Count; i++)
            {
                SpawnedBalls.Remove(SameColoredBalls[i]);
                Destroy(SameColoredBalls[i].gameObject);
            }
            SameColoredBalls.Clear();
        }
        SameColoredBalls.Clear();
    }
}