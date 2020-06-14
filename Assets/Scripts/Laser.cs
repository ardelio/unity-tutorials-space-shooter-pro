using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    void Update()
    {
        MoveUpwards();
        DestroyIfOffScreen();
    }

    private void MoveUpwards()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void DestroyIfOffScreen()
    {
        float laserYPosition = transform.position.y;
        bool isOffScreen = laserYPosition > 8f;
        if (isOffScreen)
        {
            Destroy(gameObject);
        }
    }
}
