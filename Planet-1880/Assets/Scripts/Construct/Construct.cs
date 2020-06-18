using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construct : MonoBehaviour
{
    public GameObject body;
    public int price;
    public Rigidbody2D rb;
    public virtual void Action()
    {
        Debug.Log("Default Action");
    }
}
