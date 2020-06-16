using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab = null;

    [SerializeField]
    private GameObject _enemyContainer = null;

    [SerializeField]
    private GameObject _powerUpPrefab = null;

    [SerializeField]
    private GameObject _powerUpContainer = null;

    [SerializeField]
    private float _enemySpawnRateInSeconds = 5f;

    private bool _canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(_canSpawn)
        {
            float yPosition = 8f;
            float zPosition = 0f;
            float widthFromCentre = 9.3f;
            float randomXPosition = Random.Range(-widthFromCentre, widthFromCentre);
            Vector3 startPosition = new Vector3(randomXPosition, yPosition, zPosition);
            GameObject newEnemy = Instantiate(_enemyPrefab, startPosition, Quaternion.identity);
            newEnemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(_enemySpawnRateInSeconds);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_canSpawn)
        {
            float yPosition = 8f;
            float zPosition = 0f;
            float widthFromCentre = 9.3f;
            float randomXPosition = Random.Range(-widthFromCentre, widthFromCentre);
            Vector3 startPosition = new Vector3(randomXPosition, yPosition, zPosition);
            GameObject newPowerUp = Instantiate(_powerUpPrefab, startPosition, Quaternion.identity);
            newPowerUp.transform.SetParent(_powerUpContainer.transform);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _canSpawn = false;
    }
}
