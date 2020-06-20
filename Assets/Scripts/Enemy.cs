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
    private AudioSource _audioSource = null;

    void Start()
    {
        _animator = GetComponentOrThrow<Animator>(transform);
        _audioSource = GetComponentOrThrow<AudioSource>(transform);
        _player = GetComponentOrThrow<Player>(FindGameObjectOrThrow("Player").transform);
    }

    void Update()
    {
        MoveDownwards();
        ResetWithRandomXCoord();
        DestroyIfNoPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isAlive)
        {
            return;
        }

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
        _audioSource.Play();
        Destroy(gameObject, 3f);
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
