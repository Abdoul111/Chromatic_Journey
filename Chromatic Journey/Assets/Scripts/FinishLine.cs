using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public coinHandler coinHandler; // Reference to the coinHandler script

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if all coins are collected
            if (coinHandler != null && coinHandler.AllCoinsCollected())
            {
                SceneManager.LoadScene("Level2");
            }
            else
            {
                Debug.Log("Not all coins collected!");
            }
        }
    }
}
