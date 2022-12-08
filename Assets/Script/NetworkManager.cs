using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    bool readyToCreate = false;
    [SerializeField]
    TMP_InputField inputName;
    [SerializeField]
    Button startMale,startFemale;
    [SerializeField]
    Slider slider;
    [SerializeField]
    GameObject loadingPanel;
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void ConnectToServer(){
        PhotonNetwork.ConnectUsingSettings();
        readyToCreate = false;
        print("Try to connect...");
    }
    public override void OnConnectedToMaster()
    {
        print("connected to server");

        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("Joined Lobby");
        startMale.interactable = true;
        startFemale.interactable = true;
        readyToCreate = true;
    }
    public void JoinRoom(bool isMale){
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Gameplay",roomOptions,TypedLobby.Default);
        Hashtable hash = new Hashtable();
        hash.Add("coin", 0);
        hash.Add("gender", isMale);
        PhotonNetwork.LocalPlayer.NickName = inputName.text;
        PhotonNetwork.SetPlayerCustomProperties(hash);

        StartCoroutine(LoadLevel(1));
    }
    IEnumerator LoadLevel(int index){
        PhotonNetwork.LoadLevel(1);
        loadingPanel.SetActive(true);
        while(PhotonNetwork.LevelLoadingProgress < 1){
            float progress = Mathf.Clamp01(PhotonNetwork.LevelLoadingProgress/.9f);
            slider.value = progress;
            yield return null;
        }
    }
    public override void OnJoinedRoom(){
        print("Join room "+PhotonNetwork.CurrentRoom.Name);
        base.OnJoinedRoom();
    }
    public void LeaveRoom(){
        PhotonNetwork.LoadLevel(0);

        PhotonNetwork.LeaveRoom();
    }
    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }
}
