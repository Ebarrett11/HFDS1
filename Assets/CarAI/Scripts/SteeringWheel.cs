using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageShow : MonoBehaviour
{

    public bool isImgOn;
    public Image imgWhite;
    public Image imgRed;
    public Image imgBlue;
    public Image imgGreen;


    void Start()
    {
        imgBlue.enabled = false;
        imgRed.enabled = true;
        imgGreen.enabled = false;
        imgWhite.enabled = false;
        isImgOn = true;
    }

    public void WhiteLight()
    {
        imgBlue.enabled = false;
        imgRed.enabled = false;
        imgGreen.enabled = false;
        imgWhite.enabled = true;
        isImgOn = true;

    }

    public void GreenLight()
    {
        imgBlue.enabled = false;
        imgRed.enabled = false;
        imgGreen.enabled = true;
        imgWhite.enabled = false;
        isImgOn = true;

    }

    public void BlueLight()
    {
        imgBlue.enabled = true;
        imgRed.enabled = false;
        imgGreen.enabled = false;
        imgWhite.enabled = false;
        isImgOn = true;
    }

    public void RedLight()
    {
        imgBlue.enabled = false;
        imgRed.enabled = true;
        imgGreen.enabled = false;
        imgWhite.enabled = false;
        isImgOn = true;
    }

    void Update()
    {

    }
}