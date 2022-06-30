using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    private Tween tween;
    public float speed;
    
    void Start()
    {
        tween = transform.DOMoveY(-100f, speed);
    }

    public void stop()
    {
        tween.Pause();
    }

    public void go()
    {
        tween.Play();
    }
}
