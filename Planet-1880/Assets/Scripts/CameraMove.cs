using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public UniverseHandler universe;
    public float moveSpeed = 20;
    public GameObject bodyToFollow;

    void Update()
    {
        if (bodyToFollow == null)
        {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = new Vector3(bodyToFollow.transform.position.x, bodyToFollow.transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            GetComponent<Camera>().orthographicSize++;
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (GetComponent<Camera>().orthographicSize - 1 >= 1)
            {
                GetComponent<Camera>().orthographicSize--;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            bodyToFollow = universe.GetBodyAtLocation(Input.mousePosition);
        }

    }
}
