using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NameEdit :MonoBehaviourPunCallbacks
{
    [SerializeField] InputField NameText;
    // Start is called before the first frame update
    void Awake()
    {
        //名前のデータが存在する場合はテキストを変更しておく
        if (PlayerPrefs.HasKey("name"))
        {
            NameText.text = PlayerPrefs.GetString("name");
            PhotonNetwork.LocalPlayer.NickName = NameText.text;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = $"Pendulum({Random.Range(0 , 1024)})";
        }
    }

   public void TextChanged()
    {
        PlayerPrefs.SetString("name", NameText.text);
        PhotonNetwork.LocalPlayer.NickName = NameText.text;
    }
}