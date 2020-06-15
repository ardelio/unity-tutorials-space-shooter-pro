using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _spawnRateInSeconds = 5f;

    private bool _canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
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
            yield return new WaitForSeconds(_spawnRateInSeconds);
        }
    }

    public void OnPlayerDeath()
    {
        _canSpawn = false;
    }
}
