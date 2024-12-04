using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : EnergyConsumer
{
    // Start is called before the first frame update
    void Start()
    {
        setConsumptionRate(2f);
        setCurrentConsumption(20f);
        setOperationalStatus(true);
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Check operational status every second
    }

    public override bool isOperational()
    {
        if (isRepairing)
        {
            return false; // If under repair,d not be operational
        }

        if (getCurrentConsumption() < getConsumptionRate())
        {
            Debug.Log("Factory output is below the threshold.");
            setOperationalStatus(false);
        }
        else if (Random.Range(0f, 1f) < 0.001f) // 0.1% chance of failure per check
        {
            Debug.Log("Factory has randomly failed.");
            setOperationalStatus(false);
        }

        return getOperationalStatus();
    }

    public override float requestEnergy()
    {
        return getOperationalStatus() ? getConsumptionRate() : 0; // Send energy only if operational
    }

    public override void checkOperationalStatus()
    {
        isOperational(); // Call to check status

        if (!getOperationalStatus() && !isRepairing)
        {
            Debug.Log("Factory is not operational. Initiating repair.");
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
        Debug.Log("Repair process started for factory.");
        yield return new WaitForSeconds(5); // Example repair time (change as needed)
        isRepairing = false;
        setOperationalStatus(true); // Mark as operational after repair
        Debug.Log("Repair completed. Factory is now operational.");
    }

    // Update is called once per frame
    void Update()
    {
        // Update the sprite color based on operational status
        SetSpriteColor(getOperationalStatus());
    }
}
