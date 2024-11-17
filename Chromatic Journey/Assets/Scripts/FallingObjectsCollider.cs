using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectsCollider : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthCounter.Damage(10);
            Debug.Log("Player Damaged");
        }

        Destroy(gameObject);
    }

}
