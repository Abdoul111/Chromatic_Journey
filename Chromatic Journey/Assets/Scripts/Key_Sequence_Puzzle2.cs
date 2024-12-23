using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key_Sequence_Puzzle2 : MonoBehaviour
{
    public GameObject puzzlePanel;
    public TextMeshProUGUI currentWordLabel;

    float currentTimeWait = 0f;
    float maxTimeWait = 2f;
    bool playerWon = false;

    private MovingPlatform movingPlatform;

    private string targetSequence = "ADADADWASD";
    private string currentInput = "";
    private bool puzzleAlreadySolved = false;

    private PlayerMovement PlayerMovement;

    private void Start()
    {
        // Get the MovingPlatform component from the parent
        movingPlatform = GetComponentInParent<MovingPlatform>();
        targetSequence = targetSequence.ToUpper();
    }


    void Update()
    {
        if (puzzlePanel.activeSelf && !playerWon)
        {
            foreach (char c in Input.inputString)
            {
                Debug.Log($"Key pressed: {c}");
                currentInput += char.ToUpper(c);
                currentWordLabel.text = currentInput;

                Debug.Log($"Current input sequence: {currentInput}");

                if (!targetSequence.StartsWith(currentInput))
                {
                    Debug.Log("Wrong input! Resetting sequence.");
                    currentWordLabel.color = Color.red;
                    currentInput = "";
                }
                else if (currentInput == targetSequence)
                {
                    Debug.Log("Correct sequence entered!");
                    currentWordLabel.color= Color.green;
                    playerWon = true;
                    break;
                }
            }
        }

        if (playerWon)
        {
            currentTimeWait += Time.deltaTime;
            if (currentTimeWait >= maxTimeWait)
            {
                currentTimeWait = 0f;
                playerWon = false;
                CompletePuzzle();
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (puzzleAlreadySolved)
            {
                Debug.Log("Puzzle already solved. Ignoring trigger.");
                return; // Exit early if the puzzle is already solved
            }

            Debug.Log("Player has entered the puzzle zone.");
            puzzlePanel.SetActive(true);
            PlayerMovement = FindObjectOfType<PlayerMovement>();
            PlayerMovement.isBusy = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player has left the puzzle zone.");
            puzzlePanel.SetActive(false);
        }
    }

    void CompletePuzzle()
    {
        Debug.Log("Puzzle solved! Moving platform.");
        puzzlePanel.SetActive(false);
        movingPlatform.TriggerMovement(); // Trigger platform movement
        puzzleAlreadySolved = true;
        PlayerMovement.isBusy = false;
    }

}
