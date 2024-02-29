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
    }


    // Update is called once per frame
    void Update()
    {
        if (!myPV.IsMine)
        {
            return;
        }

    }


    [PunRPC]
    void RPC_SetScore(int sc)
    {
        //scoreText.text = "Score: " + sc;
    }
    
    public void loseGame()
    {
        if (myPV.IsMine)
        {
            myPM.loseGame();
        }
    }
    
}
