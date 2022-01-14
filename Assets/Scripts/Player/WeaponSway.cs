using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float amount = 0.1f;

    public float maxAmount = 0.3f;

    public float smoothAmount = 6.0f;

    private Vector3 initPos;

    private Gun gun;

    private void Start()
    {
        gun = GetComponent<Gun>();
        initPos = gun.viewPosition.localPosition;

    }

    private void FixedUpdate()
    {
        
        float moveX = -Input.GetAxis("Mouse X") * amount;
        float moveY = -Input.GetAxis("Mouse Y") * amount;

        moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
        moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

        Vector3 finalPostoMove = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPostoMove + initPos, Time.fixedDeltaTime * smoothAmount);
    }
}
