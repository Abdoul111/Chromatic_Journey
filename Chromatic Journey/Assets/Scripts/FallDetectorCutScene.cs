using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FallDetectorCutScene : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.transform.position = new Vector3(-5.5f, -0.6f, 0f);
        }
    }

    
}
