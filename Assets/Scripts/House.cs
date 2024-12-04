using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : EnergyConsumer
{
    // Start is called before the first frame update
    void Start()
    {
        setConsumptionRate(5f);
        setCurrentConsumption(15f);
        setOperationalStatus(true); // Initialize operational status
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Check operational status every second
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic if needed
    }

    public override float requestEnergy()
    {
        return getOperationalStatus() ? getConsumptionRate() : 0; // Return consumption rate only if operational
    }

    public override bool isOperational()
{
    if (isRepairing)
    {
        return false; // If under repair, not operational
    }

    if (getCurrentConsumption() < getConsumptionRate())
    {
        Debug.Log("House consumption rate is too low.");
        setOperationalStatus(false);
        SetSpriteColor(false); 
    }
    else if (Random.Range(0f, 1f) < 0.01f) // 10% chance of failure per check
    {
        Debug.Log("House has randomly failed.");
        setOperationalStatus(false);
        SetSpriteColor(false); 
    }

    return getOperationalStatus();
}


    public override void checkOperationalStatus()
    {
        isOperational(); // Check operational status

        if (!getOperationalStatus() && !isRepairing)
        {
            Debug.Log("House is not operational. Initiating repair.");
            Repair(); // Trigger repair process
        }
    }

    public override void Repair()
    {
        isRepairing = true;
        StartCoroutine(RepairProcess());
    }

    private IEnumerator RepairProcess()
    {
        Debug.Log("Repair process started for house.");
        yield return new WaitForSeconds(3); // Example repair time for house
        isRepairing = false;
        setOperationalStatus(true); 
                    SetSpriteColor(true); 
// Mark as operational after repair
        Debug.Log("Repair completed. House is now operational.");
    }
}
