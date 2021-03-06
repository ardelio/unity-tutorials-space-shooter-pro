﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] // This make the variable an attribute and hence available withing the Editor but still remains a private variable.
    private float _speed = 5f;

    [SerializeField]
    private int _speedBoostMultiplier = 2;

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

    [SerializeField]
    private GameObject _shieldVisualiser = null;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private GameObject[] _engines = null;

    [SerializeField]
    private AudioClip _laserShotAudioClip = null;

    private float _canShootAfter = -1f;
    private SpawnManager _spawnManager = null;
    private UIManager _uiManager = null;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    private AudioSource _audioSource = null;

    public void ActivateShield()
    {
        enableShield(true);
    }

    public void ActivateSpeedBoost()
    {
        _speed *= _speedBoostMultiplier;

        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            enableShield(false);
            return;
        }

        _lives--;

        DamageEngines();

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            GameOver();
        }
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void Start()
    {
        transform.position = new Vector3(0, -3f, 0);
        _spawnManager = GetComponentOrThrow<SpawnManager>(FindGameObjectOrThrow("SpawnManager").transform);
        _uiManager = GetComponentOrThrow<UIManager>(FindGameObjectOrThrow("Canvas").transform);
        _audioSource = GetComponentOrThrow<AudioSource>(transform);

        _uiManager.UpdateScore(_score);

        if (_engines == null || _engines.Length != 2)
        {
            throw new ArgumentNullException("Engines not added not found");
        }

        foreach (GameObject engine in _engines)
        {
            engine.SetActive(false);
        }
    }

    private void Update()
    {
        Move();
        EnforceBounds();
        ShootLaser();
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

    private void DamageEngines()
    {
        if (_lives >= 1)
        {
            _engines[_lives - 1].SetActive(true);
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void EnforceBounds()
    {
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

    private void GameOver()
    {
        _spawnManager.OnPlayerDeath();
        Destroy(gameObject);
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

        PlayClip(_laserShotAudioClip);
        
        newLaser.transform.SetParent(_laserContainer.transform);
    }

    private void PlayClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);

        _speed /= _speedBoostMultiplier;
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);

        _isTripleShotActive = false;
    }

    private void enableShield(bool isEnabled)
    {
        _isShieldActive = isEnabled;
        _shieldVisualiser.transform.gameObject.SetActive(isEnabled);
    }
}
