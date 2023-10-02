using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CarsDoMove : MonoBehaviour
{
    public static Action<CarsDoMove> thisCar;

    public List<Transform> wayPoints = new List<Transform>();
    int waypointIndex = 0;
    [SerializeField]float moveSpeed = .07f;

    private float moveSpeedHolder = 0f;
    private Vector3 bobbing = Vector3.zero;

    [SerializeField]private float floatStrength = 0.0002f;
    [SerializeField]private float bobFrequency = 10f;

    Vector3 transformExy = Vector3.zero;
    Vector3 wayPointExy = Vector3.zero;

    public PlayRugWay myWay = null;

    private Rigidbody rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void StartCar(bool enabledScripts = false)
    {
        
        //EnabledScripts(enabledScripts);
        //Destroy(gameObject.GetComponent<Collider>());
      
        if (enabledScripts == false)
        {
            thisCar?.Invoke(this);
            moveSpeedHolder = moveSpeed;
            CheckForCars();
        }
        else if(wayPoints.Count > 0)
        {
            myWay.Drive -= MoveCar;
            CancelInvoke();
            wayPoints.Clear();
            
            rb.constraints = RigidbodyConstraints.None;
            waypointIndex = 0;
        }

    }

    private void EnabledScripts(bool enabledScripts)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<WeldableObject>().enabled = enabledScripts;
        }

        gameObject.GetComponent<Grabbable>().enabled = enabledScripts;
        gameObject.GetComponent<PhysicsGrabbable>().enabled = enabledScripts;
        gameObject.GetComponent<TouchHandGrabInteractable>().enabled = enabledScripts;
        gameObject.GetComponent<WeldableObject>().enabled = enabledScripts;
        gameObject.GetComponent<HandGrabInteractable>().enabled = enabledScripts;
    }

    public void FixRotPos()
    {
        //instantiate smokepuff
        //transform.position = new Vector3(transform.position.x, wayPoints[0].position.y, transform.position.z);
         rb.constraints = RigidbodyConstraints.FreezePosition;
         rb.freezeRotation = true;
    }
    public void MoveCar(float deltaTime) // might have to forget about the y axis so that cars dont go underground
    {
        
       

        if (waypointIndex <= wayPoints.Count - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(wayPoints[waypointIndex].position.x, transform.position.y, wayPoints[waypointIndex].position.z), moveSpeed * deltaTime);// + new Vector3(0,bobbing.y,0);

            transformExy = new Vector3(transform.position.x, 0, transform.position.z);
            wayPointExy = new Vector3(wayPoints[waypointIndex].position.x, 0, wayPoints[waypointIndex].position.z);

            if ((wayPointExy - transformExy).normalized != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation((wayPointExy - transformExy).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * deltaTime);
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
    private void CheckForCars()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(transform.position.x, wayPoints[0].position.y, transform.position.z) , transform.forward, .1f);
        Debug.DrawRay(transform.position, transform.forward * .1f, Color.yellow, 1f);

        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null && hits[i].collider.transform.root.gameObject != this.transform.root.gameObject)
            {
                Debug.Log(hits[i].collider.gameObject.name);
                moveSpeed = 0;
                Invoke("CheckForCars", .2f);
                return;
            }
        }
        moveSpeed = moveSpeedHolder;
        Invoke("CheckForCars", .2f);
        
    }
    private void OnDestroy()
    {
        CancelInvoke();
        myWay.Drive -= MoveCar;
    }
}
