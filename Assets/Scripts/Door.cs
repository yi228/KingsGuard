using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    bool doorEnter = false;

    private void Update()
    {
        if (doorEnter)
        {
            if(SceneManager.GetActiveScene().name =="Scene 3")
            {
                if (GameManager.instance.talkEnd[1])
                {
                    GameManager.instance.helpText.text = "Press Shift to enter the door.";
                    GameManager.instance.helpText.gameObject.SetActive(true);
                    SceneTransition();
                }
            }
            else
            {
                GameManager.instance.helpText.text = "Press Shift to enter the door.";
                GameManager.instance.helpText.gameObject.SetActive(true);
                SceneTransition();
            }
                
        }
    }

    void SceneTransition()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            GameManager.instance.helpText.gameObject.SetActive(false);
            GameManager.instance.sceneIndex++;
            GameManager.instance.player.transform.position = GameManager.instance.spawnPos[GameManager.instance.sceneIndex -2];
            SceneManager.LoadScene("Scene "+GameManager.instance.sceneIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.helpText.gameObject.SetActive(false);
            doorEnter = false;
        }
    }
}
