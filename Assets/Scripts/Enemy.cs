using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player = null;

    void Start()
    {
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

            Destroy(gameObject);
        }
        else if (other.CompareTag("Laser"))
        {

            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            Destroy(gameObject);
        }
    }

    private Player GetPlayer()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");

        if (playerGameObject == null)
        {
            throw new System.ArgumentNullException("Cannot find GameObject with tag Player");
        }

        Player player = playerGameObject.transform.GetComponent<Player>();

        if (player == null)
        {
            throw new System.ArgumentNullException("Cannot get Player component");
        }

        return player;
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

        if (isOffScreen)
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
