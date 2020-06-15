﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    void Update()
    {
        MoveDownwards();
        ResetWithRandomXCoord();
    }

    private void OnTriggerEnter(Collider other)
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
            Destroy(gameObject);
        }
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
}
