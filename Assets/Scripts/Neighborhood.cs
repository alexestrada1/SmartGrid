using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : EnergyConsumer
{
    public List<House> houses = new List<House>();
    float energyStart ;
    void Start()
    {
        
        // Initialize houses
        for (int i = 0; i < 10; i++)
        {
            House house = gameObject.AddComponent<House>();
            energyStart += house.getConsumptionRate();
            houses.Add(house);
        }
        // Example consumption rate for neighborhood
        setOperationalStatus(true); // Initialize operational status
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Check operational status every second
    }

    public override float requestEnergy()
    {

        float energyReq = 0;
        foreach (House house in houses)
        {
            energyReq += house.requestEnergy();
        }
        return getOperationalStatus() ? energyReq : 0; // Return total energy request if operational
    }


}
