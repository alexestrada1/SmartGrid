using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solar : PowerSupplier
{
    void Start()
    {
        setProductionRate(20f);
        setOperationalStatus(true);
        setOutputThreshold(15f); 
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Check operational status every second
    }

    public override bool isOperational()
    {
        if (isRepairing)
        {
            return false; // Not operating, under repair
        }

        if (getProductionRate() < getOutputThreshold())
        {
            Debug.Log("Solar panel output is below the threshold.");
            setOperationalStatus(false);
            SetSpriteColor(false); 

        }
        else if (Random.Range(0f, 1f) < 0.001f) // 0.01% chance of failure per check
        {
            Debug.Log("Solar panel has randomly failed.");
            setOperationalStatus(false);
            SetSpriteColor(false); 
        }

        FailOverToBackup(); // Check if failure occurred and reroute power
        return getOperationalStatus();
    }

    public void FailOverToBackup()
    {  
        Debug.Log("Solar panel failed. Rerouting power to backup.");
        // Use backup power source 
        SmartGrid.Instance.UseBackupPower(); 
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
            Debug.Log("Solar panel is not operational. Initiating repair.");
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
        Debug.Log("Repair process started for solar panel.");
        yield return new WaitForSeconds(3); // Example repair time
        isRepairing = false;
        setOperationalStatus(true);
        SetSpriteColor(true); // Change sprite color to white when finished repairing
        Debug.Log("Repair completed. Solar panel is now operational.");
    }
}
