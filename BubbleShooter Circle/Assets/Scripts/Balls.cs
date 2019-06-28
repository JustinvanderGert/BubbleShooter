﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Balls : MonoBehaviour
{
    BallSpawner ballSpawner;
    NavMeshAgent Agent;

    Vector3 destination;
    Vector3 LastWaypointPos;

    int LastWaypointNum;
    int Index = 0;

    bool SpeedingUp;
    bool Moving = true;
    bool FirstMoveBack = true;

    public float DefaultSpeed = 0.6f;
    public float SpeedUpTime = 1.0f;
    public float FastSpeed = 1.2f;
    public float Speed = 0.6f;
    public float HighDist;

    public List<GameObject> Waypoints;
    public Transform SpawnPos;
    public Color MyColor;


    void Start()
    {
        Waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
        MyColor = GetComponent<Renderer>().material.color;
        ballSpawner = FindObjectOfType<BallSpawner>();
        Agent = GetComponent<NavMeshAgent>();

        destination = Waypoints[Index].transform.position;
        Agent.destination = destination;
        
        LastWaypointNum = Waypoints.Count;
        LastWaypointNum--;
        LastWaypointPos = Waypoints[LastWaypointNum].transform.position;
    }

    void Update()
    {
        var distanceEnd = Vector3.Distance(transform.position, LastWaypointPos);
        if (distanceEnd <= 0.5f)
        {
            GameOver();
        }

        Agent.speed = Speed;

        //Switch waypoints.
        if (Moving)
        {
            float Distance = Vector3.Distance(transform.position, Agent.destination);
            if (Distance <= 0f)
            {
                destination = Waypoints[Index++].transform.position;
                Agent.destination = destination;
            }
        }

        //Check Distance previous balls.
        var PrevBallPos = ballSpawner.CheckPreviousBall(this).transform.position;
        float DistancePreviousBall = Vector3.Distance(transform.position, PrevBallPos);

        if (DistancePreviousBall > 1 && !SpeedingUp)
        {
            MoveBack();
        }
        else if (DistancePreviousBall < 1 && !SpeedingUp && !Moving)
        {
            MoveAgain();
        }
    }

    void MoveBack()
    {
        Moving = false;

        var PreviousBall = ballSpawner.CheckPreviousBall(this);
        var targetPos = PreviousBall.transform.position;

        destination = targetPos;
        Agent.destination = destination;
        Speed = FastSpeed;

        float DistancePreviousBall = Vector3.Distance(transform.position, destination);
        if (DistancePreviousBall < 1 && !SpeedingUp)
        {
            HighDist = 0;
            FirstMoveBack = false;
            MoveAgain();
        }
    }
    void MoveAgain()
    {
        if (ballSpawner.TempColor == MyColor && ballSpawner.tempIndex == ballSpawner.CheckListPos(this))
        {
            var BalListPos = ballSpawner.CheckListPos(this);
            ballSpawner.CheckForTripples(MyColor, BalListPos);
        }

        Moving = true;

        destination = Waypoints[Index].transform.position;
        Agent.destination = destination;
        Speed = DefaultSpeed;
    }

    public void StartPlaceBall(Color BallColor)
    {
        StartCoroutine(PlaceShotBall(BallColor));
    }

    IEnumerator PlaceShotBall(Color BallColor)
    {
        var BallToSetUp = Instantiate(this, SpawnPos.position, transform.rotation);
        var BallToSetupScript = BallToSetUp.GetComponent<Balls>();
        var Renderer = BallToSetUp.GetComponent<Renderer>();
        var ListPos = ballSpawner.CheckListPos(this);

        Renderer.material.SetColor("_Color", BallColor);
        BallToSetupScript.MyColor = BallColor;
        ballSpawner.SpawnedBalls.Insert(ListPos, BallToSetUp);
        BallToSetupScript.Index = Index;
        BallToSetUp.gameObject.SetActive(false);

        ballSpawner.SpeedUp(this, BallColor);
        yield return new WaitForSeconds(0.25f);

        if (BallToSetUp)
            BallToSetUp.gameObject.SetActive(true);
    }

    public IEnumerator StartSpeedUp()
    {
        SpeedingUp = true;
        Speed = FastSpeed;

        yield return new WaitForSeconds(SpeedUpTime);

        Speed = DefaultSpeed;
        SpeedingUp = false;
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }
}