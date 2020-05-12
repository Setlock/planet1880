using System;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Rigidbody2D rb;
    public float launchSpeed = 3;

    Vector2 initialPosition;
    Vector2 initialVelocity;
    Vector2 currentVelocity = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = new Vector2(transform.position.x,transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y/2f);
        this.gameObject.transform.GetChild(0).position = new Vector3(transform.position.x,transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y/2f);
    }
    public void UpdateTrajectory(GameObject[] bodies, float timeStep)
    {
        int count = 2500;
        LineRenderer line = GetComponent<LineRenderer>();
        line.enabled = true;
        line.positionCount = count;

        Vector2 pos = initialPosition;
        Vector2 vel = initialVelocity;
        for (int i = 0; i < count; i++)
        {
            line.SetPosition(i, pos);
            foreach (GameObject b in bodies)
            {
                float sqrDst = (b.GetComponent<Rigidbody2D>().position - pos).sqrMagnitude;
                Vector2 forceDir = (b.GetComponent<Rigidbody2D>().position - pos).normalized;
                Vector2 force = forceDir * b.GetComponent<CelestialBody>().mass / sqrDst;
                Vector2 acceleration = force;

                vel += acceleration * timeStep;
            }
            pos += vel * timeStep;
        }
    }
    public void UpdateVelocity(GameObject[] bodies, float timeStep)
    {
        foreach(GameObject b in bodies)
        {
            float sqrDst = (b.GetComponent<Rigidbody2D>().position - rb.position).sqrMagnitude;
            Vector2 forceDir = (b.GetComponent<Rigidbody2D>().position - rb.position).normalized;
            Vector2 force = forceDir * b.GetComponent<CelestialBody>().mass / sqrDst;
            Vector2 acceleration = force;

            currentVelocity += acceleration * timeStep;
        }
    }
    public void UpdatePosition(float timeStep)
    {
        rb.position += currentVelocity * timeStep;

        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public void AngleShip()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 pivot = this.gameObject.transform.GetChild(0).position;
        transform.RotateAround(pivot, Vector3.back, moveDirection.x);

        initialVelocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (transform.eulerAngles.z + 90)) * launchSpeed, Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z + 90)) * launchSpeed);
    }
    public void LaunchShip()
    {
        currentVelocity = initialVelocity;
    }
}
