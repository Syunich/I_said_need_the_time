using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class SaveManager : MonoBehaviourPunCallbacks
{
    [SerializeField] float savespan;
    private Transform Playertransform;
    float deltatime;

    
    public override void OnJoinedRoom()
    {
        Playertransform = GameManager.instance.PlayerObj.transform;
      //  Debug.Log(Playertransform.name + Playertransform.position.ToString());
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.playerinfo.CanControll || !GameManager.instance.IsLoaded)
            return;

        deltatime += Time.deltaTime;
        if(deltatime < savespan)
        {
     //       Debug.Log($"save called_before: {GameManager.instance.PlayerObj.transform.position}");
            deltatime = 0;
            PlayerPrefs.SetFloat("passedtime", GameManager.instance.playerinfo.Passedtime);
            PlayerPrefsUtils.SetObject("playerposition", Playertransform.position);
     //       Debug.Log($"save called_after: {GameManager.instance.PlayerObj.transform.position}");
        }
    }
}
