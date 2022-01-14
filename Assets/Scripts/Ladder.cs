using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private GameObject player;
    private bool canClimb;
    public int speed;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            canClimb = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            canClimb = false;
        }
    }

    private void Update()
    {
        if (canClimb)
        {
            if (Input.GetKey(KeyCode.W))
            {
                player.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
            }
        }
    }
}
