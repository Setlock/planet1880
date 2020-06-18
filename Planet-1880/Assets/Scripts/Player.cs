using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string ID = "N/A";
    public Color color;
    public int money = 200;
    public Player(string ID, Color color)
    {
        this.ID = ID;
        this.color = color;
    }
}
