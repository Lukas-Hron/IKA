using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CarsDoMove : MonoBehaviour
{
    public static Action<CarsDoMove> thisCar;
    
    public List<Transform> wayPoints = new List<Transform>();
    int waypointIndex = 0;
    [SerializeField]float moveSpeed = .07f;

    Vector3 transformExy = Vector3.zero;
    Vector3 wayPointExy = Vector3.zero;

    public PlayRugWay myWay = null;

    private Rigidbody rb = null;
    private bool onWay = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCar(false);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            StartCar(true);
        }
    }
    public void StartCar(bool enabledScripts = false)
    {
        if (enabledScripts == false && onWay == false)
        {
            CancelInvoke();
            onWay = true;
            Invoke("StartIt", 2f);
            Debug.Log("Start");
        }
        else if(enabledScripts == true && onWay == true)
        {
            Debug.Log(myWay == null);
            if (myWay == null)
                return;

            Debug.Log("Stopp");
            gameObject.layer = 0;
            myWay.Drive -= MoveCar;
            myWay = null;

            rb.freezeRotation = false;
            rb.constraints = RigidbodyConstraints.None;
            waypointIndex = 0;
            onWay = false;
        }
    }

    private void StartIt() 
    {
        thisCar?.Invoke(this); // asks to be subscribed to one of the ways
        gameObject.layer = 11; // no longer collides with the floor
        FixRotPos();
    }

    public void FixRotPos() // locks rotation and position of rigidbody
    {
         rb.constraints = RigidbodyConstraints.FreezePosition;
         rb.freezeRotation = true;
         transform.forward = (wayPoints[waypointIndex].position - transform.position);
        
    }
    public void MoveCar(float deltaTime) // might have to forget about the y axis so that cars dont go underground
    {
        if (waypointIndex <= wayPoints.Count - 1)
        {
            Vector3 headed = wayPoints[waypointIndex].position;
            headed.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, headed, moveSpeed * deltaTime);

            transformExy = transform.position;
            wayPointExy = new Vector3(wayPoints[waypointIndex].position.x, transform.position.y, wayPoints[waypointIndex].position.z);

            Vector3 dir = (wayPoints[waypointIndex].position - transform.position);
            dir.y = 0;
            if ((wayPointExy - transformExy).normalized != Vector3.zero)
            {
                Vector3 targetDirection = dir;

                // The step size is equal to speed times frame time.
                float singleStep = 10f * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                Quaternion f = Quaternion.LookRotation(newDirection);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, f , 10f*Time.deltaTime);
               
            }
        }
        else
        {
            waypointIndex = 0;
        }
        if(transformExy == wayPointExy)
        {
            waypointIndex++;
        }
        
    }
}
