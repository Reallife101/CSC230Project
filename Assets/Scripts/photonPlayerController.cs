using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using VectorClockNamespace;
using System.Threading.Tasks;

public class photonPlayerController : MonoBehaviour
{
    //Components
    private PlayerManager myPM;
    private PhotonView myPV;
    private VectorClock clock;
    private int delay;
    private List<int> process_list;
    private GameObject[] allGameObjects;

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
        myPV.RPC("RPC_AddClock", RpcTarget.Others, myPV.ViewID);
        delay = 0;
        process_list = getProcessList();
        allGameObjects = GameObject.FindGameObjectsWithTag("Game Manager");
        Debug.Log("Length: "+allGameObjects.Length);
        
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

        if (Input.GetKeyDown(KeyCode.D)){
            randomDelay();   
        }

        if (Input.GetKeyDown(KeyCode.R)){
            delay = 0;   
        }

        if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log(delay);   
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            clock.Tick();
            myPV.RPC("RPC_RecieveMessage", allGameObjects[0].GetComponent<PhotonView>().Owner, clock.Clock());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){
            if (allGameObjects.Length >= 2){
                clock.Tick();
                myPV.RPC("RPC_RecieveMessage", allGameObjects[1].GetComponent<PhotonView>().Owner, clock.Clock());
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)){
            if (allGameObjects.Length >= 3){
                clock.Tick();
                myPV.RPC("RPC_RecieveMessage", allGameObjects[2].GetComponent<PhotonView>().Owner, clock.Clock());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)){
            if (allGameObjects.Length >= 4){
                clock.Tick();
                myPV.RPC("RPC_RecieveMessage", allGameObjects[3].GetComponent<PhotonView>().Owner, clock.Clock());
            }
        }

    }

    public List<int> getProcessList()
    {
        return new List<int>(clock.Clock().Keys);
    }

    public bool checkOwner()
    {
        return myPV.IsMine;
    }

    public void randomDelay()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(200, 2001);
        delay = randomNumber;

    }

    public async void WaitDelay()
    {
        await Task.Delay(delay);
    }


    [PunRPC]
    void RPC_SetScore(int sc)
    {
        //scoreText.text = "Score: " + sc;
    }

    [PunRPC]
    void RPC_AddClock(int ID)
    {   
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Game Manager");
        foreach (GameObject go in gos)
        {
            if (go != null)
            {
                photonPlayerController ppc = go.GetComponent<photonPlayerController>();

                if (ppc.checkOwner())
                {
                    ppc.addClock(ID);
                }
            }
            

        }
    }

    [PunRPC]
    void RPC_RemoveClock(int ID)
    {
        clock.Remove(ID);
    }

    [PunRPC]
    void RPC_RecieveMessage(Dictionary<int, int> externalClock)
    {
        WaitDelay();
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

    
    public void addClock(int ID)
    {
        clock.Add(ID);
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
