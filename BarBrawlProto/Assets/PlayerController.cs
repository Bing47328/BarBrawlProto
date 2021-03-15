using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    public float moveSpeed = 5;

    private Vector2 moveInput;

    private Vector2 mouseInput;

    public float mouseSens = 1f;

    public Camera viewCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x;
        Vector3 moveVertical = transform.right * moveInput.y;

        rb.velocity = (moveHorizontal + moveVertical) * moveSpeed;

        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSens;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);

        viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));


        if (Input.GetMouseButton(0))
        {
            Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("I'm looking at " + hit.transform.name);
            }
            else 
            {
                Debug.Log("Looking @ Nothing");
            }
        }

    }
}
