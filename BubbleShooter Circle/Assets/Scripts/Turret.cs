using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    BallHolder ballHolder;

    bool GameStarted;
    bool AllowShot;

    public Transform ReadiedBallSpot;
    public GameObject ReadiedBall;

    public float RotSpeed;


    private void Start()
    {
        ballHolder = FindObjectOfType<BallHolder>();
        StartCoroutine(ShowFirstBall());
        AllowShot = true;
    }


    IEnumerator ShowFirstBall()
    {
        if (!GameStarted)
        {
            yield return new WaitUntil(() => ReadiedBall != null);
            ReadiedBall.transform.position = transform.position;
            GameStarted = true;
        }
    }


    void FixedUpdate()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        float hitdist = 0.0f;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && AllowShot)
        {
            //Shoot the readied ball.
            ReadiedBall.GetComponent<ShotBall>().Shoot();

            //Ready a new ball.
            ballHolder.OnClick();
            ReadiedBall.transform.position = ReadiedBallSpot.position;
            StartCoroutine(ShootTimer());
        }
    }

    IEnumerator ShootTimer()
    {
        AllowShot = false;
        yield return new WaitForSeconds(0.75f);
        AllowShot = true;
    }
}