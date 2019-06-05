using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBall : MonoBehaviour
{
    Color MyColor;

    bool Shot;

    public float Speed;
    public float LifeTime;


    void Start()
    {
        MyColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if (Shot)
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    public void Shoot()
    {
        gameObject.transform.parent = null;
        Shot = true;
        StartCoroutine(LivingTime());
    }


    IEnumerator LivingTime()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision Other)
    {
        if (Other.gameObject.GetComponent<Balls>() && Shot)
        {
            Shot = false;
            Balls HitBall = Other.gameObject.GetComponent<Balls>();
            HitBall.PlaceShotBall(MyColor);

            Destroy(gameObject);
        }
    }
}