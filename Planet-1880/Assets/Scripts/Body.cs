using Packages.Rider.Editor.PostProcessors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Body : MonoBehaviour
{
    Player owner;
    [HideInInspector]
    public Dictionary<Player, float> claimLevel = new Dictionary<Player, float>();
    public Dictionary<Player, float> claimSpeed = new Dictionary<Player, float>();
    [HideInInspector]
    public Dictionary<Player, List<GameObject>> ships = new Dictionary<Player, List<GameObject>>();
    [HideInInspector]
    public List<GameObject> constructs = new List<GameObject>();
    public float orbitDist,orbitSpeed;
    public Rigidbody2D rb;

    float orbitAngle;
    public float maxClaimLevel = 500;
    float spawnShipCountdown = 3;
    int decayRate = -4;
    void Start()
    {
        orbitAngle = UnityEngine.Random.value * 360;
        rb = GetComponent<Rigidbody2D>();
    }
    public GameObject tempConstruct;
    bool placingConstruct = false;
    public void SetConstructLocation(float x, float y)
    {
        if (placingConstruct)
        {
            float angle = Mathf.Atan2((y - transform.position.y), (x - transform.position.x));
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
        tempConstruct.GetComponent<Construct>().body = gameObject;
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
            ships.Add(p, new List<GameObject>());
        }
    }
    public void SetShipsToMove(Player p, GameObject location)
    {
        foreach(GameObject ship in ships[p].ToList())
        {
            if(ship.GetComponent<Ship>().owner == p)
            {
                ship.GetComponent<Ship>().SetLocationToMove(location);
            }
        }
    }
    public void MoveShipsToObject(Player p, GameObject location)
    {
        foreach(GameObject ship in ships[p])
        {
            ship.GetComponent<Ship>().MoveToBody(location);
        }
    }
    public void Orbit(GameObject body)
    {
        orbitSpeed = 1000/orbitDist;
        rb.MovePosition(new Vector2(body.transform.position.x + Mathf.Cos(orbitAngle*Mathf.Deg2Rad)*orbitDist, body.transform.position.y + Mathf.Sin(orbitAngle* Mathf.Deg2Rad) *orbitDist));
        orbitAngle += orbitSpeed * Time.deltaTime;
        if(orbitAngle >= 360)
        {
            orbitAngle = 0;
        }
    }
    public void TimeSpawnShip(GameObject shipPrefab, GameObject container)
    {
        spawnShipCountdown -= Time.deltaTime;
        if(spawnShipCountdown <= 0)
        {
            GameObject shipObject = Instantiate(shipPrefab, container.transform);
            SpawnShip(shipObject);
            spawnShipCountdown = 2;
        }
    }
    float addMoneyCountdown = 10;
    public void AddMoney()
    {
        addMoneyCountdown -= Time.deltaTime;
        if(addMoneyCountdown <= 0)
        {
            GetOwner().money += 10;
            addMoneyCountdown = 10;
        }
    }
    public void IncrementClaim()
    {
        foreach(Player p in claimLevel.Keys.ToList())
        {
            if (p != owner)
            {
                claimLevel[p] += claimSpeed[p] * Time.deltaTime;
                if(claimLevel[p] < 0)
                {
                    claimLevel[p] = 0;
                }
                if (claimLevel[p] >= maxClaimLevel)
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
                float playerClaimSpeed = decayRate;

                if (ships[p].Count > 0)
                {
                    playerClaimSpeed = ships[p].Count/2f;
                }

                claimSpeed[p] = playerClaimSpeed;
            }
        }
    }
    public void SpawnShip(GameObject ship)
    {
        ship.GetComponent<Ship>().owner = owner;
        ship.transform.position = transform.position;
        ship.GetComponent<SpriteRenderer>().color = owner.color;
        ship.GetComponent<Ship>().SetBodyToOrbit(gameObject);
        ship.GetComponent<Ship>().orbitDist = GetComponent<SpriteRenderer>().bounds.size.y + 2 + (10*UnityEngine.Random.value);
        ship.GetComponent<Ship>().orbitAngle = 360 * UnityEngine.Random.value;

        ships[owner].Add(ship);
    }
    public void AddShip(Player p, GameObject ship)
    {
        ship.GetComponent<Ship>().SetBodyToOrbit(gameObject);
        ships[p].Add(ship);
    }
    public void RemoveShip(Player p, GameObject ship)
    {
        ships[p].Remove(ship);
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
    public bool BodyWillBeClaimedByEnemy()
    {
        int gunCount = constructs.Count;
        foreach(Player p in ships.Keys)
        {
            if(p != owner)
            {
                int numShips = ships[p].Count;
                for(int i = 0; i < 10; i++)
                {
                    numShips -= gunCount;
                }

                if(numShips > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public int PlayerShipAmount(Player p)
    {
        return ships[p].Count;
    }
    public Player GetOwner()
    {
        return owner;
    }
}
