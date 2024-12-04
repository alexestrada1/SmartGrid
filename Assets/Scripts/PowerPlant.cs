using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : PowerSupplier
{
    void Start()
    {
        setProductionRate(40f);
        setOperationalStatus(true);
        setOutputThreshold(25f); // Example output threshold value
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Regularly check operational status
    }

    public override bool isOperational()
    {
        if (isRepairing)
        {
            return false; // If under repair, it's considered not operational
        }

        if (getProductionRate() < getOutputThreshold())
        {
            Debug.Log("Power plant output is below the threshold.");
            setOperationalStatus(false);
             SetSpriteColor(false); 

        }
        else if (Random.Range(0f, 1f) < 0.004f) // 0.4% chance of failure per check
        {
            Debug.Log("Power plant has randomly failed.");
            setOperationalStatus(false);
            SetSpriteColor(false);
        }

        FailOverToBackup(); // Check for failure and reroute power if necessary
        return getOperationalStatus();
    }

    public void FailOverToBackup()
    {
        if (!getOperationalStatus()) // If the power plant fails
        {
            Debug.Log("Power plant failed. Rerouting power to backup.");
            // Use backup power source (e.g., batteries or other power plants)
            SmartGrid.Instance.UseBackupPower();  // Example method to use backup power
        }
    }

    public override float sendEnergy()
    {
        // Send energy only if operational
        return getOperationalStatus() ? getProductionRate() : 0;
    }

    public override void checkOperationalStatus()
    {
        isOperational(); // Check if the plant is operational

        // If the plant is not operational and it's not already repairing, initiate repair
        if (!getOperationalStatus() && !isRepairing)
        {
            Debug.Log("Power plant is not operational. Initiating repair.");
            Repair(); // Initiate repair process
        }
    }

    public override void Repair()
    {
        isRepairing = true;
        StartCoroutine(RepairProcess());
    }

    private IEnumerator RepairProcess()
    {
        Debug.Log("Repair process started for power plant.");
        yield return new WaitForSeconds(6); // Example repair time
        isRepairing = false;
        setOperationalStatus(true);
        SetSpriteColor(true); 

        Debug.Log("Repair completed. Power plant is now operational.");
    }
}
