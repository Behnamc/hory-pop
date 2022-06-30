using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject Help;
    [SerializeField] private GameObject AboutUs;
    [SerializeField] private GameObject[] readyPlayer;
    [SerializeField] private GameObject[] readyPlayerText;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color deActColor;

    private (string, float)[] levels = { 
        ("Grand Master", 600f),
        ("Pro", 900f), 
        ("Newbie", 1500f), 
        ("Smol Baby", 4000f)
    };
    private int[] playerSpeed = {3, 3};
    [SerializeField] private Text[] text;
    // 600, 900, 1200, 2000
    public void StartGame()
    {

        Scores.increasewin1 = false;
        Scores.increasewin2 = false;
        Scores.WinPlayer1 = 0;
        Scores.WinPlayer2 = 0;
        Scores.speedPlayer1 = levels[playerSpeed[0]].Item2;
        Scores.speedPlayer2 = levels[playerSpeed[1]].Item2;
        deActive(0);
        deActive(1);
        SceneManager.LoadScene("Game");
    }

    public void HelpGame()
    {
        if (Help.active)
        {
            Help.SetActive(false);
        }
        else
        {
            Help.SetActive(true);
        }
    }

    public void AboutUsGame()
    {
        if (AboutUs.active)
        {
            AboutUs.SetActive(false);
        }
        else
        {
            AboutUs.SetActive(true);
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    private void active(int a)
    {
        readyPlayer[a].GetComponent<Image>().color = activeColor;
        readyPlayerText[a].GetComponent<Text>().color = deActColor;
        readyPlayerText[a].GetComponent<Text>().text = "Ready";
    }

    private void deActive(int a)
    {
        readyPlayer[a].GetComponent<Image>().color = deActColor;
        readyPlayerText[a].GetComponent<Text>().color = activeColor;
        readyPlayerText[a].GetComponent<Text>().text = "Not Ready";
    }

    private void Update()
    {
        StartCoroutine(keys());
    }

    private IEnumerator keys()
    {
        if (Input.GetKeyDown(KeyCode.S) && playerSpeed[0] + 1 < levels.Length)
        {
            playerSpeed[0]++;
            setTexts();
        }
        if (Input.GetKeyDown(KeyCode.W) && playerSpeed[0] > 0)
        {
            playerSpeed[0]--;
            setTexts();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && playerSpeed[1] + 1 < levels.Length)
        {
            playerSpeed[1]++;
            setTexts();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && playerSpeed[1] > 0)
        {
            playerSpeed[1]--;
            setTexts();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
            Debug.Log("Quit");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            active(0);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            deActive(0);
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            active(1);
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            deActive(1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Scores.changeMusic = true;
        }

        if (readyPlayer[0].GetComponent<Image>().color == activeColor
            && readyPlayer[1].GetComponent<Image>().color == activeColor)
        {
            yield return new WaitForSeconds(.5f);
            StartGame();
        }
    }

    private void setTexts()
    {
        text[0].text = levels[playerSpeed[0]].Item1;
        text[1].text = levels[playerSpeed[1]].Item1;
    }

    

}
