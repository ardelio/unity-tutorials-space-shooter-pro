using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private Animator _animator = null;
    private Player _player = null;
    private bool _isAlive = true;

    void Start()
    {
        _animator = GetComponentOrThrow<Animator>(transform);
        _player = GetPlayer();
    }

    void Update()
    {
        MoveDownwards();
        ResetWithRandomXCoord();
        DestroyIfNoPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }
            DestructionSequence();
        }
        else if (other.CompareTag("Laser"))
        {

            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddToScore(10);
            }

            DestructionSequence();
        }
    }

    private void DestructionSequence()
    {
        _isAlive = false;
        _animator.SetTrigger("OnEnemyDeath");
        Destroy(gameObject, 3f);
    }

    private Player GetPlayer()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");

        if (playerGameObject == null)
        {
            throw new System.ArgumentNullException("Cannot find GameObject with tag Player");
        }

        return GetComponentOrThrow<Player>(playerGameObject.transform);
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

    private void MoveDownwards()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void ResetWithRandomXCoord()
    {
        float heightFromCentre = 8f;
        float horizontalWidthFromCenter = 9.3f;
        float randomXPosition = Random.Range(-horizontalWidthFromCenter, horizontalWidthFromCenter);
        bool isOffScreen = transform.position.y < -heightFromCentre;

        if (isOffScreen && _isAlive)
        {
            transform.position = new Vector3(randomXPosition, heightFromCentre, transform.position.z);
        }
    }

    private void DestroyIfNoPlayer()
    {
        if (_player == null)
        {
            Destroy(gameObject);
        }
    }
}
