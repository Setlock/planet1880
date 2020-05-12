using UnityEditor;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public UniverseHandler universe;
    PlayerController pc;
    Player[] players;
    public int numPlayers = 2;
    private void Start()
    {
        universe = GetComponent<UniverseHandler>();
        players = new Player[numPlayers];
        players[0] = new Player("Default", Color.blue);
        players[1] = new Player("Enemy", Color.red);
        universe.Init(players);

        pc = new PlayerController(this, players[0]);
    }
    private void Update()
    {
        pc.Update();
    }
    public void SetConstructToBodyEdge(GameObject body, GameObject constructObject)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2((mousePos.y - body.transform.position.y), (mousePos.x - body.transform.position.x));
        float xPos = body.transform.position.x + Mathf.Cos(angle) * (body.GetComponent<SpriteRenderer>().bounds.size.x/2);
        float yPos = body.transform.position.y + Mathf.Sin(angle) * (body.GetComponent<SpriteRenderer>().bounds.size.y/2);
        Vector2 finalPos = new Vector2(xPos, yPos);
        constructObject.transform.position = finalPos;

        constructObject.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90, Vector3.forward);
    }
    private void PlaceConstruct(GameObject constructObject)
    {

    }
}
