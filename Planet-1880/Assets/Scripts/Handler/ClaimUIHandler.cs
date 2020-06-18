using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClaimUIHandler : MonoBehaviour
{
    public GameObject canvas;
    public GameObject progressBarPrefab;
    Dictionary<Player, GameObject> progressBarDictionary = new Dictionary<Player, GameObject>();

    public void CreateProgressBars(Player[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            GameObject progressBarInstance = Instantiate(progressBarPrefab, canvas.transform);
            progressBarInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -(10 + i * 25));
            progressBarInstance.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 15);
            progressBarInstance.transform.GetChild(0).GetComponent<Image>().color = players[i].color;
            progressBarInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].ID;
            progressBarInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = players[i].color;
            progressBarDictionary.Add(players[i], progressBarInstance);
        }
    }
    public void UpdateProgressBars(Body b)
    {
        EnableProgressBars();
        foreach(Player p in b.claimLevel.Keys)
        {
            progressBarDictionary[p].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((b.claimLevel[p]/b.maxClaimLevel)*progressBarDictionary[p].GetComponent<RectTransform>().sizeDelta.x, 15);
        }
        if (b.GetOwner() != null)
        {
            progressBarDictionary[b.GetOwner()].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(150, 15);
        }
    }
    public void EnableProgressBars()
    {
        foreach(GameObject progressBar in progressBarDictionary.Values)
        {
            progressBar.SetActive(true);
        }
    }
    public void DisableProgressBars()
    {
        foreach (GameObject progressBar in progressBarDictionary.Values)
        {
            progressBar.SetActive(false);
        }
    }
}
