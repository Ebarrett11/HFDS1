using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{
    public Scene scene1;
    public Scene scene2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {            
            SceneManager.LoadScene("Scene1");
        }
        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene("Scene2");
        }
    }
}