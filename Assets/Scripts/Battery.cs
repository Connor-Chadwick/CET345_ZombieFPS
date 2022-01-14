using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public Material green, blue, orange;
    public Renderer rend;
    public GameObject battery;
    public Material[] batteryMats;
    public string batteryColour;
    private Rigidbody rb;

    [Header("Tool Tip")]
    public string _Name;
    public string _Description;
    private ToolTipUI tooltipUI;
    public enum Colour
    {
        Green,
        Blue,
        Orange,
    }

    public Colour colour;

    private void Start()
    {
       
        tooltipUI = GameObject.FindGameObjectWithTag("TTUI").GetComponent<ToolTipUI>();
        rb = GetComponent<Rigidbody>();
        batteryMats = battery.gameObject.GetComponent<Renderer>().materials;
        rend = GetComponent<Renderer>();
        switch (colour)
        {
            case Colour.Green:
                SetGreen();
                break;
            case Colour.Blue:
                SetBlue();
                break;
            case Colour.Orange:
                SetYellow();
                break;
            
        }
    }

    public void SetGreen()
    {
        batteryMats[1] = green;
        batteryColour = "Green";
        battery.gameObject.GetComponent<Renderer>().materials = batteryMats;
    }

    public void SetYellow()
    {
        batteryMats[1] = orange;
        batteryColour = "Yellow";
        battery.gameObject.GetComponent<Renderer>().materials = batteryMats;
    }

    public void SetBlue()
    {
        batteryMats[1] = blue;
        batteryColour = "Blue";
        battery.gameObject.GetComponent<Renderer>().materials = batteryMats;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Generator"))
        {
            Generator gen = other.GetComponent<Generator>();
            if (gen.genColour == batteryColour)
            {
                gen.PowerOn();
                transform.parent = gen.batteryPosition.transform;
                transform.position = gen.batteryPosition.position;
                transform.rotation = gen.batteryPosition.rotation;
                rb.useGravity = false;
                rb.isKinematic = true;
                Destroy(GetComponent<Battery>());
            }
        }
    }

   
}
