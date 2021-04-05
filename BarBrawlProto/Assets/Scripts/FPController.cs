using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPController : MonoBehaviour
{
    [SerializeField] FlashImage _dmgImage = null, _healImage = null, _stunImage = null;


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

    public Collider punchCollider;

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
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, .3f))
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
        _dmgImage.Flash();
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

    public void GetStun(int seconds)
    {
        StartCoroutine(Stunned(seconds));
    }

    public void AddHealth(int healAmount)
    {
        _healImage.Flash();
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthText.text = currentHealth.ToString() + "%";
    }


    IEnumerator Stunned(float seconds)
    {
        _stunImage.Flash();
        float _prevSpeed = _speed;
        _speed = 0f;

        yield return new WaitForSeconds(seconds);

        _speed = _prevSpeed;


    }
}
