using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using BTPGameJam;

public class GameManagement : MonoBehaviour
{

    public bool isNotUIScene = true;
    public bool levelFinished = false;

    // Update is called once per frame
    void Update()
    {
        RestartLevel();
        ChangeSceneIfLevelFinished();
        ChangeSceneUI();
    }


    #region Scene Management
    /// <summary>
    /// Trigger a scene change in UI scenes
    /// First and Last Scenes
    /// </summary>
    private void ChangeSceneUI()
    {
        if (Input.anyKey && !isNotUIScene)
        {
            ChangeScene();
        }
    }

    /// <summary>
    /// Restarts a game level scene
    /// </summary>
    private void RestartLevel()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isNotUIScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// Changes scene after a level is finished. If last scene is finished, go to first scene
    /// </summary>
    public void ChangeSceneIfLevelFinished()
    {
        if (levelFinished)
        {
            ChangeScene();
        }
    }

    private static void ChangeScene()
    {
        bool gameInLastScene = SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings;
        if (gameInLastScene)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    #endregion


    #region Game Management
    /// <summary>
    /// Checks if any rock is moving
    /// </summary>
    /// <returns>True if at least one rock is moving, false if there are no rocks or no rock is moving</returns>
    public bool CheckIfRocksAreMoving()
    {
        List<GameObject> rocksGo = GameObject.FindGameObjectsWithTag(Constants.Tags.ROCK).ToList() ;

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
    #endregion
}
