using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    private enum GameState
    {
        PAUSE = 0,
        RUN = 1,
    }

    void Update()
    {
        bool restartKeyIsPressed = Input.GetKey(KeyCode.R);

        if (_isGameOver && restartKeyIsPressed)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = (float)GameState.PAUSE;
    }

    public void ResumeGame()
    {
        Time.timeScale = (float)GameState.RUN;
    }
}
