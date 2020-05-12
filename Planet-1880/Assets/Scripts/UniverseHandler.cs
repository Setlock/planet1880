using System.Collections.Generic;
using UnityEngine;

public class UniverseHandler : MonoBehaviour
{
    public GameObject sun;
    public GameObject shipContainer;
    public GameObject planetPrefab;
    public GameObject shipPrefab;

    public int extraBodies = 0;
    List<GameObject> bodies;

    public void Init(Player[] players)
    {
        bodies = new List<GameObject>();

        CreateBodies(players);
    }
    private void Update()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].GetComponent<Body>().Orbit(sun);
            bodies[i].GetComponent<Body>().ShipOrbit();
        }
    }
    void CreateBodies(Player[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            GameObject body = Instantiate(planetPrefab);

            body.GetComponent<Body>().Claim(players[i]);

            body.GetComponent<Body>().orbitDist = i*(body.GetComponent<SpriteRenderer>().bounds.size.x*10)+(sun.GetComponent<SpriteRenderer>().bounds.size.x*3);
            body.GetComponent<Body>().CreateDefaultDictionaries(players);
            body.name = "Planet" + body.GetComponent<Body>().orbitDist;
            bodies.Add(body);
        }
        for(int i = 0; i < extraBodies; i++)
        {
            GameObject body = Instantiate(planetPrefab);
            body.GetComponent<Body>().orbitDist = i * (body.GetComponent<SpriteRenderer>().bounds.size.x * 10) + (sun.GetComponent<SpriteRenderer>().bounds.size.x * 3);
            body.GetComponent<Body>().CreateDefaultDictionaries(players);
            body.name = "Planet" + body.GetComponent<Body>().orbitDist;
            bodies.Add(body);
        }
    }
    public GameObject GetBodyAtLocation(Vector2 pos)
    {
        GameObject outBody = null;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
        foreach(GameObject body in bodies)
        {
            if (hit.collider != null && hit.collider.Equals(body.GetComponent<CircleCollider2D>()))
            {
                return body;
            }
        }
        return outBody;
    }
    public void SpawnShip(GameObject body)
    {
        if(body != null)
        {
            foreach(GameObject b in bodies)
            {
                if(b == body)
                {
                    body.GetComponent<Body>().SpawnShip(Instantiate(shipPrefab, shipContainer.transform));
                }
            }
        }
    }
}
