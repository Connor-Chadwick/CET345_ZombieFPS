using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    public Transform itemSpawnPoint;

    public GameObject[] items;

    public Animator animator;

    public int minItems;
    public int maxItems;
    public int noOfItemsInCrate;
    public float spawnRotation;
    private Quaternion itemSpawnRotation;
    private bool opened;
    private void Start()
    {
        noOfItemsInCrate = Random.Range(minItems, maxItems);
        animator = GetComponent<Animator>();
    }
    public void OpenCrate()
    {
        animator.SetBool("open", true);
        GetComponent<BoxCollider>().isTrigger = true;
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        if (!opened)
        {
            opened = true;
            yield return new WaitForSeconds(2.75f);
            for (int i = 0; i < noOfItemsInCrate * 2; i++)
            {
                spawnRotation = Random.Range(-40, 40);
                itemSpawnRotation = Quaternion.Euler(0, 0, spawnRotation);
                GameObject spawnedItem = Instantiate(items[Random.Range(0, items.Length)], itemSpawnPoint.transform.position, itemSpawnPoint.transform.rotation * itemSpawnRotation);
                spawnedItem.GetComponent<Rigidbody>().AddRelativeForce(0, 150f, 150f);

                i++;
                yield return new WaitForSeconds(0.5f);

            }
        }
        
     
    }
   
}
