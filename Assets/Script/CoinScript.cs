using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CoinScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && PhotonNetwork.IsMasterClient){
            PhotonView photonView = other.GetComponent<PhotonView>();
            int currentCoin = int.Parse(photonView.Owner.CustomProperties["coin"].ToString());
            photonView.Owner.SetCustomProperties(
                new Hashtable() {{"coin", ++currentCoin }}
            );
            DestroyCoin();
        }
    }
    void DestroyCoin(){
        GameObject[] coin = GameObject.FindGameObjectsWithTag("Coin");
        if(coin.Length <= 1){
            GameplayController.instance.GameEnd();
        }
        Destroy(gameObject);
    }
}
