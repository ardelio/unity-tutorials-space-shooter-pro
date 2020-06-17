using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private PowerUpId _powerUpId = PowerUpId.TRIPLE_SHOT;

    public enum PowerUpId
    {
        TRIPLE_SHOT = 0,
        SPEED = 1,
        SHIELD = 2,
    }

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
                ActivatePowerUp(player);
            }
            Destroy(gameObject);
        }
    }

    private void ActivatePowerUp(Player player)
    {
        switch(_powerUpId)
        {
            case PowerUpId.TRIPLE_SHOT:
                player.ActivateTripleShot();
                break;
            case PowerUpId.SPEED:
                player.ActivateSpeedBoost();
                break;
            case PowerUpId.SHIELD:
                player.ActivateShield();
                break;
            default:
                throw new System.Exception("ActivatePowerUp Default case should not be reached");
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
