using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [HideInInspector] public static int WinPlayer1 = 0;
    [HideInInspector] public static int WinPlayer2 = 0;
    [HideInInspector] public static bool increasewin1 = false;
    [HideInInspector] public static bool increasewin2 = false;
    [HideInInspector] public static float speedPlayer1 = 1000f;
    [HideInInspector] public static float speedPlayer2 = 1000f;
    [HideInInspector] public static bool changeMusic = false;
}
