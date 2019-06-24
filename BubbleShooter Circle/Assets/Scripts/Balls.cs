using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Balls : MonoBehaviour
{
    BallSpawner ballSpawner;
    NavMeshAgent Agent;
    Vector3 destination;
    int Index = 0;

    bool SpeedingUp;
    bool Moving = true;

    public float DefaultSpeed = 0.6f;
    public float SpeedUpTime = 1.0f;
    public float FastSpeed = 1.2f;
    public float Speed = 0.6f;

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
    }

    void Update()
    {
        Agent.speed = Speed;

        //Switch waypoints.
        if (Moving)
        {
            float Distance = Vector3.Distance(transform.position, Agent.destination);
            if (Distance <= 0.5f)
            {
                destination = Waypoints[Index++].transform.position;
                Agent.destination = destination;
            }
        }

        //Check Distance previous balls.
        var PrevBallPos = ballSpawner.CheckPreviousBall(this).transform.position;
        float DistancePreviousBall = Vector3.Distance(transform.position, PrevBallPos);

        if (DistancePreviousBall > 0.75f && !SpeedingUp)
        {
            MoveBack();
        }
        else if (DistancePreviousBall < 0.75f && !SpeedingUp && !Moving)
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
        if (DistancePreviousBall < 0.75f && !SpeedingUp)
            MoveAgain();
    }
    void MoveAgain()
    {
        var BalListPos = ballSpawner.CheckListPos(this);
        ballSpawner.CheckForTripples(MyColor, BalListPos);

        Moving = true;

        destination = Waypoints[Index].transform.position;
        Agent.destination = destination;
        Speed = DefaultSpeed;

    }

    public void PlaceShotBall(Color BallColor)
    {
        var BallToSetUp = Instantiate(this, SpawnPos.position, transform.rotation);
        var BallToSetupScript = BallToSetUp.GetComponent<Balls>();
        var Renderer = BallToSetUp.GetComponent<Renderer>();
        var ListPos = ballSpawner.CheckListPos(this);

        Renderer.material.SetColor("_Color", BallColor);
        BallToSetupScript.MyColor = BallColor;
        ballSpawner.SpawnedBalls.Insert(ListPos, BallToSetUp);
        BallToSetupScript.Index = Index;
        ballSpawner.SpeedUp(this, BallColor);
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