using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

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
}
