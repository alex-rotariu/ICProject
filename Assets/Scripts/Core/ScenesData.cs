using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScenesData
{
    private static Sprite chosenPolitician;
    public static void SetPolitician(Sprite politician) {
        chosenPolitician = politician;
    }
    
    public static Sprite GetPolitician() {
        return chosenPolitician;
    }
}
