using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using VectorClockNamespace;

public class photonPlayerController : MonoBehaviour
{
    //Components
    private PlayerManager myPM;
    private PhotonView myPV;
    private VectorClock clock;

    [SerializeField]
    GameObject UICanvas;


    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        myPM = PhotonView.Find((int)myPV.InstantiationData[0]).GetComponent<PlayerManager>();


        if (!myPV.IsMine)
        {
            UICanvas.SetActive(false);
        }
        else
        {
            //showNextUI();

        }

        clock = new VectorClock(myPV.ViewID);
    }


    // Update is called once per frame
    void Update()
    {
        if (!myPV.IsMine)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)){ 
            clock.Tick();
        }

        if (Input.GetKeyDown(KeyCode.A)){
            clock.Tick();
            myPV.RPC("RPC_RecieveMessage", RpcTarget.Others, clock.Clock());
        }

        if (Input.GetKeyDown(KeyCode.P)){
            clock.DisplayClock();
            
        }

    }

    public bool checkOwner()
    {
        return myPV.IsMine;
    }


    [PunRPC]
    void RPC_SetScore(int sc)
    {
        //scoreText.text = "Score: " + sc;
    }

    [PunRPC]
    void RPC_AddClock(int ID)
    {
        clock.Add(ID);
    }

    [PunRPC]
    void RPC_RemoveClock(int ID)
    {
        clock.Remove(ID);
    }

    [PunRPC]
    void RPC_RecieveMessage(Dictionary<int, int> externalClock)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Game Manager");
        Debug.Log("Length: "+gos.Length);
        foreach (GameObject go in gos)
        {
            if (go != null)
            {
                photonPlayerController ppc = go.GetComponent<photonPlayerController>();

                if (ppc.checkOwner())
                {
                    ppc.recieveMessage(externalClock);
                }
            }
            

        }
        //clock.ReceiveMessage(externalClock);
    }

    public void recieveMessage(Dictionary<int, int> externalClock)
    {
        clock.ReceiveMessage(externalClock);
    }
    
    public void loseGame()
    {
        if (myPV.IsMine)
        {
            myPM.loseGame();
        }
    }
    
}
