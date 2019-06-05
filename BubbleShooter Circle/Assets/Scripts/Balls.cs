﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    BallSpawner ballSpawner;

    int Index = 0;

    bool SpeedingUp;
    bool Moving = true;

    public List<GameObject> Waypoints;

    public float DefaultSpeed;
    public float SpeedUpTime;
    public float FastSpeed;
    public float Speed;


    void Start()
    {
        Waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
        ballSpawner = FindObjectOfType<BallSpawner>();
    }

    void Update()
    {
        if (Moving)
        {
            Vector3 targetDir = Waypoints[Index].transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, Speed, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[Index].transform.position, Speed * Time.deltaTime);

            float Distance = Vector3.Distance(transform.position, Waypoints[Index].transform.position);
            if (Distance <= 0)
            {
                Index++;
            }
        }

        float DistancePreviousBall = Vector3.Distance(transform.position, ballSpawner.CheckListPos(gameObject).transform.position);
        Debug.Log(DistancePreviousBall);
        if (DistancePreviousBall > 0.66f && !SpeedingUp)
        {
            MoveBack();
        }
        else if (DistancePreviousBall < 0.66f && !SpeedingUp)
        {
            MoveAgain();
        }
    }

    void MoveBack()
    {
        Moving = false;

        GameObject PreviousBall = ballSpawner.CheckListPos(gameObject);

        Vector3 targetDir = PreviousBall.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, Speed, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = Vector3.MoveTowards(transform.position, PreviousBall.transform.position, Speed * 2 * Time.deltaTime);


        float DistancePreviousBall = Vector3.Distance(transform.position, PreviousBall.transform.position);
        if (DistancePreviousBall < 0.66f && !SpeedingUp)
        {
            MoveAgain();
        }
    }
    void MoveAgain()
    {
        Moving = true;
        Speed = DefaultSpeed;
    }

    public void PlaceShotBall(Color BallColor)
    {
        int ThisObjectInList = 0;
        for (int i = 0; i < Waypoints.Count; i++)
        {
            if (ballSpawner.SpawnedBalls[i] == gameObject)
            {
                ThisObjectInList = i;
                break;
            }
            else
                continue;
        }

        GameObject BallToSetUp = Instantiate(gameObject, transform.position, transform.rotation);
        Balls BallToSetupScript = BallToSetUp.GetComponent<Balls>();
        BallToSetUp.GetComponent<Renderer>().material.SetColor("_Color", BallColor);
        ballSpawner.SpawnedBalls.Insert(ThisObjectInList, BallToSetUp);
        BallToSetupScript.Index = Index;
        ballSpawner.SpeedUp(gameObject, BallToSetUp);
    }

    public IEnumerator StartSpeedUp()
    {
        SpeedingUp = true;
        Speed = FastSpeed;

        yield return new WaitForSeconds(SpeedUpTime);

        Speed = DefaultSpeed;
        SpeedingUp = false;
    }
}