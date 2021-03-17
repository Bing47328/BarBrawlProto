using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody2D rb;

    public float moveSpeed = 5;

    private Vector2 moveInput;

    private Vector2 mouseInput;

    public float mouseSens = 1f;

    public Camera viewCam;

    public GameObject impactPrefab;

    public int currentAmmo;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x;
        Vector3 moveVertical = transform.right * moveInput.y;

        rb.velocity = (moveHorizontal + moveVertical) * moveSpeed;

        //View Control
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSens;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);

        viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));

        //Shoot
        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Instantiate(impactPrefab, hit.point, transform.rotation);
                }
                else
                {
                    Debug.Log("Looking @ Nothing");
                }
                currentAmmo--;
            }

        }
    }
}
