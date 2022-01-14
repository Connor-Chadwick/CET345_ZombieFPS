using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject mainCam;
    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Weapon"), LayerMask.NameToLayer("Default"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
