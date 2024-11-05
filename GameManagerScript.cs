using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject CharacterUI;

    public void gameOver()
    {
        CharacterUI.SetActive(false);
        gameOverUI.SetActive(true);

    }

    public void restart()
    {
        Debug.Log("Restart button clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
