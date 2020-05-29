using UnityEditor;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public UniverseHandler universe;
    PlayerController pc;
    Player[] players;
    public int numPlayers = 2;
    public GameObject turretObject;
    private void Start()
    {
        universe = GetComponent<UniverseHandler>();
        players = new Player[numPlayers];
        players[0] = new Player("Default", Color.cyan);
        for (int i = 1; i < numPlayers; i++)
        {
            players[i] = new Player("COMP" + i, new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
        }
        universe.Init(players);
        GetComponent<PlayerClaimUI>().CreateProgressBars(players);

        pc = new PlayerController(this, players[0]);
    }
    private void Update()
    {
        pc.Update();
        universe.UpdateClaimAmounts();
    }
    public void SetConstructToBodyEdge(GameObject body, GameObject constructObject)
    {

    }
    private void PlaceConstruct(GameObject constructObject)
    {

    }
}
