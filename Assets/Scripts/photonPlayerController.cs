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

    [SerializeField]
    GameObject UICanvas;

    [SerializeField]
    GameObject displayObject;

    [SerializeField]
    TMP_Text delayDisplay;

    [SerializeField]
    TMP_Text actionDisplay;


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
        delay = 0;
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
            printDisplay();
            StartCoroutine(setActionText("Action: Ticking Display"));
        }

        if (Input.GetKeyDown(KeyCode.A)){
            clock.Tick();
            WaitDelay();
            printDisplay();
            StartCoroutine(setActionText("Action: Multicast"));
            StartCoroutine(waitMulticast());
            
        }

        if (Input.GetKeyDown(KeyCode.M)){
            clock.Tick();
            WaitDelay();
            
            printDisplay();
            StartCoroutine(setActionText("Action: Master Client"));
            StartCoroutine(waitMastercast());

        }

        if (Input.GetKeyDown(KeyCode.P)){
            printDisplay();
            
        }

        if (Input.GetKeyDown(KeyCode.D)){
            randomDelay();
            delayDisplay.text = "Delay: " + delay;
            StartCoroutine(setActionText("Action: Random Delay"));
        }

        if (Input.GetKeyDown(KeyCode.R)){
            delay = 0;
            delayDisplay.text = "Delay: " + delay;
            StartCoroutine(setActionText("Action: Reset Delay"));
        }

        if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log(delay);
            delayDisplay.text = "Delay: " + delay;
        }

    }

    IEnumerator waitMulticast()
    {
        yield return new WaitForSeconds(delay / 1000f);
        myPV.RPC("RPC_RecieveMessage", RpcTarget.Others, clock.Clock());
    }

    IEnumerator waitMastercast()
    {
        yield return new WaitForSeconds(delay / 1000f);
        myPV.RPC("RPC_RecieveMessage", RpcTarget.MasterClient, clock.Clock());
    }

    public void printDisplay()
    {
        List<string> st = clock.DisplayClock();

        //clear children
        foreach (Transform go in UICanvas.transform)
        {
            Destroy(go.gameObject);
        }

        foreach (string s in st)
        {
            GameObject go = Instantiate(displayObject, UICanvas.transform);

            processDisplay pd = go.GetComponent<processDisplay>();

            if (pd != null)
            {
                pd.setProcNum(s);
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
        int randomNumber = random.Next(5000, 10001);
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
        Debug.Log("Length: "+gos.Length);
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
        
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Game Manager");
        Debug.Log("Length: "+gos.Length);
        int clock_ID = clock.MyID();
        foreach (GameObject go in gos)
        {
            if (go != null)
            {
                photonPlayerController ppc = go.GetComponent<photonPlayerController>();

                if (ppc.checkOwner())
                {
                    ppc.clock.Tick();
                    ppc.actionDisplay.text = "";
                    ppc.actionDisplay.text = "Recieved Message from: " + clock_ID;
                    ppc.recieveMessage(externalClock);
                    
                }
            }
            

        }
        //clock.ReceiveMessage(externalClock);
    }
    IEnumerator setActionText(string s)
    {
        actionDisplay.text = "";
        yield return new WaitForSeconds(.25f);
        actionDisplay.text = s;
    }

    public void addClock(int ID)
    {
        clock.Add(ID);
        printDisplay();
    }
    public void recieveMessage(Dictionary<int, int> externalClock)
    {
        clock.ReceiveMessage(externalClock);
        printDisplay();
    }
    
    public void loseGame()
    {
        if (myPV.IsMine)
        {
            myPM.loseGame();
        }
    }
    
}
