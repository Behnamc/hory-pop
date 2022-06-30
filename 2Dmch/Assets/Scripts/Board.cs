using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Board : MonoBehaviour
{
	public GameManager gm;
	public Board enemy;
	public PlayerCamera camera;

	public int rsize = 6;
	private const int firstYSize = 4, _rsize = 6;
	private const float timeChange = 0.3f;
	[HideInInspector] public bool canChange = true;

	private float blockZeroPos = -0.4f;

	// queue variables
	public int downInt = -1, topInt = 0, topBeadInt = 0;
	public Element down = null, top = null, topBead = null;
	
	private int size = 0;


	// queue functions
	private void pop_top()
	{
		if (top == null)
		{
			return;
		}
		else if (top.prev == null)
		{
			down = null;
			top = null;
			size = 0;
			return;
		}
		size--;
		top = top.prev;
		top.next = null;
	}
	private void pop_down()
	{
		if (down == null)
		{
			return;
		}
		else if (down.next == null)
		{
			down = null;
			top = null;
			size = 0;
			return;
		}
		size--;
		down = down.next;
		down.prev = null;
	}
	private void push_top(GameObject[] el)
	{
		size++;
		Element newElement = new Element(el);

		if (top == null)
		{
			top = newElement;
			down = newElement;
			return;
		}

		Element temp = top;
		top = newElement;
		temp.next = top;
		top.prev = temp;
		topInt++;
	}
	private void push_down(GameObject[] el)
	{
		size++;
		Element newElement = new Element(el);

		if (down == null)
		{
			top = newElement;
			down = newElement;
			return;
		}
		Element temp = down;
		down = newElement;
		temp.prev = down;
		down.next = temp;
		downInt--;
	}
	public void logPrint()
	{
		Element temp = top;
		Debug.Log("size:" + size);
		if (top == null)
		{
			return;
		}

		while (temp != null)
		{
			string a = "";
			for (int i = 0; i < rsize; i++)
			{
				if (temp.elements[i] != null)
					a += temp.elements[i].tag.ToString();
				else
					a += "#";
				a += " ";
			}
			Debug.Log(a);
			temp = temp.prev;
		}
	}
	// end queue functions

	public void createBoard()
	{
		(int, int)[] a = new (int, int)[_rsize] { (-1, -1), (-1, -1), (-1, -1), (-1, -1), (-1, -1), (-1, -1) };
		for (int i = 0; i < firstYSize; i++)
		{
			push_top(setRow(i, a));
		}
		topBead = top;
	}

	// create new row functions
	
	private int color((int, int) w, (int, int) z)
	{
		int c;
		do
		{
			c = Random.Range(0, gm.sprites.Length);
		} while ((c == w.Item1 && c == w.Item2) || (c == z.Item1 && c == z.Item2));

		return c;
	}
	public GameObject[] setRow(int j, (int, int)[] after)
	{
		GameObject[] a = new GameObject[rsize];

		(int, int) r = (-1, -1);
		for (int i = 0; i < rsize; i++)
		{
			int y = r.Item2;
			int y2 = after[i].Item2;
			after[i].Item2 = r.Item2 = color(r, after[i]);
			r.Item1 = y;
			after[i].Item1 = y2;

			a[i] = Instantiate(gm.bead,
						new Vector3(transform.position.x + gm.distance * i,
							transform.position.y + gm.distance * j, 0),

						Quaternion.identity,
						this.transform);
			a[i].GetComponent<SpriteRenderer>().sprite = gm.sprites[r.Item2];
			a[i].tag = r.Item2.ToString();
		}

		return a;
	}
	// end create new row functions


	// todo set not 3 in new block
	public void add()
	{
		// it must change
		(int, int)[] a = new (int, int)[rsize];
		for (int i = 0;i < rsize; i++)
        {
			if (down.elements[i] != null)
            {
				a[i].Item2 = int.Parse(down.elements[i].tag);
				if (down.next.elements[i] != null)
					a[i].Item1 = int.Parse(down.next.elements[i].tag);
			}
				
        }
		push_down(setRow(downInt, a));
		
	}

	// change functions
	
	public void setTopBoard()
	{
		while (top != null && top.isempty())
        {
			pop_top();
			topInt--;
        }

		Element empty = topBead;
		while (empty != null && empty.isempty())
		{
			empty = empty.prev;
			topBeadInt--;
		}

		topBead = empty;

		int kpos = topBeadInt;
		for (Element e = empty.next, k = empty.next; e != null; e = e.next)
		{
			if (e.elements[0] != null)
			{
				if (e != k)
				{
					e.elements[0].transform.DOMoveY(transform.position.y + kpos * gm.distance + blockZeroPos, gm.timeChange);

					(e.elements[0], k.elements[0]) = (k.elements[0], e.elements[0]);
					(e.elements[1], k.elements[1]) = (k.elements[1], e.elements[1]);
				}
				k = k.next;
				kpos++;
			}
		}
	}

	public void beadChangeVertical(Element y1, Element y2, int x, int y2pos)
	{
		y1.elements[x].transform.DOMoveY(transform.position.y + y2pos * gm.distance, gm.timeChange);

		(y1.elements[x], y2.elements[x]) = (y2.elements[x], y1.elements[x]);
	}

	public void setNewBoard()
	{
		// set beads
		for (int i = 0; i < rsize; i++)
		{
			int kpos = downInt + 1;
			for (Element e = down, k = down; 
				e != topBead.next;
				e = e.next)
			{
				if (e.elements[i] != null)
				{
					if (e != k)
					{
						beadChangeVertical(e, k, i, kpos);
					}
					k = k.next;
					kpos++;
				}
			}
		}
		// set blocks
		setTopBoard();
	}
	private List<(Element, int)> check1Element(Element e)
	{
		List<(Element, int)> result = new List<(Element, int)>();
		for (int i = 0; i < rsize - 2; i++)
		{
			if (e.elements[i] != null
			 && e.elements[i + 1] != null
			 && e.elements[i + 2] != null
			 && e.elements[i].tag == e.elements[i + 1].tag
			 && e.elements[i].tag == e.elements[i + 2].tag)
			{
				result.Add((e, i));
				result.Add((e, i + 1));
				result.Add((e, i + 2));
			}
		}
		return result;
	}

	private List<(Element, int)> check3Element(Element e1, Element e2, Element e3)
	{
		List<(Element, int)> result = new List<(Element, int)>();

		for (int i = 0; i < rsize; i++)
		{
			if (e1.elements[i] != null
			 && e2.elements[i] != null
			 && e3.elements[i] != null
			 && e1.elements[i].tag == e2.elements[i].tag
			 && e2.elements[i].tag == e3.elements[i].tag)
			{
				result.Add((e1, i));
				result.Add((e2, i));
				result.Add((e3, i));
			}
		}

		return result;
	}

    public void DestroyObj(Element e, int j)
    {
		gm.popAudio.PlayDelayed(.3f);
		Instantiate(gm.beadDestroy[int.Parse(e.elements[j].tag)],
			new Vector3(e.elements[j].transform.position.x,
				e.elements[j].transform.position.y, 0),
			Quaternion.identity,
			this.transform);

			Destroy(e.elements[j]);
	}

    public bool checkPop()
	{
		List<(Element, int)> pop = new List<(Element, int)>();
		Element e1 = topBead, e2 = null, e3 = null;
		if (e1 != null) { e2 = e1.prev; pop.AddRange(check1Element(e1)); }
		if (e2 != null) { e3 = e2.prev; pop.AddRange(check1Element(e2)); }

		while (e3 != null)
		{
			pop.AddRange(check1Element(e3));
			pop.AddRange(check3Element(e1, e2, e3));
			e1 = e2;
			e2 = e3;
			e3 = e3.prev;
		}
		int k = 0;
		foreach ((Element e, int j) in pop)
		{
			if (e == topBead && top != topBead) popBlocks();
			if (e.elements[j] != null) DestroyObj(e, j);
			k++;
		}
		// todo enemy must give BlockManager
		enemy.BlockManager(k);

		
		if (pop.Count == 0)
		{
			return false;
		}
		return true;
	}
	// end change functions
	

	// Punishment functions
	public void BlockManager(int d)
    {
		if (d <= 3) return;
		d = (d <= 5) ? 0: 1;
		// just one block
		d = 0;
		GameObject x = Instantiate(gm._block[d],
			new Vector3(transform.position.x + blockZeroPos,
				transform.position.y + gm.distance * topInt + blockZeroPos + 5, 0),
			Quaternion.identity,
			transform);

		x.tag = "Block";
		x.transform.DOMoveY(transform.position.y + gm.distance * topInt + blockZeroPos, timeChange);

		for (int j = 0; j <= d; j++)
        {
			GameObject[] a = new GameObject[rsize];
			a[0] = x;
			if (j != 1 || d == 0) { a[1] = x; }
			// twoBlock:
			// Block #     # # # #
			// Block Block # # # #

			// oneBlock:
			// Block Block # # # #
			push_top(a);
		}
		
		setNewBoard();
		
	}

	public void popBlocks()
    {
		(int, int)[] empty = { (-1, -1), (-1, -1), (-1, -1), (-1, -1) , (-1, -1), (-1, -1) };
		for (topBead = topBead.next;; topBead = topBead.next)
        {
			int alpha = (topBead.elements[1] != null
				&& topBead.next != null
				&& topBead.next.elements[1] == null
				) ? 1 : 0;

			if (topBead.elements[1] != null)
            {
				Instantiate(gm.blockDestroy[alpha],
					new Vector3(transform.position.x + blockZeroPos,
						transform.position.y + blockZeroPos + topBeadInt * gm.distance, 0),
					Quaternion.identity);
            }
				
			Destroy(topBead.elements[0]);
			topBead.elements = setRow(topBeadInt, empty);
			topBeadInt++;
			if (topBead.next == null) break;
		}

	}

	

	
	// end Punishment functions


}
