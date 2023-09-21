using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<PlayRugWay> ways = new List<PlayRugWay>();
    private void OnEnable()
    {
        CarsDoMove.thisCar += AddCarToWay;
    }
    private void OnDisable()
    {
        CarsDoMove.thisCar -= AddCarToWay;
    }
    public void AddCarToWay(CarsDoMove car)
    {
        PlayRugWay tempWay = ways[Random.Range(0, ways.Count)];
        car.wayPoints = tempWay.wayPoints;
        tempWay.Drive += car.MoveCar;
    }
  
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            other.gameObject.GetComponent<CarsDoMove>().StartCar(); // den h�r f�r du n�r du �r en bil
            other.gameObject.GetComponent<Cluster>()?.IAmACar(); //den h�r kallas p� n�r din inte cluster collider g�r sin grej
        }
    }
}
