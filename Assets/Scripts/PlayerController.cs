using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public Timer timer;   // referensi ke Timer

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        count = 0;
        SetCountText();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
    }

    void Update()
    {
        // test damage with space (optional)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        healthBar.SetHealth(currentHealth);

        if (currentHealth == 0)
            LoseGame();
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        healthBar.SetHealth(currentHealth);
    }

    public void LoseGame()
    {
        loseTextObject.SetActive(true);

        // stop movement
        speed = 0;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // stop timer
        if (timer != null)
            timer.isRunning = false;
    }
    
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;

            AddHealth(10);  // pickup menambah health

            SetCountText();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(5);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 15)
        {
            winTextObject.SetActive(true);

            // stop timer when win
            if (timer != null)
                timer.isRunning = false;
        }
    }
}
