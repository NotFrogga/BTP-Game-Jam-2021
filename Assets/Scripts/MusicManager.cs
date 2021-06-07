using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTPGameJam;

public class MusicManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        SetUniqueMusicManager();
    }

    /// <summary>
    /// Sets unique music manager.
    /// In case there is a load level feature
    /// </summary>
    private void SetUniqueMusicManager()
    {
        GameObject[] musicManagers = GameObject.FindGameObjectsWithTag(Constants.Tags.MUSIC_MANAGER);
        if (musicManagers.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
