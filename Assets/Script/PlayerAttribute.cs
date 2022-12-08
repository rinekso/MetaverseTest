using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerAttribute : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject male;
    [SerializeField]
    GameObject female;
    [SerializeField]
    Avatar maleBone;
    [SerializeField]
    Avatar femaleBone;
    public bool isMale;
    Animator animator;
    [SerializeField]
    Transform geometry;
    [SerializeField]
    TextMeshProUGUI nameText;
    PhotonView photonView;
    private void Awake() {
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        SetPlayerProperties(photonView.Owner.CustomProperties);        
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    public void ChangeName(string name){
        nameText.text = name;
    }
    async void SetPlayerProperties(Hashtable changeProps){
        ChangeName(photonView.Owner.NickName);
        if(changeProps.ContainsKey("gender")){
            if(changeProps["gender"].ToString() == "True"){
                Instantiate(male,geometry);
                animator.avatar = maleBone;
                // StartCoroutine(SetAvatar(maleBone));
            }
            else{
                Instantiate(female,geometry);
                animator.avatar = femaleBone;
                // StartCoroutine(SetAvatar(femaleBone));
            }
        }

        if(changeProps.ContainsKey("coin") && photonView.Owner.IsLocal){
            GameObject.Find("CoinAlert").GetComponent<TextMeshProUGUI>().text = "Coin collected : "+changeProps["coin"].ToString();
        }
        
    }
    IEnumerator SetAvatar(Avatar avatar){
        yield return new WaitForSeconds(.5f);
        animator.avatar = avatar;
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(photonView.Owner == targetPlayer)
            SetPlayerProperties(changedProps);
        // print("hp change = "+int.Parse(targetPlayer.CustomProperties["HP"].ToString()));
        base.OnPlayerPropertiesUpdate(targetPlayer,changedProps);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
