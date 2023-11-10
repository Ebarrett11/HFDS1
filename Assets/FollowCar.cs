using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public GameObject car;
    private Vector3 self_starting_coords;
    private Vector3 starting_coords;

    // Start is called before the first frame update
    void Start()
    {
        self_starting_coords = transform.position;
        starting_coords = car.transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = self_starting_coords + (car.transform.position-starting_coords);
        //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, car.transform.rotation.y, transform.rotation.z));

    }
}
