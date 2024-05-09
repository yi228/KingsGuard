using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnknownRoomEntrance : MonoBehaviour
{
    bool toUnknown = false;

    void Update()
    {
        if (toUnknown)
        {
            EnterRoom();
        }
    }

    void EnterRoom()
    {
        if (GameManager.instance.inRoom)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                GameManager.instance.inRoom = false;
                GameManager.instance.player.GetComponent<Transform>().position = new Vector2(GameManager.instance.player.GetComponent<Transform>().position.x - 100, GameManager.instance.player.GetComponent<Transform>().position.y);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                GameManager.instance.inRoom = true;
                GameManager.instance.player.GetComponent<Transform>().position = new Vector2(GameManager.instance.player.GetComponent<Transform>().position.x + 100, GameManager.instance.player.GetComponent<Transform>().position.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            toUnknown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            toUnknown = false;
        }
    }
}
