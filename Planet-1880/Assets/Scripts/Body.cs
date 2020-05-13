using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Body : MonoBehaviour
{
    Player owner;
    [HideInInspector]
    public Dictionary<Player, int> claimLevel = new Dictionary<Player, int>();
    public Dictionary<Player, int> claimSpeed = new Dictionary<Player, int>();
    [HideInInspector]
    public List<GameObject> ships = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> constructs = new List<GameObject>();
    public float orbitDist,orbitSpeed;

    float orbitAngle;
    void Start()
    {
        orbitAngle = UnityEngine.Random.value * 360;
    }
    public GameObject tempConstruct;
    bool placingConstruct = false;
    public void SetConstructLocation()
    {
        if (placingConstruct)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x));
            float xPos = transform.position.x + Mathf.Cos(angle) * (GetComponent<SpriteRenderer>().bounds.size.x / 2);
            float yPos = transform.position.y + Mathf.Sin(angle) * (GetComponent<SpriteRenderer>().bounds.size.y / 2);
            Vector2 finalPos = new Vector2(xPos, yPos);
            
            tempConstruct.transform.position = finalPos;
            tempConstruct.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90, Vector3.forward);
        }
    }
    public void StartConstructPlacement(GameObject construct)
    {
        placingConstruct = true;
        tempConstruct = Instantiate(construct);
        tempConstruct.transform.parent = transform;
    }
    public void PlaceConstruct()
    {
        placingConstruct = false;
        constructs.Add(tempConstruct);
    }
    public void DestroyTempConstruct()
    {
        placingConstruct = false;
        Destroy(tempConstruct);
    }
    public void CreateDefaultDictionaries(Player[] players)
    {
        foreach(Player p in players)
        {
            claimLevel.Add(p, 0);
            claimSpeed.Add(p, 0);
        }
    }
    public void SetShipsToMove(Player p, GameObject location)
    {
        foreach(GameObject ship in ships.ToList())
        {
            if(ship.GetComponent<Ship>().owner == p)
            {
                ship.GetComponent<Ship>().SetLocationToMove(location);
            }
        }
    }
    public void MoveShipsToObject(GameObject location)
    {
        foreach(GameObject ship in ships)
        {
            ship.GetComponent<Ship>().MoveToBody(location);
        }
    }
    public void ShipOrbit()
    {
        foreach(GameObject ship in ships)
        {
            ship.GetComponent<Ship>().SetBodyToOrbit(gameObject);
            ship.GetComponent<Ship>().Orbit();
        }
    }
    public void Orbit(GameObject body)
    {
        orbitSpeed = 1000/orbitDist;
        transform.position = new Vector2(body.transform.position.x + Mathf.Cos(orbitAngle*Mathf.Deg2Rad)*orbitDist, body.transform.position.y + Mathf.Sin(orbitAngle* Mathf.Deg2Rad) *orbitDist);
        orbitAngle += orbitSpeed * Time.deltaTime;
        if(orbitAngle >= 360)
        {
            orbitAngle = 0;
        }
    }
    float spawnCountdown = 5;
    int decayRate = -1;
    public void TimeSpawnShip(GameObject shipPrefab, GameObject container)
    {
        spawnCountdown -= Time.deltaTime;
        if(spawnCountdown <= 0)
        {
            SpawnShip(Instantiate(shipPrefab, container.transform));
            spawnCountdown = 5;
        }
    }
    public void IncrementClaim()
    {
        foreach(Player p in claimLevel.Keys.ToList())
        {
            if (p != owner)
            {
                claimLevel[p] += claimSpeed[p];
                if(claimLevel[p] < 0)
                {
                    claimLevel[p] = 0;
                }
                if (claimLevel[p] >= 500)
                {
                    Claim(p);
                    ResetAllClaimLevels();
                }
            }
        }
    }
    public void ResetAllClaimLevels()
    {
        foreach(Player p in claimLevel.Keys.ToList())
        {
            claimLevel[p] = 0;
        }
    }
    public void CalculateClaimSpeed()
    {
        foreach(Player p in claimLevel.Keys)
        {
            if (p != owner)
            {
                int playerClaimSpeed = decayRate;
                foreach (GameObject ship in ships)
                {
                    if (ship.GetComponent<Ship>().owner == p)
                    {
                        playerClaimSpeed++;
                    }
                }
                if(playerClaimSpeed != decayRate)
                {
                    playerClaimSpeed += (-decayRate);
                }

                claimSpeed[p] = playerClaimSpeed;
            }
        }
    }
    public void SpawnShip(GameObject ship)
    {
        ship.GetComponent<Ship>().owner = owner;
        ship.GetComponent<SpriteRenderer>().color = owner.color;
        ship.GetComponent<Ship>().orbitDist = GetComponent<SpriteRenderer>().bounds.size.y + 2 + (10*UnityEngine.Random.value);
        ship.GetComponent<Ship>().orbitAngle = 360 * UnityEngine.Random.value;
        ships.Add(ship);
    }
    public void AddShip(GameObject ship)
    {
        ships.Add(ship);
    }
    public void RemoveShip(GameObject ship)
    {
        ships.Remove(ship);
    }
    public void Claim(Player p)
    {
        this.owner = p;
        GetComponent<SpriteRenderer>().color = p.color;
        /*foreach(GameObject ship in ships)
        {
            ship.GetComponent<Ship>().owner = p;
            ship.GetComponent<SpriteRenderer>().color = p.color;
        }*/
    }
    public Player GetOwner()
    {
        return owner;
    }
}
