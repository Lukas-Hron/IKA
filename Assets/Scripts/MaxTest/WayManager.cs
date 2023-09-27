using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<PlayRugWay> ways = new List<PlayRugWay>();
    public ParticleSpawner spawner;
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
        int temp = Random.Range(0, ways.Count);
        PlayRugWay tempWay = ways[temp];
        car.wayPoints = tempWay.wayPoints;


        spawner.PlayOneParticles(car.transform.position, 0, true);



        Debug.Log("Called");
        car.FixRotPos();
        tempWay.Drive += car.MoveCar;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.root.GetComponent<CarsDoMove>() != null)
        {
            other.gameObject.transform.root.GetComponent<CarsDoMove>().StartCar(); // den här får du när du är en bil också kanske ska vara root
        }
    }
}
