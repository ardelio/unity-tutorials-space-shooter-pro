using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 19f;
    [SerializeField]
    private GameObject _explosionPrefab = null;
    private Animator _explosionAnimator = null;
    private SpawnManager _spawnManager = null;
    private bool _alive = true;

    private void Start()
    {
        _explosionAnimator = _explosionPrefab.transform.GetComponent<Animator>();

        if (_explosionAnimator == null)
        {
            throw new ArgumentNullException("The Animator component does not exist on the _explosionPrefab GameObject.");
        }

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            throw new ArgumentNullException("The SpawnManager component does not exist on the SpawnManager GameObject.");
        }
    }

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") && _alive)
        {
            _alive = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.25f);

        }
    }
}
