using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    public static PopupText Instance;

    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;
    private bool isPopupActive = false;
    private void Awake()
    {
        // Ensure only one instance of PopupManager exists.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }
    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
        isPopupActive = true;
    }
//    public void ClosePopUp()
//   {
//        if (isPopupActive)
//        {
//            Debug.Log("Button clicked!");
//            animator.SetTrigger("close");
//            popUpBox.SetActive(false);
//        }
//   }
}
