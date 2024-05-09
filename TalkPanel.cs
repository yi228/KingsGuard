using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkPanel : MonoBehaviour
{
    public Text talkText;
    public Text nameText;

    public List<string> talkList = new List<string>();

    int talkIndex = 0;
    int dialIndex = 0;

    private void Awake()
    {
        gameObject.SetActive(false);
    }


    private void Update()
    {
        string[] splitTalk = talkList[talkIndex].Split('^');
        string name = splitTalk[0];
        string talk = splitTalk[1];

        talkText.text = talk;
        nameText.text = name;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            talkIndex++;
            if (splitTalk.Length>2)
            {
                Debug.Log("talkEnd");
                GameManager.instance.talkEnd[dialIndex] = true;
                dialIndex++;
                GameManager.instance.talkOn = false;
            }
        }
    }
}
