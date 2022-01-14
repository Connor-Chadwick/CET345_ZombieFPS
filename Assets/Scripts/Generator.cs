using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public Material green, blue, orange;
    public Material electricMat;
    public GameObject powerObject;
    public Transform batteryPosition;
    public Material[] powerObjectMats;
    public string genColour;
    public bool isPowered;
    private Animator animator;
    public GameObject smoke;
    //public GameObject doorToPower;
    public GameObject[] doorsToPower;
    public GameObject powerWire;
    private Material[] powerWireMats;
    public enum Colour
    {
        Green,
        Blue,
        Orange,
    }

    public Colour colour;

    private void Start()
    {
        animator = GetComponent<Animator>();
        powerObjectMats = powerObject.gameObject.GetComponent<Renderer>().materials;
        powerWireMats = powerWire.gameObject.GetComponent<Renderer>().materials;
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

    public void PowerOn()
    {
        powerWireMats[1] = electricMat;
        powerWire.gameObject.GetComponent<Renderer>().materials = powerWireMats;
        animator.SetBool("powered", true);
        isPowered = true;
        smoke.SetActive(true);
        //doorToPower.GetComponent<Door>().isPowered = true;

        for (int i = 0; i < doorsToPower.Length; i++)
        {
            doorsToPower[i].GetComponent<Door>().isPowered = true;
            //i++;
        }
        GetComponent<AudioSource>().Play();
    }
    public void SetGreen()
    {
        powerObjectMats[1] = green;
        genColour = "Green";
        powerObject.gameObject.GetComponent<Renderer>().materials = powerObjectMats;
    }

    public void SetYellow()
    {
        powerObjectMats[1] = orange;
        genColour = "Yellow";
        powerObject.gameObject.GetComponent<Renderer>().materials = powerObjectMats;
    }

    public void SetBlue()
    {
        powerObjectMats[1] = blue;
        genColour = "Blue";
        powerObject.gameObject.GetComponent<Renderer>().materials = powerObjectMats;
    }
}
