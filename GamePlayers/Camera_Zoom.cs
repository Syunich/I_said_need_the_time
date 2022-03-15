using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class Camera_Zoom : MonoBehaviourPunCallbacks
{
    [SerializeField] Camera PlayerCamera;
    float NormalSize;
    // Start is called before the first frame update
    void Start()
    {
        NormalSize = PlayerCamera.orthographicSize;
        if (photonView.IsMine)
        {
            PlayerCamera.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "huriko")
        {
            PlayerCamera.DOOrthoSize(15f, 1.0f).SetEase(Ease.OutSine).Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "huriko")
        {
            PlayerCamera.DOOrthoSize(NormalSize, 1.0f).SetEase(Ease.OutSine).Play();
        }
    }
}