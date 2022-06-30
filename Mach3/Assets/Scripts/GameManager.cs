using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public SoundManager sm;
    public GameObject bead;
    public GameObject[] beadDestroy;
    public GameObject[] _block;
    public GameObject[] blockDestroy;
    public float distance = 0.8f;
    public float timeChange = 0.3f;

    public Sprite[] sprites;

    [SerializeField] private GameObject[] Star;
    [SerializeField] private float StarXpos1;
    [SerializeField] private float StarXpos2;
    [SerializeField] private float StarYpos;
    public AudioSource changeAudio;
    public AudioSource popAudio;
    [SerializeField] private PlayerCamera[] playerCamera;
    [SerializeField] private Board[] playerBoard;
    [SerializeField] private Pointer[] playerPointer;
    [SerializeField] private GameObject curtain;
    [SerializeField] private GameObject stillDre;
    private bool endGame = false;

    private void Start()
    {
        if (Scores.changeMusic)
        {
            sm.enabled = false;
            stillDre.SetActive(true);
        }
        playerCamera[0].speed = Scores.speedPlayer1;
        playerCamera[1].speed = Scores.speedPlayer2;
        setIncreaseWin();
        Loadwinners();
        
    }

    private void Update()
    {

        rungame();


    }

    private void rungame()
    {
        // 1.5
        // 2.5
        // player 2

        // player 1
        if (playerBoard[1].transform.position.y + playerBoard[1].topInt * playerBoard[1].gm.distance >= playerCamera[1].transform.position.y + 2.5f
            || playerBoard[0].transform.position.y + playerBoard[0].topInt * playerBoard[0].gm.distance >= playerCamera[0].transform.position.y + 2.5f)
        {
            sm.SwichDrmDym(true);
            sm.SwichSng(true);
        }
        else if (playerBoard[1].transform.position.y + playerBoard[1].topInt * playerBoard[1].gm.distance >= playerCamera[1].transform.position.y + 1.5f
           || playerBoard[0].transform.position.y + playerBoard[0].topInt * playerBoard[0].gm.distance >= playerCamera[0].transform.position.y + 1.5f)
        {
            sm.SwichSng(false);
            sm.SwichDrmDym(true);
        }
        else
        {
            sm.SwichDrmDym(false);
            sm.SwichSng(false);
        }

        for (int i = 0; i <= 1; i++ )
        {
            if (playerBoard[i].transform.position.y + playerBoard[i].topInt * playerBoard[i].gm.distance >= playerCamera[i].transform.position.y + 4.4f)
            {
                StopAll();
                StartCoroutine(GameOver(i));
            }
        }
        


    }


    private void Loadwinners()
    {
        
            int a = Scores.WinPlayer1;
            Instantiate(Star[a], new Vector3(StarXpos1 - .15f, StarYpos, 0), Quaternion.identity);
            Instantiate(Star[0], new Vector3(StarXpos1 - .15f, StarYpos - 1.2f, 0), Quaternion.identity);
        
        
            a = Scores.WinPlayer2;
            Instantiate(Star[a], new Vector3(StarXpos2, StarYpos, 0), Quaternion.identity);
            Instantiate(Star[0], new Vector3(StarXpos2, StarYpos - 1.2f, 0), Quaternion.identity);
        
    }

    

    private void setIncreaseWin()
    {
        if (Scores.increasewin1)
        {
            Scores.WinPlayer1++;
        }
        if (Scores.increasewin2)
        {
            Scores.WinPlayer2++;
        }
        Scores.increasewin1 = false;
        Scores.increasewin2 = false;
    }

    private IEnumerator GameOver(int a)
    {
        yield return new WaitForSeconds(.5f);
        Element e = playerBoard[a].down;

        while (e != null)
        {
            if (e != null)
            {
                for (int i = 0; i < playerBoard[a].rsize; i++)
                {
                    if (e.elements[i] != null)
                    {
                        popAudio.Play();
                        if (e.elements[i].tag != "Block")
                            playerBoard[a].DestroyObj(e, i);
                        else
                            Destroy(e.elements[i]);
                    }
                }
            }
            yield return new WaitForSeconds(.1f);
            e = e.next;
        }
        sm.endAll();
        
        
        if (a == 0) Scores.increasewin2 = true;
        else Scores.increasewin1 = true;
        if (Scores.WinPlayer1 == 1 && a != 0 && endGame == false)
        {
            endGame = true;
            curtain.transform.position = new Vector3(playerCamera[0].transform.position.x + .71f,
                playerCamera[0].transform.position.y + 9f,
                0);

            curtain.transform.DOMoveY(0.35f + playerCamera[0].transform.position.y, 5f).OnComplete(EndGame);
        }
        else if(Scores.WinPlayer2 == 1 && a == 0 && endGame == false)
        {
            endGame = true;
            curtain.transform.position = new Vector3(playerCamera[1].transform.position.x - .67f,
                playerCamera[1].transform.position.y + 9f,
                0);

            curtain.transform.DOMoveY(0.35f + playerCamera[1].transform.position.y, 5f).OnComplete(EndGame);
        }
        else if (endGame == false)
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Game");
        }
    }

    private void EndGame()
    {
        SceneManager.LoadScene("Menu");
    }

    private void StopAll()
    {
        playerCamera[0].stop();
        playerCamera[1].stop();
        playerPointer[0].enabled = false;
        playerPointer[1].enabled = false;
        playerBoard[0].enabled = false;
        playerBoard[1].enabled = false;
    }

}
