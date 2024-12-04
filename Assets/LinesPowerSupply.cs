using System.Collections.Generic;
using UnityEngine;

public class LinesPowerSupply : MonoBehaviour
{
    [SerializeField] private Transform[] _nodes; // List of nodes, first one is the Smart Grid node
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    private List<LineRenderer> _lineRenderers = new List<LineRenderer>();
    private List<PowerSupplier> connectedPowerSuppliers = new List<PowerSupplier>(); // List of connected power suppliers
    private List<Battery> connectedBatteries = new List<Battery>(); // List of connected batteries

    void Start()
    {
        if (_nodes == null || _nodes.Length < 2)
        {
            Debug.LogError("At least two nodes are required, with the first being the Smart Grid node.");
            return;
        }

        // The first node is considered the Smart Grid node
        Transform smartGridNode = _nodes[0];

        // Create a line for each subsequent node connected to the Smart Grid node
        for (int i = 1; i < _nodes.Length; i++)
        {
            GameObject lineSegment = new GameObject($"LineSegment_To_{_nodes[i].name}");
            lineSegment.transform.parent = this.transform;

            LineRenderer lr = lineSegment.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;

            lr.SetPosition(0, smartGridNode.position);
            lr.SetPosition(1, _nodes[i].position);

            // Set the line color to white initially
            lr.material.color = Color.white;

            _lineRenderers.Add(lr);

            // Check if the node is a PowerSupplier and add to the list
            PowerSupplier powerSupplier = _nodes[i].GetComponent<PowerSupplier>();
            if (powerSupplier != null)
            {
                connectedPowerSuppliers.Add(powerSupplier);
            }

            // Check if the node is a Battery and add to the list
            Battery battery = _nodes[i].GetComponent<Battery>();
            if (battery != null)
            {
                connectedBatteries.Add(battery);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the positions of the lines if nodes are moving
        if (_nodes != null && _nodes.Length > 1)
        {
            Transform smartGridNode = _nodes[0];

            for (int i = 1; i < _nodes.Length; i++)
            {
                LineRenderer lr = _lineRenderers[i - 1];
                lr.SetPosition(0, smartGridNode.position);
                lr.SetPosition(1, _nodes[i].position);
            }
        }

        // Update line colors based on operational status of connected power suppliers
        UpdateLineColorsForPowerSuppliers();

        // Update battery-specific line colors based on power supplier status
        UpdateLineColorsForBatteries();
    }

    // Method to handle the power supplier line color updates
    private void UpdateLineColorsForPowerSuppliers()
    {
        for (int i = 0; i < connectedPowerSuppliers.Count; i++)
        {
            PowerSupplier supplier = connectedPowerSuppliers[i];
            LineRenderer lr = _lineRenderers[i];

            if (supplier != null)
            {
                // Update line color based on operational status of the power supplier
                if (!supplier.getOperationalStatus()) // If not operational
                {
                    lr.material.color = Color.black;
                }
                else
                {
                    lr.material.color = Color.white; // Operational, set to white
                }
            }
        }
    }

    // Method to handle battery-specific logic for line color updates
    private void UpdateLineColorsForBatteries()
    {
        // Check if at least one line is black (indicating a non-operational power supplier)
        bool anyBlackLine = false;

        foreach (var lr in _lineRenderers)
        {
            if (lr.material.color == Color.black)
            {
                anyBlackLine = true;
                break;
            }
        }

        // Update the battery line colors based on the power supplier line status
        for (int i = 0; i < connectedBatteries.Count; i++)
        {
            Battery battery = connectedBatteries[i];
            LineRenderer lr = _lineRenderers[i + connectedPowerSuppliers.Count]; // Correct offset for battery lines

            if (battery != null)
            {
                // If at least one power supply line is black, change battery line to yellow
                if (anyBlackLine)
                {
                    lr.material.color = Color.yellow;
                }
                else
                {
                    lr.material.color = Color.white; // All lines are operational, set to white
                }
            }
        }
    }
}
