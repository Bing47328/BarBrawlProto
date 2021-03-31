using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPController : MonoBehaviour
{
    public static FPController instance;
    
    //Text
    public Text healthText;

    public GameObject deadScreen;
    public GameObject impactPrefab;

    public Animator fistAnim;

    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _mouseSensativity = 50f;
    [SerializeField] private float _minCameraview = -70f, _maxCameraview = 80f;

    //Movement
    private CharacterController _charController;
    private Camera _camera;
    private float xRotation = 0f;
    private Vector3 _playerVelocity;

    //Health
    private int currentHealth;
    private int maxHealth = 100;
    private bool hasDied;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _charController = GetComponent<CharacterController>();
        _camera = Camera.main;

        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString() + "%";

    }

    void Update()
    {
        if (!hasDied)
        {
            PlayerMovement();

            float mouseX = Input.GetAxis("Mouse X") * _mouseSensativity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensativity * Time.deltaTime;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, _minCameraview, _maxCameraview);

            _camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            transform.Rotate(Vector3.up * mouseX * 3);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 5f))
                {
                    Instantiate(impactPrefab, hit.point, transform.rotation);

                    //Enemy Hit
                    if (hit.transform.tag == "Enemy")
                    {
                        hit.transform.GetComponent<EnemyAI>().TakeDamage(5);
                    }
                }
                else
                {
                    Debug.Log("Looking @ Nothing");
                }
                //currentAmmo--;
                fistAnim.SetTrigger("Shoot");
            }
        }
    }

    private void FixedUpdate()
    {
        if (_charController.isGrounded)
        {
            _playerVelocity.y = 0f;
        }
        else
        {
            _playerVelocity.y += -9.18f * Time.deltaTime;
            _charController.Move(_playerVelocity * Time.deltaTime);
        }
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        _charController.Move(movement * Time.deltaTime * _speed);
    }

    public void TakeDMG(int dmgAmount)
    {
        currentHealth -= dmgAmount;

        if (currentHealth <= 0)
        {
            deadScreen.SetActive(true);
            hasDied = true;
            currentHealth = 0;
            Cursor.lockState = CursorLockMode.None;
        }

        healthText.text = currentHealth.ToString() + "%";
    }

    public void AddHealth(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthText.text = currentHealth.ToString() + "%";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (other.tag == "Enemy")
            {
                EnemyAI.instance.TakeDamage(5);
            }
        }
    }
}
