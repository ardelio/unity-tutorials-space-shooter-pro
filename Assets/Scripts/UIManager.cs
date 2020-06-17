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
    private Image _livesImg = null;
    [SerializeField]
    private Sprite[] _livesSprites = null;

    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
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
