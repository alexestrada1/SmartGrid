using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batteries : MonoBehaviour
{
    public List<Battery> batteries = new List<Battery>();
    void Start()
    {
        PopulateBatterysList();
        Debug.Log(batteries);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Automatically find all Battery components in the children

            Debug.Log(batteries);
            foreach (Battery battery in batteries)
            {
                Debug.Log("battery cahrge Level: " + battery.getChargeLevel() + "\n");
            }

        }
    }
    void PopulateBatterysList()
    {
        // Automatically find all Battery components in the children
        foreach (Transform child in transform)
        {
            Battery Battery = child.GetComponent<Battery>();
            if (Battery != null)
            {
                batteries.Add(Battery);
            }
        }
        Debug.Log(batteries);
        Debug.Log("Number of Batterys found: " + batteries.Count);
    }
}