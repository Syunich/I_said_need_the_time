using UnityEngine;
using Photon.Pun;
using TMPro;

public class Player_Init : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshPro tmp;
    void Start()
    {

        tmp.text = photonView.Owner.NickName;

    }

}
