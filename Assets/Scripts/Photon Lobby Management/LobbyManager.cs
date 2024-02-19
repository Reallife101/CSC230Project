using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //Room Creation
    public TMP_InputField roomInputField;
    public TMP_Text roomName;

    //GameObjects of the lobby and room
    public GameObject lobbyPanel;
    public GameObject roomPanel;

    public roomItem roomItemPrefab;
    List<roomItem> roomItemsList = new List<roomItem>(); //List of rooms created.
    public Transform contentObject; //Transform for where to put the rooms in the screen

    //Time for updates between refreses for the room list
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;


    //PlayerItems
    List<PlayerItem> playerItemsList = new List<PlayerItem>(); //List of playeritems created.
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;



    private void Start()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = true;
                PhotonNetwork.CurrentRoom.IsVisible = true;
            }
            OnJoinedRoom();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }


    private void Update()
    {
        //Activates PlayButton there is enough players and all players in the room are ready
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            playButton.SetActive(true);

        }
        else
        {
            playButton.SetActive(false);
        }

    }


    public void OnClickPlayButton()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        StartCoroutine(load());
        //PhotonNetwork.LoadLevel(levelInfo.getSceneName());
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickCreate()
    {
        // Creates a new room when there is text in roomInputField
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions(){MaxPlayers = 2, BroadcastPropsChangeToAll = true, DeleteNullProperties = true});
        }
    }

    IEnumerator load()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(2);
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: "+PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public void JoinRoom(string roomName)
    {
        Debug.Log("JOIN ROOM");
        PhotonNetwork.JoinRoom(roomName);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {        
        
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
        
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        // Destroy old room items and clear list
        foreach (roomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        //Creates new room items to show the room that was recently removed or created
        foreach (RoomInfo room in list)
        {
            if (room.IsOpen && room.IsVisible)
            {
                roomItem newRoom = Instantiate(roomItemPrefab, contentObject);
                newRoom.SetRoomName(room.Name);
                roomItemsList.Add(newRoom);
            }
        }
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }

        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem playerItem = Instantiate(playerItemPrefab, playerItemParent);
            playerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                playerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(playerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

}
