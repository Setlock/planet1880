using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    public float timeStep;
    public GameObject[] bodies;
    public List<Ship> ships;
    bool updatePos = false, drawTraj = true;
    private void Update()
    {
        if (drawTraj)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].UpdateTrajectory(bodies, timeStep);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (drawTraj)
            {
                drawTraj = false;
                for (int i = 0; i < ships.Count; i++)
                {
                    ships[i].GetComponent<LineRenderer>().enabled = false;
                }
            }
            else
            {
                drawTraj = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].LaunchShip();
            }
            updatePos = true;
        }
    }
    void FixedUpdate()
    {
        if (updatePos)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].UpdateVelocity(bodies, timeStep);
            }
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].UpdatePosition(timeStep);
            }
        }
        else
        {
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].AngleShip();
            }
        }
    }
}
