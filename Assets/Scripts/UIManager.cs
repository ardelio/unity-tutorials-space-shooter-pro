using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText = null;
    [SerializeField]
    private Text _gameOverText = null;
    [SerializeField]
    private Text _restartText = null;
    [SerializeField]
    private Image _livesImg = null;
    [SerializeField]
    private Sprite[] _livesSprites = null;

    private GameManager _gameManager = null;

    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        
        GameObject gameManagerGameObject = GameObject.Find("GameManager");

        if (gameManagerGameObject == null)
        {
            throw new System.ArgumentNullException("[UIManager] GameManager game object not found");
        }

        _gameManager = gameManagerGameObject.GetComponent<GameManager>();

        if (_gameManager == null)
        {
            throw new System.ArgumentNullException("[UIManager] GameManager component not found");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        StartCoroutine(GameOverTextFlickerRoutine());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    private IEnumerator GameOverTextFlickerRoutine()
    {
        while(true)
        {
            bool currentStatus = _gameOverText.gameObject.activeSelf;
            _gameOverText.gameObject.SetActive(!currentStatus);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
