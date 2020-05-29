using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClaimUI : MonoBehaviour
{
    public GameObject canvas;
    public GameObject progressBarPrefab;
    Dictionary<Player, GameObject> progressBarDictionary = new Dictionary<Player, GameObject>();

    public void CreateProgressBars(Player[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            GameObject progressBarInstance = Instantiate(progressBarPrefab, canvas.transform);
            progressBarInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -(10 + i * 15));
            progressBarInstance.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 10);
            progressBarInstance.transform.GetChild(0).GetComponent<Image>().color = players[i].color;
            progressBarInstance.transform.GetChild(1).GetComponent<Text>().text = players[i].ID;
            progressBarInstance.transform.GetChild(1).GetComponent<Text>().color = players[i].color;
            progressBarDictionary.Add(players[i], progressBarInstance);
        }
    }
    public void UpdateProgressBars(Body b)
    {
        foreach(Player p in b.claimLevel.Keys)
        {
            progressBarDictionary[p].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((b.claimLevel[p]/b.maxClaimLevel)*progressBarDictionary[p].GetComponent<RectTransform>().sizeDelta.x, 10);
        }
        if (b.GetOwner() != null)
        {
            progressBarDictionary[b.GetOwner()].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 10);
        }
    }
}
