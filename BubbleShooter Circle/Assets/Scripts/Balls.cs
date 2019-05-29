using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    BallSpawner ballSpawner;

    int Index = 0;

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
        //Debug.Log("New Ball Spawned");
    }

    public IEnumerator StartSpeedUp()
    {
        Speed = FastSpeed;
        //Debug.Log(Speed);

        yield return new WaitForSeconds(SpeedUpTime);

        Speed = DefaultSpeed;
        //Debug.Log(Speed);
    }
}