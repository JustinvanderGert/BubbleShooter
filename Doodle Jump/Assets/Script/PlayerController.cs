using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpawnerManager SpawnerManager;
    Rigidbody Rb;
    Camera Main;

    Vector3 vel;
    Vector3 locVel;

    int DeleteHeight = -75;

    bool MoveLeftPressed;
    bool MoveRightPressed;
    bool Falling;
    bool GameOver;

    public Text GameOverText;

    public int SpawnHeight = 15;

    public float dragXZ; // drag value (1 is stop and 0 is no drag)
    public float MaxMovSpeed;
    public float MovSpeed;
    public float JumpSpeed;
    public float FallTime;

    void Start()
    {
        SpawnHeight = 15;
        GameOverText.text = "";
        SpawnerManager = FindObjectOfType<SpawnerManager>();
        Main = FindObjectOfType<Camera>();
        Cam cam = Main.GetComponent<Cam>();
        Rb = gameObject.GetComponent<Rigidbody>();

        cam.Player = transform;
    }

    void Update()
    {
        var CurrentHeight = transform.position.y;
        if (CurrentHeight >= SpawnHeight)
        {
            SpawnHeight += 15;
            DeleteHeight += 15;
            SpawnerManager.StartPlatformSpawn();
        }

        if (transform.position.x < -30)
        {
            transform.position = new Vector3(30, transform.position.y, 0);
        }
        else if (transform.position.x > 30)
        {
            transform.position = new Vector3(-30, transform.position.y, 0);
        }


        locVel = transform.InverseTransformDirection(Rb.velocity);
        locVel.x *= 1.0f - dragXZ;
        locVel.z *= 1.0f - dragXZ;
        Rb.velocity = transform.TransformDirection(locVel);


        Debug.Log(Rb.velocity);
        if (MoveLeftPressed && Rb.velocity.x >= -MaxMovSpeed)
        {
            transform.Translate(-Vector3.right * MovSpeed * Time.deltaTime);
            Rb.velocity = new Vector3(-MovSpeed, Rb.velocity.y, 0);
            Debug.Log(Rb.velocity);
        }
        if (MoveRightPressed && Rb.velocity.x <= MaxMovSpeed)
        {
            transform.Translate(Vector3.right * MovSpeed * Time.deltaTime);
            Rb.velocity = new Vector3(MovSpeed, Rb.velocity.y, 0);
            Debug.Log(Rb.velocity);
        }

        if (Rb.velocity.y <= 0)
        {
            if (!Falling)
            {
                Falling = true;
                StartCoroutine(GameOverScreen());
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 1.2f, Color.yellow);
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit Hit, 1.2f))
            {
                if (Hit.collider.gameObject.tag == "Platform")
                {
                    var velocityX = Rb.velocity.x;
                    Rb.velocity = new Vector3(velocityX, 0, 0);

                    var Platform = Hit.collider.gameObject.GetComponent<Platform>();
                    if (Platform.Boost)
                    {
                        Rb.AddForce(Vector3.up * JumpSpeed * 2, ForceMode.Impulse);
                    }
                    else
                    Rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);

                    Debug.Log("Did Hit");
                }
            }
        }
        else
        {
            Falling = false;
            StopAllCoroutines();
        }
    }

    public IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(FallTime);
        GameOver = true;
        GameOverText.text = "Game Over";
        Rb.isKinematic = true;
    }


    public void OnPointerDownLeftButton()
    {
        if (!GameOver)
            MoveLeftPressed = true;
    }
    public void OnPointerUpLeftButton()
    {
        if (!GameOver)
            MoveLeftPressed = false;
    }

    public void OnPointerDownRightButton()
    {
        if (!GameOver)
            MoveRightPressed = true;
    }
    public void OnPointerUpRightButton()
    {
        if (!GameOver)
            MoveRightPressed = false;
    }
}