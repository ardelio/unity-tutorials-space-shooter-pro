using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] // This make the variable an attribute and hence available withing the Editor but still remains a private variable.
    private float _speed = 10f;

    [SerializeField]
    private GameObject _laserPrefab = null;

    [SerializeField]
    private GameObject _tripleShotPrefab = null;
        
    [SerializeField]
    private float _firingRate = 0.5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _laserContainer = null;

    private float _canShootAfter = -1f;
    private SpawnManager _spawnManager = null;
    private bool _isTripleShotActive = false;

    void Start()
    {
        transform.position = new Vector3(0, -3f, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        CalculateMovement();
        ShootLaser();
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        float topBorder = 0f;
        float bottomBorder = -3.8f;
        float leftBorder = -11.3f;
        float rightBorder = 11.3f;
        Vector3 playerPosition = transform.position;

       /* if (playerPosition.y >= topBorder)
        {
            transform.position = new Vector3(playerPosition.x, topBorder, playerPosition.z);
        }
        else if (playerPosition.y < bottomBorder)
        {
            transform.position = new Vector3(playerPosition.x, bottomBorder, playerPosition.z);
        }*/
        // optimised approach is to use clamping;

        transform.position = new Vector3(playerPosition.x, Mathf.Clamp(playerPosition.y, bottomBorder, topBorder), playerPosition.z);

        if (playerPosition.x <= leftBorder)
        {
            transform.position = new Vector3(rightBorder, playerPosition.y, playerPosition.z);
        }
        else if (playerPosition.x >= rightBorder)
        {
            transform.position = new Vector3(leftBorder, playerPosition.y, playerPosition.z);
        }
    }

    private void ShootLaser()
    {
        bool spaceKeyIsPressed = Input.GetKeyDown(KeyCode.Space);
        bool cannotShoot = Time.time <= _canShootAfter;
        GameObject newLaser;

        if (!spaceKeyIsPressed || cannotShoot)
        {
            return;
        }

        _canShootAfter = Time.time + _firingRate;

        if (_isTripleShotActive)
        {
            Vector3 spawnPosition = transform.position;
            newLaser = Instantiate(_tripleShotPrefab, spawnPosition, Quaternion.identity);
        } else
        {
            Vector3 offset = new Vector3(0, 1.05f, 0);
            Vector3 spawnPosition = transform.position + offset;
            newLaser = Instantiate(_laserPrefab, spawnPosition, Quaternion.identity);
        }
        
        newLaser.transform.SetParent(_laserContainer.transform);
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);

        _isTripleShotActive = false;
    }
}
