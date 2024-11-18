using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Sequence_Puzzle : MonoBehaviour
{
    public GameObject puzzlePanel;
    private string targetSequence = "UNITY";
    private string currentInput = "";

    void Update()
    {
        if (puzzlePanel.activeSelf)
        {
            foreach (char c in Input.inputString)
            {
                Debug.Log($"Key pressed: {c}");
                currentInput += char.ToUpper(c);

                Debug.Log($"Current input sequence: {currentInput}");

                if (!targetSequence.StartsWith(currentInput))
                {
                    Debug.Log("Wrong input! Resetting sequence.");
                    currentInput = "";
                }
                else if (currentInput == targetSequence)
                {
                    Debug.Log("Correct sequence entered!");
                    CompletePuzzle();
                    break;
                }
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the puzzle zone.");
            puzzlePanel.SetActive(true);
        }
    }

    void CompletePuzzle()
    {
        Debug.Log("Puzzle solved! Moving platform.");
        puzzlePanel.SetActive(false);
        MovePlatform();
    }

    void MovePlatform()
    {
        Debug.Log("Platform moving to the next position.");
        // Implement platform movement logic here.

    }

}
