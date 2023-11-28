using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigBoss : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scene 3")
        {
            if(Mathf.Abs(GameManager.instance.player.GetComponent<Transform>().position.x - transform.position.x) <= 2f)
            {
                GameManager.instance.helpText.text = "Press F to talk.";
                GameManager.instance.helpText.gameObject.SetActive(true);
            }
            else
            {
                GameManager.instance.helpText.gameObject.SetActive(false);
            }

            if (!GameManager.instance.talkEnd[1] && Input.GetKey(KeyCode.F) && Mathf.Abs(GameManager.instance.player.GetComponent<Transform>().position.x - transform.position.x) <= 2f)
            {
                GameManager.instance.talkOn = true;
            }
        }

        if (SceneManager.GetActiveScene().name == "Scene 5")
        {
            if(Mathf.Abs(GameManager.instance.player.GetComponent<Transform>().position.x - transform.position.x) <= 2f)
            {
                GameManager.instance.helpText.text = "Press F to talk.";
                GameManager.instance.helpText.gameObject.SetActive(true);
            }
            else
            {
                GameManager.instance.helpText.gameObject.SetActive(false);
            }

            if (!GameManager.instance.talkEnd[4] && Input.GetKey(KeyCode.F) && Mathf.Abs(GameManager.instance.player.GetComponent<Transform>().position.x - transform.position.x) <= 2f)
            {
                GameManager.instance.talkOn = true;
            }
            if (GameManager.instance.talkEnd[4])
            {
                anim.SetTrigger("Attack");
                GameManager.instance.player.hp = 0;
            }
        }
    }
}
