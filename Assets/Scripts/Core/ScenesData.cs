using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// STATIC => PRESENT IN ALL SCENES
public static class ScenesData
{   
    private static Player playerStats;

    public static void SetCharacter(int index)
    {
        playerStats.character = index;
    }

    public static void SetPlayer(Player player)
    {
        playerStats = player;
    }

    public static Player GetPlayer()
    {
        return playerStats;
    }

    public static int GetPolitician()
    {
        return playerStats.character;
    }

}
