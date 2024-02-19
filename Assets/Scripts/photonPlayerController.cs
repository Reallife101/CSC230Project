using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class photonPlayerController : MonoBehaviour
{
    //Components
    private PlayerManager myPM;
    private PhotonView myPV;
    private Rigidbody2D myRB;

    [SerializeField]
    float speed;

    [SerializeField]
    float maxRight;

    [SerializeField]
    float maxLeft;

    [SerializeField]
    List<GameObject> objects;

    [SerializeField]
    List<GameObject> UIObjects;
    [SerializeField]
    List<GameObject> SpawnObjects;

    [SerializeField]
    GameObject UICanvas;

    [SerializeField]
    GameObject hinderanceCanvas;

    [SerializeField]
    float spawnRate;

    public int nextObjectIndex;

    private float spawnTimer;

    private float dropTimer;

    [SerializeField]
    TMP_Text scoreText;

    private int score;
    private int hinderanceTracker;

    [SerializeField]
    List<GameObject> hinderanceSpawns;
    [SerializeField]
    List<GameObject> hinderances;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody2D>();
        myPM = PhotonView.Find((int)myPV.InstantiationData[0]).GetComponent<PlayerManager>();

        spawnTimer = 0f;
        dropTimer = 0f;
        nextObjectIndex = Random.Range(0, UIObjects.Count);


        if (!myPV.IsMine)
        {
            UICanvas.SetActive(false);
        }
        else
        {
            showNextUI();
            score = 0;
            hinderanceTracker = 0;
            scoreText.text = "Score: " + score;

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!myPV.IsMine)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if ((horizontal < 0 && transform.position.x > maxLeft) || (horizontal > 0 && transform.position.x < maxRight))
        {
            transform.Translate(horizontal, 0, 0);
        }

        if ((Input.GetKey(KeyCode.Space) && spawnTimer > spawnRate) || dropTimer>4.9f)
        {
            spawnTimer = 0f;
            dropTimer = 0f;
            GameObject ball = PhotonNetwork.Instantiate(objects[nextObjectIndex].name, transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
            ball.GetComponent<photonBallCombine>().player = this;

            scoreAdd((nextObjectIndex+1) * 100);
            nextObjectIndex = Random.Range(0, objects.Count);
            showNextUI();
        }

        //update spawn timer
        spawnTimer += Time.deltaTime;
        dropTimer += Time.deltaTime;

    }

    void showNextUI()
    {
        foreach (GameObject go in UIObjects)
        {
            go.SetActive(false);
        }
        UIObjects[nextObjectIndex].SetActive(true);

        //drop ball
        foreach (GameObject go in SpawnObjects)
        {
            go.GetComponent<flash>().stopCouritine();
            go.SetActive(false);
        }
        SpawnObjects[nextObjectIndex].SetActive(true);
        SpawnObjects[nextObjectIndex].GetComponent<flash>().spawnFlash();
    }

    public void scoreAdd(int i)
    {
        score += i;
        myPV.RPC(nameof(RPC_SetScore), RpcTarget.All, score);

        hinderanceTracker += i;
        
        if (hinderanceTracker >= 3000)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Game Manager"))
            {
                PhotonView pm = go.GetComponent<PhotonView>();
                if (pm != null && pm.ViewID != myPV.ViewID)
                {
                    pm.gameObject.GetComponent<photonPlayerController>().spawnHinder();
                    hinderanceTracker -= 3000;
                }
            }
        }
    }

    [PunRPC]
    void RPC_SetScore(int sc)
    {
        scoreText.text = "Score: " + sc;
    }

    public void spawnHinder()
    {
        myPV.RPC(nameof(RPC_SpawnHinderance), myPV.Owner);
    }
    
    public void loseGame()
    {
        if (myPV.IsMine)
        {
            myPM.loseGame();
        }
    }
    
    [PunRPC]
    void RPC_SpawnHinderance()
    {
        StartCoroutine(hinderanceCoroutine());
    }
    
    private IEnumerator hinderanceCoroutine()
    {
        hinderanceCanvas.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        hinderanceCanvas.SetActive(false);
        Vector3 t = hinderanceSpawns[Random.Range(0, hinderanceSpawns.Count)].transform.localPosition + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        GameObject go = PhotonNetwork.Instantiate(hinderances[Random.Range(0, hinderances.Count)].name, t, Quaternion.identity);
        photonBallCombine pbc = go.GetComponent<photonBallCombine>();
        if (pbc)
        {
            pbc.player = this;
        }

    }
    
}
