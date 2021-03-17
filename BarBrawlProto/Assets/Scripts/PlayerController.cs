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

    public Animator fistAnim;

    private int currentHealth;
    public int maxHealth = 100;
    public GameObject deadScreen;
    private bool hasDied;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDied)
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

                        //Enemy Hit
                        if (hit.transform.tag == "Enemy")
                        {
                            hit.transform.GetComponent<EnemyController>().TakeDMG();
                        }
                    }
                    else
                    {
                        Debug.Log("Looking @ Nothing");
                    }
                    currentAmmo--;
                    fistAnim.SetTrigger("Shoot");
                }

            }
        }
    }

    public void TakeDMG(int dmgAmount)
    {
        currentHealth -= dmgAmount;

        if (currentHealth <= 0)
        {
            deadScreen.SetActive(true);
            hasDied = true;
        }
    }

    public void AddHealth(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
