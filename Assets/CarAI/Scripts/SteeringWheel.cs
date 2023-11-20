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
    public AudioClip beep;

    private bool allowFlashing = true;

    private bool flashingGreen = false;
    private bool flashingRed = false;
    private bool flashingRedFaster = false;
    private bool stayRed = false;

    private float flashingTimer = 0f;
    private float lastTimerValue = 0f;
    private bool isGreen = false; //used for flashing green
    private AudioSource audioSource;

    private bool playedFinalSound = false;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audio.clip = beep;

        imgBlue.enabled = false;
        imgRed.enabled = true;
        imgGreen.enabled = false;
        imgWhite.enabled = false;
        isImgOn = true;
    }

    public void AllowFlashing(){
        allowFlashing = true;
    }
    public void DisallowFlashing()
    {
        flashingGreen = false;
        flashingRed = false;
        flashingRedFaster = false;
        allowFlashing = false;
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

    public void FlashingGreen()
    {
        flashingGreen = true;
        //Debug.Log("true");
    }
    public void FlashingRed()
    {
        flashingGreen = false;
        flashingRed = true;
        //Debug.Log("true");
    }
    public void FlashingRedFaster()
    {
        flashingRed = false;
        flashingRedFaster = true;
        //Debug.Log("true");
    }
    public void StayRed()
    {
        if (!playedFinalSound)
        {
            audioSource.clip = beep;
            audioSource.Play();
            playedFinalSound = true;
        }
        

        flashingRedFaster = false;
        //stayRed = true;
        //Debug.Log("true");
    }


    void Update()
    {
        if (allowFlashing)
        {
            if (flashingGreen)
            {
                //Debug.Log("here");
                flashingTimer += Time.deltaTime;
                if (flashingTimer - lastTimerValue >= 4f)
                {
                    lastTimerValue = flashingTimer;
                    //Debug.Log("Last timer value:" + lastTimerValue + "flashing timer" + flashingTimer);
                    if (isGreen)
                    {
                        WhiteLight();
                        isGreen = false;
                    }
                    else
                    {

                        GreenLight();
                        isGreen = true;
                    }
                }
            }
            else if (flashingRed)
            {
                //Debug.Log("here");
                flashingTimer += Time.deltaTime;
                if (flashingTimer - lastTimerValue >= 4f)
                {
                    //GetComponent<AudioSource>().Play();
                    audioSource.Play();
                    lastTimerValue = flashingTimer;
                    //Debug.Log("Last timer value:" + lastTimerValue + "flashing timer" + flashingTimer);
                    if (isGreen)
                    {
                        WhiteLight();
                        isGreen = false;
                    }
                    else
                    {

                        RedLight();
                        isGreen = true;
                    }
                }
            }
            else if (flashingRedFaster)
            {
                //Debug.Log("here");
                flashingTimer += Time.deltaTime;
                if (flashingTimer - lastTimerValue >= 1.8f)
                {
                    audioSource.Play();
                    lastTimerValue = flashingTimer;
                    //Debug.Log("Last timer value:" + lastTimerValue + "flashing timer" + flashingTimer);
                    if (isGreen)
                    {
                        WhiteLight();
                        isGreen = false;
                    }
                    else
                    {

                        RedLight();
                        isGreen = true;
                    }
                }
            }
        }
        


    }
}