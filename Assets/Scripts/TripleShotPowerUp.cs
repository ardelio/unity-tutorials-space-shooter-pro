using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotPowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    void Update()
    {
        MoveDown();
        DestroyIfOffScreen();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.ActivateTripleShot();
            }
            Destroy(gameObject);
        }
    }

    private void DestroyIfOffScreen()
    {
        bool isOffScreen = transform.position.y < -8f;

        if (isOffScreen)
        {
            Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
