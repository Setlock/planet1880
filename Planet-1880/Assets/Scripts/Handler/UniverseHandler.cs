using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniverseHandler : MonoBehaviour
{
    public GameObject sun;
    public GameObject shipContainer;
    public GameObject planetPrefab;
    public GameObject shipPrefab;

    public int extraBodies = 0;
    public List<GameObject> bodies;

    public void Init(Player[] players)
    {
        bodies = new List<GameObject>();

        CreateBodies(players);
    }
    private void Update()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            Body body = bodies[i].GetComponent<Body>();
            if (body.GetOwner() != null)
            {
                body.TimeSpawnShip(shipPrefab, shipContainer);
                body.AddMoney();
            }
        }
    }
    private void FixedUpdate()
    {
        foreach(GameObject b in bodies)
        {
            Body body = b.GetComponent<Body>();
            body.Orbit(sun);
        }
    }
    private void LateUpdate()
    {
        CheckWin();
    }
    private void CheckWin()
    {
        bool win = true;
        Player lastPlayer = bodies[0].GetComponent<Body>().GetOwner();
        foreach (GameObject b in bodies)
        {
            Body body = b.GetComponent<Body>();
            if (lastPlayer != body.GetOwner())
            {
                win = false;
                break;
            }
            lastPlayer = body.GetOwner();
        }
        if (win)
        {
            Debug.Log(lastPlayer.ID + " Wins!");
        }
    }
    public int NumBodies(Player p)
    {
        int num = 0;
        foreach(GameObject bodyObject in bodies)
        {
            if(bodyObject.GetComponent<Body>().GetOwner() == p)
            {
                num++;
            }
        }
        return num;
    }
    public float countdown = 1;
    public void UpdateClaimAmounts()
    {
        foreach(GameObject body in bodies)
        {
            body.GetComponent<Body>().CalculateClaimSpeed();
        }
        foreach (GameObject body in bodies)
        {
            body.GetComponent<Body>().IncrementClaim();
        }
    }
    void CreateBodies(Player[] players)
    {
        for(int i = 0; i < players.Length+extraBodies; i++)
        {
            GameObject body = Instantiate(planetPrefab);

            body.GetComponent<Body>().orbitDist = i*(body.GetComponent<SpriteRenderer>().bounds.size.x*6)+(sun.GetComponent<SpriteRenderer>().bounds.size.x*3);
            body.GetComponent<Body>().CreateDefaultDictionaries(players);
            body.name = "Planet" + body.GetComponent<Body>().orbitDist;
            bodies.Add(body);
        }

        int[] chosenBodies = new int[players.Length];
        for(int i = 0; i < chosenBodies.Length; i++)
        {
            chosenBodies[i] = -1;
        }
        int numBody = -1;
        for(int i = 0; i < players.Length; i++)
        {
            while (chosenBodies.Contains(numBody))
            {
                numBody = (int)(UnityEngine.Random.value * bodies.Count);
            }
            chosenBodies[i] = numBody;

            bodies[numBody].GetComponent<Body>().Claim(players[i]);
        }
    }
    public void SpawnShip(GameObject body)
    {
        if(body != null)
        {
            foreach(GameObject b in bodies)
            {
                if(b == body)
                {
                    GameObject shipObject = Instantiate(shipPrefab, shipContainer.transform);
                    body.GetComponent<Body>().SpawnShip(shipObject);
                }
            }
        }
    }
    public GameObject GetBodyAtLocation(Vector2 pos)
    {
        GameObject outBody = null;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
        foreach (GameObject body in bodies)
        {
            if (hit.collider != null && hit.collider.Equals(body.GetComponent<CircleCollider2D>()))
            {
                return body;
            }
        }
        return outBody;
    }
}
