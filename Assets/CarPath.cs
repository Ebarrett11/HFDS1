using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPath : MonoBehaviour
{
    public GameObject cube_pos;
    public GameObject cube_pos2;
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
        if(count == 0)
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
