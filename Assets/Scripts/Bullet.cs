using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float damage;

    public ParticleSystem bloodPrefab;
    public ParticleSystem impact;

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("zombie"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 position = contact.point;
            GetComponent<Collider>().enabled = false;

            ZombieAI zomb = collision.gameObject.GetComponentInParent<ZombieAI>();
            if (!zomb.isDead)
            {
                ParticleSystem blood = Instantiate(bloodPrefab, position, Quaternion.Euler(0, 0, 90));
                blood.transform.parent = collision.transform;
                blood.transform.localScale = new Vector3(1, 1, 1);
                zomb.TakeDamage(damage);
                // zomb.GetComponent<Animator>().SetBool("crawling", true);
                if (collision.collider.tag == "zombie_leg")
                {
                    zomb.isCrawling = true;
                }
            }

            Destroy(gameObject);
            
           

        }

        else if (collision.gameObject.CompareTag("SWAT"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 position = contact.point;
            GetComponent<Collider>().enabled = false;

            SwatGuy swat = collision.gameObject.GetComponentInParent<SwatGuy>();
            if (!swat.isDead)
            {
                ParticleSystem blood = Instantiate(bloodPrefab, position, Quaternion.Euler(0, 0, 90));
                blood.transform.parent = collision.transform;
                blood.transform.localScale = new Vector3(1, 1, 1);

                swat.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        else
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 position = contact.point;
            GetComponent<Collider>().enabled = false;

            Instantiate(impact, contact.point, Quaternion.LookRotation(contact.normal));
        }
        
        Destroy(gameObject);

        
    }


}
