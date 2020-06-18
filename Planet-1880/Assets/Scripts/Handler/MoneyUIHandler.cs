using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUIHandler : MonoBehaviour
{
    public GameObject moneyText;
    public GameHandler handler;
    public void Update()
    {
        moneyText.GetComponent<TextMeshProUGUI>().text = "Money: " + handler.players[0].money + "$";
    }
}
