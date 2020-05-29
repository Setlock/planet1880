using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurret : Construct
{
    GameObject barrel;
    public void Start()
    {
        barrel = transform.GetChild(0).gameObject;
    }
    public override void Action()
    {
        
    }
    float fireCountdown = 1;
    public void FixedUpdate()
    {
        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0)
        {
            GameObject shipObject = RaycastCone();
            Shoot(shipObject);
            barrel.transform.rotation = Quaternion.AngleAxis(barrelAngle + transform.rotation.eulerAngles.z, Vector3.forward);
            fireCountdown = 1;
        }
    }
    float barrelAngle = 0;
    public void Shoot(GameObject target)
    {
        if(target != null)
        {
            //Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(fireAngle), Mathf.Sin(fireAngle)) * 40, Color.green, 1);
            target.GetComponent<Ship>().Remove();
        }
    }
    int numRays = 120;
    public GameObject RaycastCone()
    {
        GameObject hitObject = null;
        int layer = LayerMask.GetMask("Ships");
        float tempAngle, tempBarrelAngle;
        for (int i = numRays/2; i >= -numRays/2; i--)
        {
            tempBarrelAngle = i;
            tempAngle = (tempBarrelAngle + transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad;
            //Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(tempAngle), Mathf.Sin(tempAngle)) * 40, Color.blue, 1);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(tempAngle), Mathf.Sin(tempAngle)), 50, layer);
            if(hit.collider != null && hit.collider.gameObject.GetComponent<Ship>().owner != transform.parent.gameObject.GetComponent<Body>().GetOwner())
            {
                hitObject = hit.collider.gameObject;
                barrelAngle = tempBarrelAngle;
                break;
            }
        }

        return hitObject;
    }
}
