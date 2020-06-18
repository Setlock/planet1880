using UnityEditor;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public UniverseHandler universe;
    PlayerController pc;
    AIController[] aiControllers;
    public Player[] players;
    public int numPlayers = 3;
    public GameObject turretObject;
    private void Start()
    {
        universe = GetComponent<UniverseHandler>();
        players = new Player[numPlayers];
        //Make this easier to change between all AI and 1 player
        //players[0] = new Player("Default", Color.cyan);
        aiControllers = new AIController[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            players[i] = new Player("COMP" + i, new Color(UnityEngine.Random.value + 0.15f, UnityEngine.Random.value + 0.15f, UnityEngine.Random.value + 0.15f));
            aiControllers[i] = new AIController(this,players[i], i*100 + System.DateTime.Now.Millisecond + System.DateTime.Now.Second);
        }
        universe.Init(players);
        GetComponent<ClaimUIHandler>().CreateProgressBars(players);

        pc = new PlayerController(this, players[0]);
    }
    private void Update()
    {
        universe.UpdateClaimAmounts();
    }
    private void LateUpdate()
    {
        pc.Update();
        foreach (AIController ai in aiControllers)
        {
            ai.Update();
        }
    }
    public void SetConstructToBodyEdge(GameObject body, GameObject constructObject)
    {

    }
    private void PlaceConstruct(GameObject constructObject)
    {

    }
}
