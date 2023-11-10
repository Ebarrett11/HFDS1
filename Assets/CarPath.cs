using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPath : MonoBehaviour
{
    public GameObject cube_pos;
    public GameObject cube_pos2;
    public GameObject cube_pos3;
    public GameObject cube_pos4;
    public GameObject cube_pos5;
    public GameObject cube_pos6;
    public GameObject cube_pos7;

    private int count = 0;
    //public Vector3 second_pos = new Vector3(-124f, 5f, -491f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (count)
        {
            case 0:
                transform.position = cube_pos.transform.position;
                break;
            case 1:
                transform.position = cube_pos2.transform.position;
                break;
            case 2:
                transform.position = cube_pos3.transform.position;
                break;
            case 3:
                transform.position = cube_pos4.transform.position;
                break;
            case 4:
                transform.position = cube_pos5.transform.position;
                break;
            case 5:
                transform.position = cube_pos6.transform.position;
                break;
            case 6:
                transform.position = cube_pos7.transform.position;
                break;
            default:
                break;
        }
        if (count == 0)
        {
            transform.position = cube_pos.transform.position;
        }
        else if(count == 1)
        {
            transform.position = cube_pos2.transform.position;
        }

        count++;
    }
    
}
