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

        clock = new VectorClock(myPM.ID);
    }


    // Update is called once per frame
    void Update()
    {
        if (!myPV.IsMine)
        {
            return;
        }

        if (Input.GetButtonDown(KeyCode.E)){ 
            clock.Tick();
        }

        if (Input.GetButtonDown(KeyCode.A)){
            clock.Tick();
            photonView.RPC("RPC_RecieveMessage", RpcTarget.Others, clock.Clock());
        }

        if (Input.GetButtonDown(KeyCode.P)){
            clock.DisplayClock();
            
        }

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
        clock.RecieveMessage(externalClock);
    }
    
    public void loseGame()
    {
        if (myPV.IsMine)
        {
            myPM.loseGame();
        }
    }
    
}
