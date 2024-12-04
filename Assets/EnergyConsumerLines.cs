using System.Collections.Generic;
using UnityEngine;

public class EnergyConsumerPowerLines : MonoBehaviour
{
    [SerializeField] private Transform[] _nodes; // List of nodes
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    private List<LineRenderer> _lineRenderers = new List<LineRenderer>();
    private List<EnergyConsumer> connectedEnergyConsumers = new List<EnergyConsumer>();

    void Start()
    {
        if (_nodes == null || _nodes.Length < 2)
        {
            Debug.LogError("At least two nodes are required, with the first being the Smart Grid node.");
            return;
        }

        // Log the number of nodes and their names for debugging
        // The first node is considered the Smart Grid node
        Transform smartGridNode = _nodes[0];

        // Iterate through the nodes (starting from index 1) and create lines
        for (int i = 1; i < _nodes.Length; i++)
        {
            Transform node = _nodes[i];
            
            // Create a line from the Smart Grid to this energy consumer (house or other node)
            CreateLine(smartGridNode, node);

            // Get the EnergyConsumer component from the node (assuming it's attached to the node's GameObject)
            EnergyConsumer energyConsumer = _nodes[i].GetComponent<EnergyConsumer>();
            if (energyConsumer != null)
            {
                connectedEnergyConsumers.Add(energyConsumer);
            }
        }
    }

    void Update()
    {
        // Optional: Update line positions dynamically if nodes move
        if (_nodes != null && _nodes.Length > 1)
        {
            Transform smartGridNode = _nodes[0];

            for (int i = 1; i < _nodes.Length; i++)
            {
                if (_lineRenderers.Count > i - 1)  // Ensure the line renderer exists
                {
                    LineRenderer lr = _lineRenderers[i - 1];
                    lr.SetPosition(0, smartGridNode.position);
                    lr.SetPosition(1, _nodes[i].position);
                }
            }
        }

        // Update line colors based on operational status of connected energy consumers
        for (int i = 0; i < connectedEnergyConsumers.Count; i++)
        {
            EnergyConsumer consumer = connectedEnergyConsumers[i];
            LineRenderer lr = _lineRenderers[i];

            if (consumer != null)
            {
                // Change the line color based on the operational status of the energy consumer
                if (!consumer.getOperationalStatus()) // If not operational (i.e., failed)
                {
                    lr.material.color = Color.black; // Set the line color to black to indicate failure
                }
                else
                {
                    lr.material.color = Color.white; // Set the line color to white when operational
                }
            }
        }
    }

    // Method to create a line from the Smart Grid to an energy consumer node
    private void CreateLine(Transform start, Transform end)
    {
        GameObject lineSegment = new GameObject($"LineSegment_To_{end.name}");
        lineSegment.transform.parent = this.transform;

        LineRenderer lr = lineSegment.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = lineMaterial;
        lr.useWorldSpace = true;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        lr.SetPosition(0, start.position);
        lr.SetPosition(1, end.position);

        _lineRenderers.Add(lr);
    }

    // Optional: Adjust all line widths dynamically
    public void UpdateLineWidths(float newWidth)
    {
        foreach (var lr in _lineRenderers)
        {
            lr.startWidth = newWidth;
            lr.endWidth = newWidth;
        }
    }
}
