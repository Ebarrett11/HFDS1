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
    public AudioClip end;
    public AudioClip chime;

    public int test;


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

    private bool chimePlayed = false;
    private float defaultVolume = 0f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audio.clip = beep;

        imgBlue.enabled = false;
        imgRed.enabled = true;
        imgGreen.enabled = false;
        imgWhite.enabled = false;
        isImgOn = true;
        defaultVolume = audioSource.volume;
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
        chimePlayed = false;

    }

    public void GreenLight()
    {

        audioSource.clip = chime;
        if (!chimePlayed)
        {
            audioSource.Play();
            chimePlayed = true;
        }

        imgBlue.enabled = false;
        imgRed.enabled = false;
        imgGreen.enabled = true;
        imgWhite.enabled = false;
        isImgOn = true;

    }

    public void GreenLightPhase()
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

        chimePlayed = false;
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

        audioSource.clip = beep;
        flashingGreen = false;
        flashingRed = true;
        //Debug.Log("true");
    }
    public void FlashingRedFaster()
    {
        audioSource.volume = defaultVolume;

        flashingRed = false;
        flashingRedFaster = true;
        //audioSource.clip = voicePrompt;
        //Debug.Log("true");
    }
    public void StayRed()
    {
        audioSource.volume = defaultVolume;

        if (!playedFinalSound)
        {
            audioSource.clip = end;
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

                        GreenLightPhase();
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