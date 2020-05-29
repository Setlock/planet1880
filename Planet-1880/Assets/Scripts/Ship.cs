using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ship : MonoBehaviour
{
    public Player owner;
    Rigidbody2D rb;
    float moveSpeed = 45;
    Vector2 currentVelocity = Vector2.zero;
    bool movingToLocation = false;
    float orbitSpeed = 10;
    public float orbitAngle = 0, orbitDist = 20;
    GameObject bodyToOrbit;
    GameObject bodyToMoveTo;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }
    public void FixedUpdate()
    {
        if (bodyToMoveTo != null)
        {
            MoveToBody(bodyToMoveTo);
            rb.MovePosition(rb.position + currentVelocity*Time.deltaTime);
        }
        else
        {
            Orbit();
        }
    }
    Vector2 orbitPos = Vector2.zero;
    public void Orbit()
    {
        if (!movingToLocation && bodyToOrbit != null)
        {
            orbitPos.x = bodyToOrbit.GetComponent<Body>().rb.position.x + Mathf.Cos(orbitAngle * Mathf.Deg2Rad) * orbitDist;
            orbitPos.y = bodyToOrbit.GetComponent<Body>().rb.position.y + Mathf.Sin(orbitAngle * Mathf.Deg2Rad) * orbitDist;
            rb.MovePosition(orbitPos);

            orbitAngle += orbitSpeed * Time.deltaTime;
            if (orbitAngle >= 360)
            {
                orbitAngle = 0;
            }
            rb.MoveRotation(orbitAngle);
        }
    }
    public void SetLocationToMove(GameObject body)
    {
        bodyToOrbit.GetComponent<Body>().RemoveShip(owner, gameObject);
        bodyToMoveTo = body;
        locationOffset = rb.position-bodyToOrbit.GetComponent<Rigidbody2D>().position;
    }
    public void SetBodyToOrbit(GameObject body)
    {
        this.bodyToOrbit = body;
    }
    Vector2 locationOffset;
    public void MoveToBody(GameObject location)
    {
        movingToLocation = true;

        float xPos = (location.transform.position.x + locationOffset.x);
        float yPos = (location.transform.position.y + locationOffset.y);

        float xVel = xPos - rb.position.x;
        float yVel = yPos - rb.position.y;
        Vector2 vel = new Vector2(xVel, yVel).normalized;
        currentVelocity = vel * moveSpeed;

        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90;
        rb.MoveRotation(angle);

        Vector2 dist = new Vector2(xPos, yPos) - rb.position;
        float sqrDist = (dist).sqrMagnitude;
        if (sqrDist <= 0.1f)
        {
            location.GetComponent<Body>().AddShip(owner, gameObject);
            SetBodyToOrbit(location);
            bodyToMoveTo = null;
            movingToLocation = false;

            orbitAngle = Mathf.Atan2((rb.position.y - location.transform.position.y), (rb.position.x - location.transform.position.x)) * Mathf.Rad2Deg;
        }
    }
    public void MoveToLocation(Vector2 pos)
    {
        movingToLocation = true;

        float xVel = pos.x - rb.position.x;
        float yVel = pos.y - rb.position.y;
        Vector2 vel = new Vector2(xVel, yVel).normalized;
        currentVelocity = vel*moveSpeed;

        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90;
        rb.MoveRotation(angle);
    }
    public void Remove()
    {
        bodyToOrbit.GetComponent<Body>().ships[owner].Remove(gameObject);
        Destroy(gameObject);
    }
}
