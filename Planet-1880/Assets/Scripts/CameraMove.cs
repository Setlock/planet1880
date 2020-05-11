using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 20;
    void Update()
    {
        //Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
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
    }
}
