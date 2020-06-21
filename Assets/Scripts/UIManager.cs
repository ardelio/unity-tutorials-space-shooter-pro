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
    [SerializeField]
    private GameObject _exitGamePanel = null;

    private GameManager _gameManager = null;

    private void Start()
    {
        _gameManager = GetComponentOrThrow<GameManager>(FindGameObjectOrThrow("GameManager").transform);
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _exitGamePanel.SetActive(false);
    }

    private void Update()
    {
        WasEscapeKeyPressed();
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

    public void CloseExitPanel()
    {
        _exitGamePanel.SetActive(false);
        _gameManager.ResumeGame();
    }

    private GameObject FindGameObjectOrThrow(string name)
    {
        GameObject gameObject = GameObject.Find(name);

        if (gameObject == null)
        {
            throw new System.ArgumentNullException($"Cannot find GameObject {name}");
        }

        return gameObject;
    }

    private T GetComponentOrThrow<T>(Transform _transform)
    {
        T component = _transform.GetComponent<T>();

        if (component == null)
        {
            throw new System.ArgumentNullException($"The {typeof(T).FullName} component does not exist on the {_transform.name} GameObject.");
        }

        return component;
    }

    private void GameOver()
    {
        StartCoroutine(GameOverTextFlickerRoutine());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    private void WasEscapeKeyPressed()
    {
        bool escapeKeyWasPressed = Input.GetKeyDown(KeyCode.Escape);

        if (escapeKeyWasPressed)
        {
            _exitGamePanel.SetActive(true);
            _gameManager.PauseGame();
        }
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
