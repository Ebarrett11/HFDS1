using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CarAI : MonoBehaviour
{
    [Header("Car Wheels (Wheel Collider)")]// Assign wheel Colliders through the inspector
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;

    [Header("Car Wheels (Transform)")]// Assign wheel Transform(Mesh render) through the inspector
    public Transform wheelFL;
    public Transform wheelFR;
    public Transform wheelBL;
    public Transform wheelBR;

    [Header("Car Front (Transform)")]// Assign a Gameobject representing the front of the car
    public Transform carFront;

    [Header("General Parameters")]// Look at the documentation for a detailed explanation
    public List<string> NavMeshLayers;
    public int MaxSteeringAngle = 45;
    public int MaxRPM = 150;

    [Header("Debug")]
    public bool ShowGizmos;
    public bool Debugger;

    [Header("Destination Parameters")]// Look at the documentation for a detailed explanation
    public bool Patrol = true;
    public Transform CustomDestination;
    public GameObject steeringWheel;
    public GameObject speedText;
    public GameObject attentionSystem;
    public bool ConstructionZone = false;
    public bool Attention = true;

    [HideInInspector] public bool move;// Look at the documentation for a detailed explanation

    private Vector3 PostionToFollow = Vector3.zero;
    private int currentWayPoint;
    private float AIFOV = 60;
    private bool allowMovement;
    private int NavMeshLayerBite;
    private List<Vector3> waypoints = new List<Vector3>();
    private float LocalMaxSpeed;
    private int Fails;
    private float MovementTorque = 1;
    private bool hfds = false;
    private bool hfdsTimerBlueLight = false;

    private Rigidbody rb;
    private bool allowHFDS = true;
    public float val = 20f;

    private int attentionPhase = 0;
    private float attentionTimer = 0f;
    private float manualTimer = 0f;

    public Text actualSpeedText;
    public Text metricSpeedText;

    private AudioSource audioSource;
    void Awake()
    {
        currentWayPoint = 0;
        allowMovement = true;
        move = true;
    }

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        CalculateNavMashLayerBite();
        rb = GetComponent<Rigidbody>();
        //actualSpeedText = speedText.GetComponent<Text>();
        //metricSpeedText = speedText.GetComponent<Text>();

        audioSource = GetComponent<AudioSource>();
        Attention = true;
        hfds = false;
    }

    void PhaseOne()
    {
        Debug.Log("Flashing Green");
    }

    void Update()
    {
        audioSource.pitch = val * rb.velocity.magnitude;

        actualSpeedText.text = Mathf.RoundToInt(rb.velocity.magnitude*2.98f).ToString();
        metricSpeedText.text = Mathf.RoundToInt(rb.velocity.magnitude * 2.98f* 1.60934f).ToString();

        //Debug.Log(rb.velocity.magnitude);

        //attention section
        if (Input.GetKeyDown("i"))
        {
            Debug.Log("Attention");

            if (Attention)
            {
                Attention = false;
                attentionSystem.GetComponent<AttentionManagement>().CloseEye();
                Debug.Log("Close");

            }
            else
            {
                Attention = true;
                attentionSystem.GetComponent<AttentionManagement>().OpenEye();
                attentionTimer = 0f;
                Debug.Log("Open");

            }

        }
      
        //Level 1: if driver is not engaged for 3 seconds, the wheel light indicator begins flashing green

        //Level 2: After level 1, if driver is not engaged for 3 more seconds, the wheel light indicator begins flashing red with beeping audio queues(1 every second) and continuous seat vibration

        //Level 3(Final): After level 2, if driver is not engaged for 3 more seconds, the system will begin flashing red faster on the wheel light indicator, seat will still continuously vibrate, and an audio queue will tell driver to reengage, or system will abort

        //After level warning level 3, the driver will have 3 seconds to reengage, or system will abort




        if (Input.GetKeyDown("m"))
        {
            if (hfds)
            {
                hfds = false;
                hfdsTimerBlueLight = false;

            }
            else if (!hfds && rb.velocity.magnitude > 20 && allowHFDS)
            {
                hfds = true;
                hfdsTimerBlueLight = true;
            }
            Debug.Log(hfds);
        }
        if (Input.GetKeyDown("w") || Input.GetKeyDown("s"))
        {
            hfds = false;
        }

        if (hfdsTimerBlueLight)
        {
            Debug.Log("true");
            if (Input.GetKey("w") || Input.GetKey("s"))
            {
                Debug.Log(manualTimer);
                manualTimer += Time.deltaTime;
                if (manualTimer >= 60f)
                {
                    Debug.Log("Turn on Manual");
                    hfdsTimerBlueLight = false;
                    manualTimer = 0f;

                }

            }
        }
        
        if (Input.GetKeyUp("w") || Input.GetKeyUp("s"))
        {
            if (hfdsTimerBlueLight)
            {
                hfds = true;
            }
            manualTimer = 0f;
        }

        if (!Attention && hfds)
        {
            steeringWheel.GetComponent<ImageShow>().AllowFlashing();

            attentionTimer += Time.deltaTime;
            if(attentionTimer <= 36)
            {
                steeringWheel.GetComponent<ImageShow>().GreenLight();

            }
            if (attentionTimer >= 36 && attentionTimer < 72)
            {
                steeringWheel.GetComponent<ImageShow>().FlashingGreen();
                //Debug.Log("Flashing Green");
            }
            else if (attentionTimer >= 72 && attentionTimer < 108)
            {
                steeringWheel.GetComponent<ImageShow>().FlashingRed();
            }
            else if (attentionTimer >= 108 && attentionTimer < 144)
            {
                steeringWheel.GetComponent<ImageShow>().FlashingRedFaster();
            }
            else if (attentionTimer >= 144 && attentionTimer < 180)
            {
                allowHFDS = false;
                hfds = false;
                hfdsTimerBlueLight = false;

                steeringWheel.GetComponent<ImageShow>().StayRed();
        
                steeringWheel.GetComponent<ImageShow>().RedLight();

            }
        }
        else if(!Attention && !hfds)
        {
            if (!hfds && rb.velocity.magnitude > 20)
            {
                if (!hfds && hfdsTimerBlueLight)
                {
                    steeringWheel.GetComponent<ImageShow>().BlueLight();
                    Attention = true;
                    attentionSystem.GetComponent<AttentionManagement>().OpenEye();
                    attentionTimer = 0f;

                }
                else
                {
                    steeringWheel.GetComponent<ImageShow>().WhiteLight();
                }
            }  
            
        }
        else
        {
            steeringWheel.GetComponent<ImageShow>().DisallowFlashing();
            if (ConstructionZone)
            {
                steeringWheel.GetComponent<ImageShow>().RedLight();
            }
            else if (!hfds && rb.velocity.magnitude > 20 && allowHFDS)
            {
                if(!hfds && hfdsTimerBlueLight)
                {
                    steeringWheel.GetComponent<ImageShow>().BlueLight();

                }
                else
                {
                    steeringWheel.GetComponent<ImageShow>().WhiteLight();
                }
            }
            else if (hfds)
            {
                steeringWheel.GetComponent<ImageShow>().GreenLight();
            }
            else if (!hfds)
            {
                steeringWheel.GetComponent<ImageShow>().RedLight();
            }
            if (Input.GetKeyDown("s"))
            {
                ApplyBrakes();
            }
        }
        

        
    }

    void FixedUpdate()
    {

        //if (!hfds)
        //{
        if (Input.GetKey("w") || hfds)
        {
            //Debug.Log("W");
            move = true;

        }
        else
        {
            move = false;
        }
            
            //else
            //{
            //    move = false;
            //}
        //}
        //else
        //{
        //    move = true;
        //}

        UpdateWheels();
        ApplySteering();
        PathProgress();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            hfds = false;
            allowHFDS = false;
            move = false;
            allowMovement = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Construction")
        {
            hfds = false;
            hfdsTimerBlueLight = false; ;
            ConstructionZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Construction")
        {
            ConstructionZone = false;
        }
    }



    private void CalculateNavMashLayerBite()
    {
        if (NavMeshLayers == null || NavMeshLayers[0] == "AllAreas")
            NavMeshLayerBite = NavMesh.AllAreas;
        else if (NavMeshLayers.Count == 1)
            NavMeshLayerBite += 1 << NavMesh.GetAreaFromName(NavMeshLayers[0]);
        else
        {
            foreach (string Layer in NavMeshLayers)
            {
                int I = 1 << NavMesh.GetAreaFromName(Layer);
                NavMeshLayerBite += I;
            }
        }
    }

    private void PathProgress() //Checks if the agent has reached the currentWayPoint or not. If yes, it will assign the next waypoint as the currentWayPoint depending on the input
    {
        wayPointManager();
        Movement();
        ListOptimizer();

        void wayPointManager()
        {
            if (currentWayPoint >= waypoints.Count)
                allowMovement = false;
            else
            {
                PostionToFollow = waypoints[currentWayPoint];
                allowMovement = true;
                if (Vector3.Distance(carFront.position, PostionToFollow) < 2)
                    currentWayPoint++;
            }

            if (currentWayPoint >= waypoints.Count - 3)
                CreatePath();
        }

        void CreatePath()
        {
            if (CustomDestination == null)
            {
                if (Patrol == true)
                    RandomPath();
                else
                {
                    debug("No custom destination assigned and Patrol is set to false", false);
                    allowMovement = false;
                }
            }
            else
               CustomPath(CustomDestination);
            
        }

        void ListOptimizer()
        {
            if (currentWayPoint > 1 && waypoints.Count > 30)
            {
                waypoints.RemoveAt(0);
                currentWayPoint--;
            }
        }
    }

    public void RandomPath() // Creates a path to a random destination
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 sourcePostion;

        if (waypoints.Count == 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 100;
            randomDirection += transform.position;
            sourcePostion = carFront.position;
            Calculate(randomDirection, sourcePostion, carFront.forward, NavMeshLayerBite);
        }
        else
        {
            sourcePostion = waypoints[waypoints.Count - 1];
            Vector3 randomPostion = Random.insideUnitSphere * 100;
            randomPostion += sourcePostion;
            Vector3 direction = (waypoints[waypoints.Count - 1] - waypoints[waypoints.Count - 2]).normalized;
            Calculate(randomPostion, sourcePostion, direction, NavMeshLayerBite);
        }

        void Calculate(Vector3 destination, Vector3 sourcePostion, Vector3 direction, int NavMeshAreaByte)
        {
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 150, 1 << NavMesh.GetAreaFromName(NavMeshLayers[0])) &&
                NavMesh.CalculatePath(sourcePostion, hit.position, NavMeshAreaByte, path) && path.corners.Length > 2)
            {
                if (CheckForAngle(path.corners[1], sourcePostion, direction))
                {
                    waypoints.AddRange(path.corners.ToList());
                    debug("Random Path generated successfully", false);
                }
                else
                {
                    if (CheckForAngle(path.corners[2], sourcePostion, direction))
                    {
                        waypoints.AddRange(path.corners.ToList());
                        debug("Random Path generated successfully", false);
                    }
                    else
                    {
                        debug("Failed to generate a random path. Waypoints are outside the AIFOV. Generating a new one", false);
                        Fails++;
                    }
                }
            }
            else
            {
                debug("Failed to generate a random path. Invalid Path. Generating a new one", false);
                Fails++;
            }
        }
    }

    public void CustomPath(Transform destination) //Creates a path to the Custom destination
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 sourcePostion;

        if (waypoints.Count == 0)
        {
            sourcePostion = carFront.position;
            Calculate(destination.position, sourcePostion, carFront.forward, NavMeshLayerBite);
        }
        else
        {
            sourcePostion = waypoints[waypoints.Count - 1];
            Vector3 direction = (waypoints[waypoints.Count - 1] - waypoints[waypoints.Count - 2]).normalized;
            Calculate(destination.position, sourcePostion, direction, NavMeshLayerBite);
        }

        void Calculate(Vector3 destination, Vector3 sourcePostion, Vector3 direction, int NavMeshAreaBite)
        {
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 150, NavMeshAreaBite) &&
                NavMesh.CalculatePath(sourcePostion, hit.position, NavMeshAreaBite, path))
            {
                if (path.corners.ToList().Count() > 1&& CheckForAngle(path.corners[1], sourcePostion, direction))
                {
                    waypoints.AddRange(path.corners.ToList());
                    debug("Custom Path generated successfully", false);
                }
                else
                {
                    if (path.corners.Length > 2 && CheckForAngle(path.corners[2], sourcePostion, direction))
                    {
                        waypoints.AddRange(path.corners.ToList());
                        debug("Custom Path generated successfully", false);
                    }
                    else
                    {
                        debug("Failed to generate a Custom path. Waypoints are outside the AIFOV. Generating a new one", false);
                        Fails++;
                    }
                }
            }
            else
            {
                debug("Failed to generate a Custom path. Invalid Path. Generating a new one", false);
                Fails++;
            }
        }
    }

    private bool CheckForAngle(Vector3 pos, Vector3 source, Vector3 direction) //calculates the angle between the car and the waypoint 
    {
        Vector3 distance = (pos - source).normalized;
        float CosAngle = Vector3.Dot(distance, direction);
        float Angle = Mathf.Acos(CosAngle) * Mathf.Rad2Deg;

        if (Angle < AIFOV)
            return true;
        else
            return false;
    }

    private void ApplyBrakes() // Apply brake torque 
    {
        frontLeft.brakeTorque = 5000;
        frontRight.brakeTorque = 5000;
        backLeft.brakeTorque = 5000;
        backRight.brakeTorque = 5000;
    }


    private void UpdateWheels() // Updates the wheel's postion and rotation
    {
        ApplyRotationAndPostion(frontLeft, wheelFL);
        ApplyRotationAndPostion(frontRight, wheelFR);
        ApplyRotationAndPostion(backLeft, wheelBL);
        ApplyRotationAndPostion(backRight, wheelBR);
    }

    private void ApplyRotationAndPostion(WheelCollider targetWheel, Transform wheel) // Updates the wheel's postion and rotation
    {
        targetWheel.ConfigureVehicleSubsteps(5, 12, 15);

        Vector3 pos;
        Quaternion rot;
        targetWheel.GetWorldPose(out pos, out rot);
        wheel.position = pos;
        wheel.rotation = rot;
    }

    void ApplySteering() // Applies steering to the Current waypoint
    {
        Vector3 relativeVector = transform.InverseTransformPoint(PostionToFollow);
        float SteeringAngle = (relativeVector.x / relativeVector.magnitude) * MaxSteeringAngle;
        if (SteeringAngle > 15) LocalMaxSpeed = 100;
        else LocalMaxSpeed = MaxRPM;

        frontLeft.steerAngle = SteeringAngle;
        frontRight.steerAngle = SteeringAngle;
    }


    void Movement() // moves the car forward and backward depending on the input
    {
        if (move == true && allowMovement == true)
            allowMovement = true;
        else
            allowMovement = false;

        if (allowMovement == true)
        {
            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
            backLeft.brakeTorque = 0;
            backRight.brakeTorque = 0;

            int SpeedOfWheels = (int)((frontLeft.rpm + frontRight.rpm + backLeft.rpm + backRight.rpm) / 4);

            if (SpeedOfWheels < LocalMaxSpeed)
            {
                backRight.motorTorque = 400 * MovementTorque;
                backLeft.motorTorque = 400 * MovementTorque;
                frontRight.motorTorque = 400 * MovementTorque;
                frontLeft.motorTorque = 400 * MovementTorque;
            }
            else if (SpeedOfWheels < LocalMaxSpeed + (LocalMaxSpeed * 1 / 4))
            {
                backRight.motorTorque = 0;
                backLeft.motorTorque = 0;
                frontRight.motorTorque = 0;
                frontLeft.motorTorque = 0;
            }
            else
                ApplyBrakes();
            
        }
        else
            ApplyBrakes();
    }

    void debug(string text, bool IsCritical)
    {
        if (Debugger)
        {
            if (IsCritical)
                Debug.LogError(text);
            else
                Debug.Log(text);
        }
    }

    private void OnDrawGizmos() // shows a Gizmos representing the waypoints and AI FOV
    {
        if (ShowGizmos == true)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (i == currentWayPoint)
                    Gizmos.color = Color.blue;
                else
                {
                    if (i > currentWayPoint)
                        Gizmos.color = Color.red;
                    else
                        Gizmos.color = Color.green;
                }
                Gizmos.DrawWireSphere(waypoints[i], 2f);
            }
            CalculateFOV();
        }

        void CalculateFOV()
        {
            Gizmos.color = Color.white;
            float totalFOV = AIFOV * 2;
            float rayRange = 10.0f;
            float halfFOV = totalFOV / 2.0f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(carFront.position, leftRayDirection * rayRange);
            Gizmos.DrawRay(carFront.position, rightRayDirection * rayRange);
        }
    }
}
