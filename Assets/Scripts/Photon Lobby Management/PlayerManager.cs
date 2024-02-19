using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    //Player manager will keep track of scoring, tarot cards, and respawning for the player (ie all data we want to persist past player death)
    private PhotonView PV;
    public GameObject[] playerPrefabs;
    private GameObject leftSpawn;
    private GameObject rightSpawn;

    GameObject controller;
    photonPlayerController PlayerController;

    [SerializeField] private AnimationCurve timeSlowCurve;

    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject LoseScreen;


    private void Awake()
    {
        
        rightSpawn = GameObject.FindWithTag("RightSpawn");
        leftSpawn = GameObject.FindWithTag("LeftSpawn");
        PV = GetComponent<PhotonView>();

    }

    private void Start()
    {
        if (PV.IsMine)
        {
            Spawn();
        }
    }

    //Spawns/Respawn player
    private void Spawn()
    {

        Transform spawnPoint;

        if (PhotonNetwork.IsMasterClient)
        {
            spawnPoint = leftSpawn.transform;
        }
        else
        {
            spawnPoint = rightSpawn.transform;
        }


        //0 if host
        GameObject playerToSpawn = playerPrefabs[PhotonNetwork.IsMasterClient? 0 : 1];

        controller = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity, 0, new object[] { PV.ViewID });
        

        PlayerController = controller.GetComponent<photonPlayerController>();

       
    }

    public void SetController(GameObject _controller)
    {
        controller = _controller;
    }

    public GameObject GetController()
    {
        return controller;
    }

    public playerController GetPlayerController()
    {
        return controller.GetComponent<playerController>();
    }

    public void loseGame()
    {
        if (PV.IsMine)
        {
            // Tell everyone Game is Over
            PV.RPC(nameof(RPC_WinGame), RpcTarget.Others);
            StartCoroutine(LoseCoroutine());
        }
    }

    [PunRPC]
    void RPC_WinGame()
    {
        StartCoroutine(WinCoroutine());
    }

    private IEnumerator LoseCoroutine()
    {
        LoseScreen.SetActive(true);
        Time.timeScale = .1f;

        yield return new WaitForSecondsRealtime(.5f);

        PhotonNetwork.LoadLevel("Lobby");

    }

    private IEnumerator WinCoroutine()
    {
        winScreen.SetActive(true);

        Time.timeScale = .1f;
        yield return new WaitForSecondsRealtime(.5f);

        PhotonNetwork.LoadLevel("Lobby");

    }

}
