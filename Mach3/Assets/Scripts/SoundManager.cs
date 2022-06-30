using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource MatchBattleDynDrm;
    [SerializeField] private AudioSource MatchBattleStaDrm;
    [SerializeField] private AudioSource MatchBattleDynSng;
    [SerializeField] private AudioSource MatchBattleStaSng;
    [SerializeField] private AudioSource losAudio;

    private float timeRange = 1.8462f;
    private float timeChange;
    private bool IsDrm = false, changeDrum = false;
    private bool IsSng = false, changeSng = false;
    private void Start()
    {
        timeChange = Time.time;

        MatchBattleDynSng.enabled = true;
        MatchBattleDynSng.Play();
        MatchBattleDynSng.volume = 0;

        MatchBattleStaSng.enabled = true;
        MatchBattleStaSng.Play();

        MatchBattleDynDrm.enabled = true;
        MatchBattleDynDrm.Play();
        MatchBattleDynDrm.volume = 0;

        MatchBattleStaDrm.enabled = true;
        MatchBattleStaDrm.Play();

    }

    private void Update()
    {
        updateAllSounds();
    }

    private void updateAllSounds()
    {
        if (timeChange <= Time.time)
        {
            if (changeDrum)
            {
                int a = IsDrm ? 1 : 0;
                MatchBattleDynDrm.volume = 1 - a;
                MatchBattleStaDrm.volume = a;
                Debug.Log("IsDrm" + IsDrm);
                IsDrm = !IsDrm;
                changeDrum = false;
            }
            // 
            if (changeSng)
            {
                int a = IsSng ? 1 : 0;
                MatchBattleDynSng.volume = 1 - a;
                MatchBattleStaSng.volume = a;
                Debug.Log("IsSng" + IsSng);
                IsSng = !IsSng;
                changeSng = false;
            }
            //
            timeChange += timeRange;
        }
    }


    public void SwichDrmDym(bool h)
    {
        if (h != IsDrm)
            changeDrum = true;
    }

    public void SwichSng(bool h)
    {
        if (h != IsSng)
            changeSng = true;

    }


    public void endAll()
    {
        MatchBattleDynSng.Pause();
        MatchBattleStaSng.Pause();
        MatchBattleDynDrm.Pause();
        MatchBattleStaDrm.Pause();
    }

}
