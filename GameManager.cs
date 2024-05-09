using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public int sceneIndex; //�� �ε���
    public Image atkPotionOn; //�������� ���� �� ȿ�� ǥ�� �̹���
    public Vector2[] spawnPos; //�� �̵��� �÷��̾� ��ġ
    public bool inRoom = false; //���� �뿡 ������
    public Image talkPanel;
    public bool talkOn = false;
    public bool[] talkEnd;
    public bool meetKing = false;
    public bool kingDead = false;
    public Image help;
    public bool helpOn = false;
    public Image restartImage;
    public Image endingImage;
    public bool restart = false;
    public Text helpText;

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        instance = this;
        sceneIndex = 1;

        talkEnd = new bool[5]; //0: ��1 ����, 1: ��3 �ٽ���, 2: ��5 ��(��), 3: ��5 ��(��), 4: ��5 �ٽ���
        for(int i = 0; i < 5; i++)
        {
            talkEnd[i] = false;
        }
    }

    private void Start()
    {
        talkOn = true;
        restartImage.gameObject.SetActive(false);
        endingImage.gameObject.SetActive(false);
        helpText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (talkOn)
        {
            talkPanel.gameObject.SetActive(true);
            player.isTalk = true;
        }
        else
        {
            talkPanel.gameObject.SetActive(false);
            player.isTalk=false;
        }

        if (player.dead)
        {
            if (!talkEnd[4])
                Invoke("RestartImageOn", 2f);
            else
                Invoke("EndingImageOn", 2f);

        }
    }

    public void HelpOn()
    {
        if (help.gameObject.activeSelf)
        {
            helpOn = false;
            help.gameObject.SetActive(false);
        }
        else
        {
            helpOn = true;
            help.gameObject.SetActive(true);
        }
    }

    void RestartImageOn()
    {
        restartImage.gameObject.SetActive(true);
        helpText.gameObject.SetActive(false);
    }

    void EndingImageOn()
    {
        endingImage.gameObject.SetActive(true);
        helpText.gameObject.SetActive(false);
    }

    public void Restart()
    {
        restart = true;
        SceneManager.LoadScene(0);
        restartImage.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
