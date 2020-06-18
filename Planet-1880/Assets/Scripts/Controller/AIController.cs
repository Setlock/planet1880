using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class AIController
{
    public GameHandler handler;
    public Player myPlayer;
    System.Random r;
    public AIController(GameHandler handler, Player p, int seed)
    {
        this.handler = handler;
        this.myPlayer = p;
        r = new System.Random(seed);
    }
    bool foundTargetBody = false;
    float timeRandomEvent = 10;
    GameObject stockpilePlanet;
    public void Update()
    {
        int numBodies = handler.universe.NumBodies(myPlayer);
        if (numBodies > 0 && numBodies < handler.universe.bodies.Count)
        {
            UpdateStockpilePlanet();

            DefendPlanetUnderAttack();

            MoveAvailableShipsToTargetPlanet();

            timeRandomEvent -= Time.deltaTime;
            if (!foundTargetBody)
            {
                SelectPlanet();
                if (timeRandomEvent <= 0)
                {
                    if (!couldMoveToRandom)
                    {
                        StockpileShips();
                    }
                    timeRandomEvent = 10;
                }
            }
        }
    }
    public void DefendPlanetUnderAttack()
    {
        foreach (GameObject bodyObject in handler.universe.bodies)
        {
            Body body = bodyObject.GetComponent<Body>();
            if (body.GetOwner() == myPlayer)
            {
                if (body.BodyWillBeClaimedByEnemy())
                {
                    if (myPlayer.money >= handler.turretObject.GetComponent<Construct>().price)
                    {
                        body.GetComponent<Body>().StartConstructPlacement(handler.turretObject);
                        body.GetComponent<Body>().SetConstructLocation(body.transform.position.x + (UnityEngine.Random.value * 2 - 1) * 10, body.transform.position.y + (UnityEngine.Random.value * 2 - 1) * 10);
                        body.GetComponent<Body>().PlaceConstruct();
                        myPlayer.money -= handler.turretObject.GetComponent<Construct>().price;
                    }
                }
            }
        }
    }
    public void MoveAvailableShipsToTargetPlanet()
    {
        foundTargetBody = false;
        foreach (GameObject bodyObject in handler.universe.bodies)
        {
            Body body = bodyObject.GetComponent<Body>();
            if (body.GetOwner() != myPlayer && body.PlayerShipAmount(myPlayer) > 0)
            {
                foreach (GameObject bodyObjectFind in handler.universe.bodies)
                {
                    if (bodyObject != bodyObjectFind && (bodyObjectFind.GetComponent<Body>().PlayerShipAmount(myPlayer)+bodyObject.GetComponent<Body>().PlayerShipAmount(myPlayer)) >= bodyObject.GetComponent<Body>().constructs.Count * 10 + 5)
                    {
                        bodyObjectFind.GetComponent<Body>().SetShipsToMove(myPlayer, bodyObject);
                    }
                }
                foundTargetBody = true;
                break;
            }
        }
    }
    public bool couldMoveToRandom = false;
    public void SelectPlanet()
    {
        couldMoveToRandom = false;
        GameObject randomBody = null;
        foreach (GameObject bodyObjectFind in handler.universe.bodies)
        {
            if(bodyObjectFind.GetComponent<Body>().GetOwner() == null && r.Next(4) == 0)
            {
                randomBody = bodyObjectFind;
            }
        }
        if (randomBody == null)
        {
            randomBody = handler.universe.bodies[(int)(UnityEngine.Random.value * handler.universe.bodies.Count)];
            while (randomBody.GetComponent<Body>().GetOwner() == myPlayer)
            {
                randomBody = handler.universe.bodies[(int)(UnityEngine.Random.value * handler.universe.bodies.Count)];
            }
        }

        foreach (GameObject bodyObjectFind in handler.universe.bodies)
        {
            if (randomBody != bodyObjectFind && bodyObjectFind.GetComponent<Body>().PlayerShipAmount(myPlayer) >= randomBody.GetComponent<Body>().constructs.Count * 10 + 5)
            {
                bodyObjectFind.GetComponent<Body>().SetShipsToMove(myPlayer, randomBody);
                couldMoveToRandom = true;
            }
        }
    }
    public void UpdateStockpilePlanet()
    {
        if(stockpilePlanet  == null || stockpilePlanet.GetComponent<Body>().GetOwner() != myPlayer)
        {
            GameObject randomBody = handler.universe.bodies[(int)(UnityEngine.Random.value * handler.universe.bodies.Count)];
            while (randomBody.GetComponent<Body>().GetOwner() != myPlayer)
            {
                randomBody = handler.universe.bodies[(int)(UnityEngine.Random.value * handler.universe.bodies.Count)];
            }
            stockpilePlanet = randomBody;
        }
    }
    public void StockpileShips()
    {
        foreach (GameObject bodyObjectFind in handler.universe.bodies)
        {
            if (stockpilePlanet != bodyObjectFind && bodyObjectFind.GetComponent<Body>().GetOwner() == myPlayer && bodyObjectFind.GetComponent<Body>().PlayerShipAmount(myPlayer) >= 0)
            {
                bodyObjectFind.GetComponent<Body>().SetShipsToMove(myPlayer, stockpilePlanet);
            }
        }
    }
}
