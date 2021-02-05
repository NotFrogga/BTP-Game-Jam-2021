using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{

    public bool canRestart = true;
    public bool changeScene = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RestartLevel();
        ChangeScene();
        TriggerChangeScene();
    }

    private void TriggerChangeScene()
    {
        if (Input.anyKey && !canRestart)
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void RestartLevel()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Changes scene after a level is finished. If last scene is finished, go to first scene
    public void ChangeScene()
    {
        if (changeScene)
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        } 
    }


    /// <summary>
    /// Checks if any rock is moving
    /// </summary>
    /// <returns>True if at least one rock is moving, false if there are no rocks or no rock is moving</returns>
    public bool CheckIfRocksAreMoving()
    {
        List<GameObject> rocksGo = GameObject.FindGameObjectsWithTag("Rock").ToList() ;

        if (rocksGo.Any())
        {
            foreach (GameObject rockGo in rocksGo)
            {
                if (rockGo.GetComponent<RockMovement>().rockMoving)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
