using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipItem : MonoBehaviour
{
    public string _name;
    public string _description;
    private ToolTipUI toolTipUI;
    public Outline outline1;
    public bool hasOutline = false;

    private void Start()
    {
       
            if (outline1 != null)
            {
                StartCoroutine(DisableOutlineOnStart());
                hasOutline = true;
            }
            
        
        toolTipUI = FindObjectOfType<ToolTipUI>();

    }

    public void Reveal()
    {
        toolTipUI.objectName.text = _name;
        toolTipUI.objectDesc.text = _description;
        toolTipUI.nonWeaponToolTip.SetActive(true);

        if (hasOutline)
        {
            outline1.enabled = true;
        }
    }

    public void UnReveal()
    {
        toolTipUI.nonWeaponToolTip.SetActive(false);

        if (hasOutline)
        {
            outline1.enabled = false;
        }
    }

    public void DisableToolTip()
    {
        toolTipUI.nonWeaponToolTip.SetActive(false);
        outline1.enabled = false;
    }

    IEnumerator DisableOutlineOnStart()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Outline>().enabled = false;
    }
}
