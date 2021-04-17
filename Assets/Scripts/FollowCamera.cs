using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    public GameObject tPlayer;
    public CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (tPlayer == null)
        {
            tPlayer = GameObject.FindWithTag("Player");

            if (tPlayer != null)
            {
                vcam.LookAt = tPlayer.transform;
                vcam.Follow = tPlayer.transform;
            }
        }
    }
}
