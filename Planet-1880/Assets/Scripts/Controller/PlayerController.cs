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
                for (int i = 0; i < 100; i++)
                {
                    gameHandler.universe.SpawnShip(body);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && myPlayer.money >= gameHandler.turretObject.GetComponent<Construct>().price)
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
                    myPlayer.money -= gameHandler.turretObject.GetComponent<Construct>().price;
                    fPressedOnce = false;
                }
            }
        }
        if (fPressedOnce)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.GetComponent<CameraMove>().bodyToFollow.GetComponent<Body>().SetConstructLocation(mousePos.x, mousePos.y);
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
