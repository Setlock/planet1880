using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string ID = "N/A";
    public Color color;
    public Player(string ID, Color color)
    {
        this.ID = ID;
        this.color = color;
    }
}
