using System.Collections;
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
    public Color MyColor;


    void Start()
    {
        Waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
        MyColor = GetComponent<Renderer>().material.color;
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
        
        float DistancePreviousBall = Vector3.Distance(transform.position, ballSpawner.CheckPreviousBall(this).transform.position);
        if (DistancePreviousBall > 0.66f && !SpeedingUp)
        {
            Debug.Log("Move Back");
            MoveBack();
        }
        else if (DistancePreviousBall < 0.66f && !SpeedingUp && !Moving)
        {
            MoveAgain();
        }
    }

    void MoveBack()
    {
        Moving = false;

        var PreviousBall = ballSpawner.CheckPreviousBall(this);
        var targetDir = PreviousBall.transform.position - transform.position;
        var newDir = Vector3.RotateTowards(transform.forward, targetDir, Speed, 0.0f);

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

        //var PrevBall = ballSpawner.CheckPreviousBall(this);
        //var PrevBallScript = PrevBall.GetComponent<Balls>();

        //float Distance1 = Vector3.Distance(transform.position, Waypoints[Index].transform.position);
        //float Distance2 = Vector3.Distance(transform.position, Waypoints[PrevBallScript.Index].transform.position);
        //if (Distance1 > Distance2)
        //{
        //    Index = PrevBallScript.Index;
        //}
    }

    public void PlaceShotBall(Color BallColor)
    {
        var BallToSetUp = Instantiate(this, transform.position, transform.rotation);
        var BallToSetupScript = BallToSetUp.GetComponent<Balls>();
        var Renderer = BallToSetUp.GetComponent<Renderer>();
        var ListPos = ballSpawner.CheckListPos(this);

        Renderer.material.SetColor("_Color", BallColor);
        BallToSetupScript.MyColor = BallColor;
        ballSpawner.SpawnedBalls.Insert(ListPos, BallToSetUp);
        BallToSetupScript.Index = Index;
        ballSpawner.SpeedUp(this, BallToSetUp);
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