using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartGrid : MonoBehaviour
{
     public static SmartGrid Instance { get; private set; }

    private float totalEnergyProduced = 0f;
    private float totalEnergyConsumed = 0f;
     public List<PowerSupplier> powerSuppliers;
    public List<EnergyConsumer> energyConsumers;


    public Solar solarPanel;
    public Wind windTurbine;
    public Nuclear nuclearPlant;
    public PowerPlant powerPlant;
    public Batteries batteries;
    public Neighborhood neighborhood;
    public House house;
    public Factory factory;
     // Method to simulate a blackout
    public void SimulateBlackout()
    {
        StartCoroutine(BlackoutSequence());
    }

    private IEnumerator BlackoutSequence()
    {
        Debug.Log("Blackout initiated: Power suppliers and consumers are now down.");

        // Set all power suppliers and energy consumers to non-operational
        foreach (PowerSupplier supplier in powerSuppliers)
        {
            supplier.setOperationalStatus(false);
        }

        foreach (EnergyConsumer consumer in energyConsumers)
        {
            consumer.setOperationalStatus(false);
        }

        // Wait for 10 seconds to simulate the blackout
        yield return new WaitForSeconds(10f);

        Debug.Log("Blackout ended: Restoring power suppliers and consumers.");

        // Restore the power suppliers and energy consumers to operational status
        foreach (PowerSupplier supplier in powerSuppliers)
        {
            supplier.setOperationalStatus(true);
        }

        foreach (EnergyConsumer consumer in energyConsumers)
        {
            consumer.setOperationalStatus(true);
        }
    }

   public void ReceiveEnergy()
{
    totalEnergyProduced = 0f;

    // Check if each power supplier is operational before adding energy
    if (solarPanel.isOperational())
    {
        totalEnergyProduced += solarPanel.sendEnergy();
    }
    else
    {
        solarPanel.FailOverToBackup();
        UseBackupPower(); // Fail over to backup power when solar fails
    }

    if (windTurbine.isOperational())
    {
        totalEnergyProduced += windTurbine.sendEnergy();
    }
    else
    {
        windTurbine.FailOverToBackup();
        UseBackupPower(); // Fail over to backup power when wind turbine fails
    }

    if (nuclearPlant.isOperational())
    {
        totalEnergyProduced += nuclearPlant.sendEnergy();
    }
    else
    {
        nuclearPlant.FailOverToBackup();
        UseBackupPower(); // Fail over to backup power when nuclear plant fails
    }

    if (powerPlant.isOperational())
    {
        totalEnergyProduced += powerPlant.sendEnergy();
        
    }
    else
    {
        powerPlant.FailOverToBackup(); 
        UseBackupPower();// Fail over to backup power when power plant fails
    }

    // Add consumer energy requests
    totalEnergyConsumed = 0f;
    totalEnergyConsumed += neighborhood.requestEnergy();
    totalEnergyConsumed += house.requestEnergy();
    totalEnergyConsumed += factory.requestEnergy();
}

     void Awake()
    {
        // Ensure there is only one instance of SmartGrid
        if (Instance == null)
        {
            Instance = this; // Assign this instance to the static Instance property
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        // You could also keep this object persistent if needed
        DontDestroyOnLoad(gameObject);
    }
    public void UseBackupPower()
{
    if (batteries.batteries.Count > 0)
    {
        float backupEnergy = 0f;
        
        // Check if the battery has energy to provide
        foreach (Battery battery in batteries.batteries)
        {
            if (battery.getChargeLevel() > 0)
            {
                backupEnergy += battery.getChargeLevel();
                battery.setChargeLevel(0); // Using up the battery power
                break; // Only use one backup power source for now
            }
        }

        totalEnergyProduced += backupEnergy;

    }
    else
    {
        Debug.Log("No backup power available. Consider adding more energy sources.");
    }
}


    public void sendEnergy()
    {
        float surplusEnergy = totalEnergyProduced - totalEnergyConsumed;

        if (surplusEnergy > 0)
        {
            foreach (Battery battery in batteries.batteries)
            {
                if (battery.getChargeLevel() < battery.getCapacity())
                {
                    float energyToSend = Mathf.Min(surplusEnergy, battery.getCapacity() - battery.getChargeLevel());
                    battery.recieveEnergy(energyToSend);
                    surplusEnergy -= energyToSend;

                    if (surplusEnergy <= 0) break;
                }
            }
        }
    }

   

    void Start()
    {
        InvokeRepeating("ReceiveAndSendEnergy", 0f, 2f);
    }

    private void ReceiveAndSendEnergy()
    {
        ReceiveEnergy();
        sendEnergy();
    }

    private void Update()
    {
        // Check for "B" key press to trigger blackout
        if (Input.GetKeyDown(KeyCode.B))
        {
            SimulateBlackout();
        }
    }
}