using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttentionManagement : MonoBehaviour
{
    public bool isImgOn;
    public Image eyeOpen;
    public Image eyeClosed;


    void Start()
    {
        eyeOpen.enabled = true;
        eyeClosed.enabled = false;
        isImgOn = true;
    }

    public void OpenEye()
    {
        eyeOpen.enabled = true;
        eyeClosed.enabled = false;
        isImgOn = true;

    }

    public void CloseEye()
    {
        eyeOpen.enabled = false;
        eyeClosed.enabled = true;
        isImgOn = true;

    }


    void Update()
    {

    }
}