using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Element
{
    public Element prev = null, next = null;
    public GameObject[] elements = new GameObject[6];

    public Element(GameObject[] el)
    {
        elements = el;
    }

    public bool isempty()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i] != null) return false;
        }
        return true;
    }

    public IEnumerable<int> elementsTag()
    {
        for(int i = 0; i < elements.Length; i++)
        {
            yield return  int.Parse(elements[i].tag);
        }
    }

    public string log()
    {
        string a = "";
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i] != null)
                a += elements[i].tag;
            else
                a += '#';
        }
        return a;
    }

}
