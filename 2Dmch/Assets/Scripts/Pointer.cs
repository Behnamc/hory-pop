using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pointer : MonoBehaviour
{
    private const int rsize = 6;
    private const float timeMove = 0.15f;
    private const float timeChange = 0.2f;
    private const float timeAnimRemove = 0.8f;

    [SerializeField] private Board board;
    [SerializeField] private UnityEngine.KeyCode rightKey;
    [SerializeField] private UnityEngine.KeyCode leftKey;
    [SerializeField] private UnityEngine.KeyCode upKey;
    [SerializeField] private UnityEngine.KeyCode downKey;
    [SerializeField] private UnityEngine.KeyCode changeKey;
    public PlayerCamera camera;
    // where i am in board
    private Element pointer;
    private (int, int) pos = (0, 0);
    [SerializeField] private float zeroX;
    [SerializeField] private float zeroY;
    private float adding = 1f;


    private void Start()
    {
        board.createBoard();
        pointer = board.down;
        zeroX = transform.position.x;
        zeroY = transform.position.y;
    }

    void Update()
    {
        StartCoroutine(Move());
        if (camera.transform.position.y <= adding)
        {
            adding -= board.gm.distance;
            board.add();
        }
    }

    

    private IEnumerator Move()
    {
        
        if (Input.GetKeyDown(rightKey) && pos.Item1 < rsize - 2)
        {
            pos.Item1++;
            transform.DOMoveX(zeroX + pos.Item1 * board.gm.distance, timeMove);
        }
        if (Input.GetKeyDown(leftKey) && pos.Item1 > 0)
        {
            pos.Item1--;
            transform.DOMoveX(zeroX + pos.Item1 * board.gm.distance, timeMove);
        }
        if (Input.GetKeyDown(upKey) && pointer.next != null)
        {
            pos.Item2++;
            transform.DOMoveY(zeroY + pos.Item2 * board.gm.distance, timeMove);
            pointer = pointer.next;
        }
        if (Input.GetKeyDown(downKey) && pointer.prev != null)
        {
            pos.Item2--;
            transform.DOMoveY(zeroY + pos.Item2 * board.gm.distance, timeMove);
            pointer = pointer.prev;
        }

        if (Input.GetKeyDown(changeKey) && board.canChange)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
            board.canChange = false;
            beadChangeHorizontal();
            yield return new WaitForSeconds(timeChange);
            board.canChange = true;
            board.setNewBoard();
            yield return new WaitForSeconds(board.gm.timeChange);

            // if delete one row and pointer is in . what will happen?


            
            while (board.checkPop())
            {
                camera.stop();
                yield return new WaitForSeconds(timeAnimRemove - .2f);
                board.setNewBoard();
                yield return new WaitForSeconds(board.gm.timeChange);
            }

            this.GetComponent<SpriteRenderer>().color = Color.white;
            
            camera.go();
            
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            board.BlockManager(4);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            board.BlockManager(5);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            board.logPrint();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            board.setTopBoard();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("board.down" + board.down.log() + " -- board.downInt" + board.downInt);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("board.top" + board.top.log() + " -- board.topInt" + board.topInt);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        { 
            Debug.Log("board.topBead" + board.topBead.log() + " -- board.topInt" + board.topBeadInt);
        }
    }



    private void beadChangeHorizontal()
    {
        if (!((pointer.elements[pos.Item1] != null && pointer.elements[pos.Item1].tag == "Block")
         || (pointer.elements[pos.Item1 + 1] != null && pointer.elements[pos.Item1 + 1].tag == "Block"))
        )
        {
            if (pointer.elements[pos.Item1] != null)
            {
                // transform.position.x + gm.distance * i
                pointer.elements[pos.Item1].transform.DOMoveX(
                    board.transform.position.x + (pos.Item1 + 1) * board.gm.distance, timeChange);
            }
            if (pointer.elements[pos.Item1 + 1] != null)
            {
                pointer.elements[pos.Item1 + 1].transform.DOMoveX(
                    board.transform.position.x + pos.Item1 * board.gm.distance, timeChange);
            }
            board.gm.changeAudio.Play();
            (pointer.elements[pos.Item1], pointer.elements[pos.Item1 + 1]) =
            (pointer.elements[pos.Item1 + 1], pointer.elements[pos.Item1]);
        }

    }



}
