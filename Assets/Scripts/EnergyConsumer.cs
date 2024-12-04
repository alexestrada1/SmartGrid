using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyConsumer : MonoBehaviour
{
    private float consumptionRate;
    private float currentConsumption;
    public bool isRepairing = false;
    private bool operationalStatus = true;
    protected SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer attached to the object
        SetSpriteColor(operationalStatus); // Set initial sprite color based on operational status
    }

    void Update()
    {
    }

    public virtual float getConsumptionRate()
    {
        return consumptionRate;
    }

    public virtual void setConsumptionRate(float value)
    {
        consumptionRate = value;
    }

    public virtual float getCurrentConsumption()
    {
        return currentConsumption;
    }

    public virtual void setCurrentConsumption(float value)
    {
        currentConsumption = value;
    }

    public virtual float requestEnergy()
    {
        Debug.Log("Parent class: " + getConsumptionRate());
        return 0;
    }

    // Methods to manage operational status and repair
    public virtual bool isOperational()
    {
        return operationalStatus;
    }
    
    public bool getOperationalStatus()
    {
        return operationalStatus;
    }

    public virtual void setOperationalStatus(bool status)
    {
        operationalStatus = status;
        SetSpriteColor(status); // Change sprite color when operational status changes
    }

    public virtual void checkOperationalStatus()
    {
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
        Debug.Log("Repairing energy consumer... please wait 5 seconds.");
        yield return new WaitForSeconds(5); // Wait for 5 seconds
        operationalStatus = true;
        SetSpriteColor(true); // Set sprite to operational after repair
        isRepairing = false; // Reset the flag after repair
        Debug.Log("Energy consumer has been repaired and is now operational.");
    }

    protected void SetSpriteColor(bool status)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = status ? Color.white : Color.black; // White for operational, black for failure
        }
    }
}
