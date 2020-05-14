using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ship : MonoBehaviour
{
    public Player owner;
    Rigidbody2D rb;
    float moveSpeed = 40;
    Vector2 pivotPoint;
    Vector2 currentVelocity = Vector2.zero;
    bool movingToLocation = false;
    float orbitSpeed = 10;
    public float orbitAngle = 0, orbitDist = 20;
    GameObject bodyToOrbit;
    GameObject bodyToMoveTo;

    private void Start()
    {
        this.transform.GetChild(0).position = new Vector2(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2f);
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (bodyToMoveTo != null)
        {
            MoveToBody(bodyToMoveTo);
            rb.position += currentVelocity * Time.deltaTime;
        }
        else
        {
            Orbit();
        }
    }
    public void Orbit()
    {
        if (!movingToLocation && bodyToOrbit != null)
        {
            transform.position = new Vector2(bodyToOrbit.transform.position.x + Mathf.Cos(orbitAngle * Mathf.Deg2Rad) * orbitDist, bodyToOrbit.transform.position.y + Mathf.Sin(orbitAngle * Mathf.Deg2Rad) * orbitDist);
            orbitAngle += orbitSpeed * Time.deltaTime;
            if (orbitAngle >= 360)
            {
                orbitAngle = 0;
            }

            transform.rotation = Quaternion.AngleAxis(orbitAngle, Vector3.forward);
        }
    }
    public void SetLocationToMove(GameObject body)
    {
        bodyToOrbit.GetComponent<Body>().RemoveShip(gameObject);
        bodyToMoveTo = body;
    }
    public void SetBodyToOrbit(GameObject body)
    {
        this.bodyToOrbit = body;
    }
    public void MoveToBody(GameObject location)
    {
        movingToLocation = true;

        float xVel = location.transform.position.x - transform.position.x;
        float yVel = location.transform.position.y - transform.position.y;
        Vector2 vel = new Vector2(xVel, yVel).normalized;
        currentVelocity = vel * moveSpeed;

        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 dist = location.transform.position - transform.position;
        dist.z = 0;
        float sqrDist = (dist).sqrMagnitude;
        if (sqrDist < (orbitDist*orbitDist))
        {
            location.GetComponent<Body>().AddShip(gameObject);
            SetBodyToOrbit(location);
            bodyToMoveTo = null;
            movingToLocation = false;

            orbitAngle = Mathf.Atan2((transform.position.y - location.transform.position.y), (transform.position.x - location.transform.position.x)) * Mathf.Rad2Deg;
        }
    }
    public void MoveToLocation(Vector2 pos)
    {
        movingToLocation = true;

        float xVel = pos.x - transform.position.x;
        float yVel = pos.y - transform.position.y;
        Vector2 vel = new Vector2(xVel, yVel).normalized;
        currentVelocity = vel*moveSpeed;

        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
