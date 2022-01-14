using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellControl : MonoBehaviour
{
    public GameObject forceField;
    public Material mat;

    float lerpDuration = 1;
    float startValue = 0;
    float endValue = 4;
    float _in = 0.2f;
    float _out = 3f;
    public bool lerping;
    private void Start()
    {
        mat = forceField.GetComponent<Renderer>().material;
        forceField.GetComponent<Renderer>().material = mat;
    }
    public void CloseCell()
    {
        Debug.Log("Close");
        if (!lerping)
        {
            StartCoroutine(LerpForceFieldIn());
        }
        
    }

    public void OpenCell()
    {
        Debug.Log("Open");
        if (!lerping)
        {
            StartCoroutine(LerpForceFieldOut());
        }
        
    }

    IEnumerator LerpForceFieldIn()
    {
        float timeElapsed = 0;
        float startval = (mat.GetFloat("Vector1_4B999DF5"));
        while (timeElapsed < lerpDuration)
        {
            lerping = true;
            mat.SetFloat("Vector1_4B999DF5", Mathf.Lerp(startval, _in, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        mat.SetFloat("Vector1_4B999DF5", _in);
        lerping = false;
    }
    IEnumerator LerpForceFieldOut()
    {
        float timeElapsed = 0;
        float startval = (mat.GetFloat("Vector1_4B999DF5"));
        while (timeElapsed < lerpDuration)
        {
            lerping = true;
            mat.SetFloat("Vector1_4B999DF5", Mathf.Lerp(startval, _out, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        mat.SetFloat("Vector1_4B999DF5", _out);
        lerping = false;
    }
}
