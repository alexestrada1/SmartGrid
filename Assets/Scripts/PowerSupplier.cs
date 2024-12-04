using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupplier : MonoBehaviour
{
    private float outputThreshold;
    public bool isRepairing = false;
    private float productionRate;
    private float efficiency =1f;
    private bool operationalStatus;
    protected SpriteRenderer spriteRenderer; 
    protected void SetSpriteColor(bool status)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = status ? Color.white : Color.black; // White for operational, black for failure
        }
    }


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer attached to the object
    }


    public  virtual bool isOperational()
    {
        return false;
    }

    public float getProductionRate()
    {
        return productionRate;
    }
    public void setProductionRate(float value)
    {
        productionRate = value;
    }
    public float getOutputThreshold()
    {
        return outputThreshold;
    }
    public void setOutputThreshold(float value)
    {
        outputThreshold = value;
    }
    public bool getOperationalStatus()
    {
        return operationalStatus;
    }
    public void setOperationalStatus(bool value)
    {
        operationalStatus = value;
    }

    public float getEfficiency()
    {
        return efficiency;
    }
    public void setEfficiency(float value)
    {
        efficiency = value;
    }
    public virtual float sendEnergy()
    {
        // this function will send the energy the power consumers make to the grid.
        // Empy right now
        return 0;
    }
    public virtual void Repair()
    {
        if (!isRepairing) // Check if a repair is already in progress
        {
            StartCoroutine(RepairCoroutine());
        }
    }

    public IEnumerator RepairCoroutine()
    {
        isRepairing = true;
        Debug.Log("Repairing power supplier... please wait 5 seconds.");
        yield return new WaitForSeconds(5); // Wait for 5 seconds
        operationalStatus = true;
        isRepairing = false; // Reset the flag after repair
        Debug.Log("Power supplier has been repaired and is now operational.");
    }

void Start()
    {
        InvokeRepeating("checkOperationalStatus", 0f, 1f); // Check every second
        InvokeRepeating("sendEnergy", 0f, 2f);
    }

    public virtual void checkOperationalStatus()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}