using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private float capacity = 100f;
    private float chargeLevel;
    private bool backup = false;

    private float dischargeRate = 5f;
    private float chargeRate = 5f;



    public float getCapacity() { return capacity; }
    public bool getBackup() { return backup; }
    public void setBackup(bool value ) { backup = value; }


    public void setCapacity(float value)
    {
        capacity = value;
    }
    public float getChargeRate() { return chargeRate; }
    public void setChargeRate(float value)
    {
        chargeRate = value;
    }
     public float getDischargeChargeRate() { return dischargeRate; }
    public void setDischargeChargeRate(float value)
    {
        dischargeRate = value;
    }
    public float getChargeLevel() { return chargeLevel; }
    public void setChargeLevel(float value)
    {
        chargeLevel = value;
    }

    public void recieveEnergy(float energy)
    {

        chargeLevel += energy;
        chargeLevel = Mathf.Clamp(chargeLevel, 0, capacity);
       // Debug.Log("Received " + energy + "fromm grid");

    }
    public void dischargeEnergy(float amount)
    {
        chargeLevel -= amount;
        chargeLevel = Mathf.Clamp(chargeLevel, 0, capacity);
    }


    // Start is called before the first frame update
    void Start()
    {
        capacity = 100f;
        chargeLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
