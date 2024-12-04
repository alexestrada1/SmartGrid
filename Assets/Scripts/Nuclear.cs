using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuclear : PowerSupplier
{
    void Start()
    {
        setProductionRate(50f);
        setOperationalStatus(true);
        setOutputThreshold(30f); // Example output threshold value
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Check operational status every second
    }

    public override bool isOperational()
    {
        if (isRepairing)
        {
            return false; // If under repair, it should be considered not operational.
        }

        if (getProductionRate() < getOutputThreshold())
        {
            Debug.Log("Nuclear plant output is below the threshold.");
            setOperationalStatus(false);
            SetSpriteColor(false); 

        }
        else if (Random.Range(0f, 1f) < 0.001f) // 0.1% chance of failure per check
        {
            Debug.Log("Nuclear plant has randomly failed.");
            setOperationalStatus(false);
            SetSpriteColor(false); 

        }

        FailOverToBackup(); // Check if failure occurred and reroute power
        return getOperationalStatus();
    }

    public void FailOverToBackup()
    {
        if (!getOperationalStatus()) // If the nuclear plant fails
        {
            Debug.Log("Nuclear plant failed. Rerouting power to backup.");
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
        // Check if operational status is OK
        isOperational();
        // If not operational and not repairing, initiate repair
        if (!getOperationalStatus() && !isRepairing)
        {
            Debug.Log("Nuclear plant is not operational. Initiating repair.");
            Repair(); // Initiate repair if necessary
        }
    }

    public override void Repair()
    {
        isRepairing = true;
        StartCoroutine(RepairProcess());
    }

    private IEnumerator RepairProcess()
    {
        Debug.Log("Repair process started for nuclear plant.");
        yield return new WaitForSeconds(5); // Example repair time
        isRepairing = false;
        setOperationalStatus(true);
        SetSpriteColor(true); 
        Debug.Log("Repair completed. Nuclear plant is now operational.");
    }
}
