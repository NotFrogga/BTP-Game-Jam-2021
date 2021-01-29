using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] musicManagers = GameObject.FindGameObjectsWithTag("MusicManager");
        if (musicManagers.Length > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
