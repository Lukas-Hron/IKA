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
    public void StartCar()
    {
        transform.rotation = Quaternion.Euler(0,transform.rotation.y,0);   
        thisCar?.Invoke(this);
        moveSpeedHolder = moveSpeed;
        CheckForCars();
    }

    public void MoveCar(float deltaTime) // might have to forget about the y axis so that cars dont go underground
    {
        if(waypointIndex <= wayPoints.Count - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointIndex].position, moveSpeed * deltaTime);
           
            if((wayPoints[waypointIndex].position - transform.position).normalized != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation((wayPoints[waypointIndex].position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * deltaTime);
            }
        }
        else
        {
            waypointIndex = 0;
        }

        if(transform.position == wayPoints[waypointIndex].position)
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
            if (hits[i].collider != null && hits[i].collider.gameObject != this.gameObject && hits[i].collider.CompareTag("Car"));
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
