using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBackScript : MonoBehaviour
{
    public float speed = 2f; // скорость движения рыбки

    // Update is called once per frame
    void Update()
    {
        // передвинуть рыбку налево
        transform.position += Vector3.left * Time.deltaTime * speed;

        // удалить рыбку, когда достигнута минимальная Y-позиция
        if (transform.position.y <= -4 || transform.position.x <= -13)
        {
            Destroy(gameObject);
        }
    }
}
