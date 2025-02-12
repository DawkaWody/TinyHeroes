using System.Collections.Generic;
using UnityEngine;

public class GLOBALS
{
    public static string playerTag = "Player";
    public static List<string> playerColorNames = new List<string>() { "blue", "pink", "white" };
    public static List<Color> playerColors = new List<Color>() { new Color(0.235f, 0.588f, 0.858f, 1),
                                                                new Color(0.956f, 0.537f, 0.964f, 1), 
                                                                new Color(0.988f, 0.996f, 0.996f, 1) };
    public static float[] hitShakeForceMultipliers = new float[] { 1.0f, 1.25f, 1.5f};
}
