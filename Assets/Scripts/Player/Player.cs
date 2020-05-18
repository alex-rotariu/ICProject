using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Player
{
    public string username;
    public ulong money;
    public ulong moneyPerSecond;
    public ulong moneyPerClick;
    public int character;
}
