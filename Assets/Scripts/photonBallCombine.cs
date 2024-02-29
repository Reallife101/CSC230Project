using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class photonBallCombine : MonoBehaviour
{
    [SerializeField]
    public int level;

    [SerializeField]
    bool isHinderance = false;

    public bool spawn = true;

    public bool canLose = false;

    public bool hitGround = false;

    public bool flash = false;
    public bool die = false;

    private PhotonView myPV;

    [SerializeField]
    List<GameObject> objects;

    public photonPlayerController player;

    Color originalColor;

    float timer = 0;
    float loseTimer = 0;
    bool isOriginalColor = false;

    SpriteRenderer sr;


    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            hitGround = true;
        }
        
        photonBallCombine bc = collision.gameObject.GetComponent<photonBallCombine>();

        if (bc && myPV.IsMine)
        {
            
            if (bc.hitGround)
            {
                hitGround = true;
            }


            if (bc.level == level && !isHinderance && !bc.isHinderance)
            {
                //Prevent double spawning
                bc.spawn = false;
                if (spawn)
                {
                    //Calculate midpoint to spawn at
                    Vector3 bcPos = bc.gameObject.transform.position;
                    Vector3 spawnLocation = new Vector3(bcPos.x + (transform.position.x - bcPos.x) / 2, bcPos.y + (transform.position.y - bcPos.y) / 2, bcPos.z + (transform.position.z - bcPos.z) / 2);

                    //Delete both and spawn new one
                    PhotonNetwork.Destroy(bc.gameObject);
                    GameObject ball = PhotonNetwork.Instantiate(objects[Mathf.Min(level, objects.Count - 1)].name, spawnLocation, Quaternion.identity); //dont index out of range
                    ball.GetComponent<photonBallCombine>().player = player;

                    //player.scoreAdd(Mathf.Min(level, objects.Count - 1) * 100);

                    PhotonNetwork.Destroy(gameObject);
                }
            }

            if (hitGround)
            {
                canLose = true;
            }

        }
    }

    private void Update()
    {
        if (flash)
        {
            if (isOriginalColor)
            {
                //sr.color = originalColor;
                sr.color = new Color(255, 255, 255, 0);
            }
            else
            {
                sr.color = Color.red;

            }

            if (timer >.25f)
            {
                isOriginalColor = !isOriginalColor;
                timer = 0f;
            }

            timer += Time.deltaTime;

            
        }
        else
        {
            sr.color = originalColor;
            sr.color = new Color(255, 255, 255, 0);
            isOriginalColor = false;
            
        }

        if (die)
        {
            if (loseTimer > 2.5f)
            {
                loseGame();
                flash = false;
            }

            loseTimer += Time.deltaTime;
        }
        else
        {
            loseTimer = 0f;
        }

        
    }

    public void loseGame()
    {
        if (myPV.IsMine)
        {
            player.loseGame();
        }
    }
}
