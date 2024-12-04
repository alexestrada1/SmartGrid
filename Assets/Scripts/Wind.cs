using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : PowerSupplier
{
    void Start()
    {
        setProductionRate(25f);
        setOperationalStatus(true);
        setOutputThreshold(10f); // Set an output threshold (example value)
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Regularly check operational status
    }

    public override bool isOperational()
{
    if (isRepairing)
    {
        return false; // If under repair, it should be considered not operational.
    }

    if (getProductionRate() < getOutputThreshold())
    {
        Debug.Log("Wind plant output is below the threshold.");
        setOperationalStatus(false);
        SetSpriteColor(false); 
    }
    else if (Random.Range(0f, 1f) < 0.001f) // Random failure chance
    {
        Debug.Log("Wind plant has randomly failed.");
        setOperationalStatus(false);
        SetSpriteColor(false); 
    }
    FailOverToBackup();
    return getOperationalStatus();
}

public void FailOverToBackup()
{
    if (!getOperationalStatus()) // If the wind turbine fails
    {
        Debug.Log("Wind turbine failed. Rerouting power to backup.");
        // Use backup power source (e.g., batteries or other power plants)
        SmartGrid.Instance.UseBackupPower();  // Example method to use backup power
    }
}


    public override float sendEnergy()
    {
        // Override the sendEnergy method for Wind logic
        return getOperationalStatus() ? getProductionRate() : 0; // Send energy only if operational
    }

    public override void checkOperationalStatus()
    {
        isOperational(); // Call the method to check operational status
        // If not operational and not repairing, initiate repair
        if (!getOperationalStatus() && !isRepairing)
        {
            Debug.Log("Wind plant is not operational. Initiating repair.");
            Repair(); // Call the repair method when not operational
        }
    }
        public override void Repair()
    {
        isRepairing = true;
        StartCoroutine(RepairProcess());
    }

    private IEnumerator RepairProcess()
    {
        Debug.Log("Repair process started for wind turbine.");
        yield return new WaitForSeconds(4); // Example repair time
        isRepairing = false;
        setOperationalStatus(true);
        SetSpriteColor(true); 
        Debug.Log("Repair completed. Wind turbine is now operational.");
    }

}
