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
    [SerializeField]float moveSpeed = .15f;
    private float moveSpeedHolder = 0f;
    private Vector3 bobbing = Vector3.zero;
    [SerializeField]private float floatStrength = 0.0002f;
    [SerializeField]private float bobFrequency = 10f;

    Vector3 transformExy = Vector3.zero;
    Vector3 wayPointExy = Vector3.zero;
    public void StartCar()
    {
        Debug.Log("Hello");
        Destroy(gameObject.GetComponent<Grabbable>());
        Destroy(gameObject.GetComponent<PhysicsGrabbable>());
        Destroy(gameObject.GetComponent<TouchHandGrabInteractable>());
        Destroy(gameObject.GetComponent<Collider>());
        Destroy(gameObject.GetComponent<WeldableObject>());
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<HandGrabInteractable>());
        thisCar?.Invoke(this);
        moveSpeedHolder = moveSpeed;
        CheckForCars();
    }
    public void FixRotPos()
    {
        //instantiate smokepuff
        GameObject smokePuff = Instantiate(Resources.Load<GameObject>("VFX/VFX_Smoke_Large"), transform.position, Quaternion.identity);
        Destroy(smokePuff, 2f);
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        transform.position = new Vector3(transform.position.x, wayPoints[0].position.y, transform.position.z);
    }
    public void MoveCar(float deltaTime) // might have to forget about the y axis so that cars dont go underground
    {
        bobbing = transform.position;
        bobbing.y = (Mathf.Sin(Time.time * bobFrequency) * floatStrength);
        if (waypointIndex <= wayPoints.Count - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointIndex].position, moveSpeed * deltaTime) + new Vector3(0,bobbing.y,0);

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
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, .1f);
        Debug.DrawRay(transform.position, transform.forward * .1f, Color.yellow, 1f);

        for(int i = 0; hits.Length > i; i++)
        {
            if (hits[i].collider != null && hits[i].collider.gameObject != this.gameObject);
            {
                moveSpeed = 0;
                Invoke("CheckForCars", .2f);
                return;
            }
        }
        moveSpeed = moveSpeedHolder;
        Invoke("CheckForCars", .2f);
    }
}
