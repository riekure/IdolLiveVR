using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraController : MonoBehaviour 
{
    public GameObject MainCamera;
    public GameObject DiveCamera;

    public void OnClickMainCameraButton()
    {
        MainCamera.SetActive(false);
        DiveCamera.SetActive(true);
    }

    public void OnClickDiveCameraButton()
    {
        MainCamera.SetActive(true);
        DiveCamera.SetActive(false);
    }
}
