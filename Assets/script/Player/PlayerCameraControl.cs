using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class PlayerCameraControl : NetworkBehaviour
{
    public GameObject CamPlayer, CamMap;
    private bool IsCamMap = false;
    void Update()
    {
        if (IsOwner)
        {
            checkInputCamera();
            changeCamera();
        }
        else
        {
            setActiveAllCamera(false);
        }
    }

    private void setActiveAllCamera(bool IsActive)
    {
        CamMap.SetActive(IsActive);
        CamPlayer.SetActive(IsActive);
    }

    private void checkInputCamera()
    {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsCamMap = !IsCamMap;
            }
    }

    private void changeCamera()
    {
        if (IsCamMap)
        {
            CamMap.SetActive(true);
        }
        else
        {
            CamMap.SetActive(false);
        }
    }
}
