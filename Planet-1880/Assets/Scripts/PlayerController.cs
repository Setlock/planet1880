using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    public GameHandler gameHandler;
    Player myPlayer;
    bool clickedOnce = false, fPressedOnce = false;
    GameObject body1, body2;
    public PlayerController(GameHandler gh, Player p)
    {
        gameHandler = gh;
        this.myPlayer = p;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject body = Camera.main.GetComponent<CameraMove>().bodyToFollow;
            if (body != null && body.GetComponent<Body>().GetOwner() == myPlayer)
            {
                gameHandler.universe.SpawnShip(body);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!fPressedOnce)
            {
                GameObject body = Camera.main.GetComponent<CameraMove>().bodyToFollow;
                GameObject construct = gameHandler.turretObject;
                if (body != null && body.GetComponent<Body>().GetOwner() == myPlayer)
                {
                    body.GetComponent<Body>().StartConstructPlacement(construct);
                    fPressedOnce = true;
                }
            }
            else
            {
                GameObject body = Camera.main.GetComponent<CameraMove>().bodyToFollow;
                if (body != null && body.GetComponent<Body>().GetOwner() == myPlayer)
                {
                    body.GetComponent<Body>().PlaceConstruct();
                    fPressedOnce = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject body = Camera.main.GetComponent<CameraMove>().bodyToFollow;
            if (body != null && body.GetComponent<Body>().GetOwner() == myPlayer)
            {
                body.GetComponent<Body>().DestroyTempConstruct();
                fPressedOnce = false;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!clickedOnce)
            {
                body1 = gameHandler.universe.GetBodyAtLocation(Input.mousePosition);
                if (body1 != null)
                {
                    clickedOnce = true;
                }
            }
            else
            {
                body2 = gameHandler.universe.GetBodyAtLocation(Input.mousePosition);
                if (body2 != null)
                {
                    body1.GetComponent<Body>().SetShipsToMove(myPlayer,body2);
                }
                clickedOnce = false;
                body1 = null;
                body2 = null;
            }
        }
    }
}
