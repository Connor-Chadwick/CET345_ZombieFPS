using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject[] obsToNotDestroy;
    public GameObject eleDoor;
    void Start()
    {
      

        
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    

    

    

    public void Scene2()
    {
        for (int i = 0; i < obsToNotDestroy.Length; i++)
        {
            DontDestroyOnLoad(obsToNotDestroy[i]);
        }
        StartCoroutine(loadnewscene());
    }

    IEnumerator loadnewscene()
    {
        yield return new WaitForSeconds(3f);
        //eleDoor.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //yield return new WaitForSeconds(1f);
        //SceneManager.LoadSceneAsync(1);
    }
}
