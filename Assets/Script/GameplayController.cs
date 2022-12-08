using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameplayController : MonoBehaviour
{
    public static GameplayController instance; 
    [SerializeField]
    GameObject _gameplayPanel;
    [SerializeField]
    GameObject _gameplayEndPanel;
    [SerializeField]
    TextMeshProUGUI _onlineText;
    [SerializeField]
    TextMeshProUGUI _coinCollectText;
    [SerializeField]
    Transform _coinsContainer;
    [SerializeField]
    GameObject _coinPrefabs;
    private void Awake() {
        if(instance == null){
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    public void ShowGameplayPanel(){
        _gameplayPanel.SetActive(true);
    }
    public void UpdateOnline(int countPlayers){
        _onlineText.text = "Online : "+countPlayers;
    }
    public void StartGameplay(){
        GetComponent<PhotonView>().RPC("StartGame",RpcTarget.AllBuffered);
    }
    public void GameEnd(){
        _gameplayEndPanel.SetActive(true);
        _gameplayPanel.SetActive(true);

        Player[] players = PhotonNetwork.PlayerList;
        string name = "";
        int maxScore = 0;
        for (int i = 0; i < players.Length; i++)
        {
            int score = int.Parse(players[i].CustomProperties["coin"].ToString());
            if(maxScore < score){
                name = players[i].NickName;
                maxScore = score;
            }
            players[i].SetCustomProperties(
                new Hashtable() {{"coin", 0 }}
            );
        }
        _gameplayEndPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Highscore is = "+name+"\n Score is = "+maxScore;
    }
    [PunRPC]
    public void StartGame(){
        print(_coinsContainer.childCount);
        for (int i = 0; i < _coinsContainer.childCount; i++)
        {
            Instantiate(_coinPrefabs,_coinsContainer.GetChild(i).position,new Quaternion());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
