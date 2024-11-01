using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    // movement physics stuff
    public float speed = 11f, jumpHeight = 4f, gravity = -40f, distanceToGround = 0.4f, KnockbackForce;
    public Transform ground;
    public LayerMask groundMask;
    private CharacterInput controls;
    private CharacterController controller;
    private int jumpAllowance = 1;

    //moveme clamp
    public float xLimitLeft = 0, xLimitRight = 6;
    private Vector2 move;
    private Vector3 velocity;

    // NINAS HEALTH!!! VARS
    public float maxHealth = 100f, lerpTimer, health, chipSpeed = 2f; 
    private bool isDead = false;    
    public Image frontHealthBar, backHealthBar;

    // damage cooldown
    public bool canTakeDamage = true, speedActive = false;
    public float Cooldown = 2f;
    public GameManagerScript gameManager;
    public GameObject gameOverUI, superSuperShroom;
    public GameObject FadeScreen;

   // ----------------------

    private void Awake()
    {
        controls = new CharacterInput();
        controller = GetComponent<CharacterController>();
        health = maxHealth; // initialise mac health
        gameOverUI.SetActive(false);
    }


    void Update()
    {
        if (isDead) return;
        PlayerMovement();
        Grav();
        Jump();
        Clamp();

        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0 && !isDead)
        {
            isDead = true;
            gameManager.gameOver();
        }
    }


    //MOVEMENT ----------------------------------------------------------------

    private void PlayerMovement()
    {
        move = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 forwardMovement = transform.forward;
        var movement = forwardMovement + move.x * transform.right;
        controller.Move(movement * speed * Time.deltaTime);
    }

    private void Clamp()
    {
        float clampedX = Mathf.Clamp(transform.position.x, 0f, 6f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }


    //DAMAGE ----------------------------------------------------------------

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle") && canTakeDamage)
        {
            Debug.Log("hit obstacle");
            TakeDamage(34);
            StartCoroutine(DamageCooldown());
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage; // Reduce health by damage amount
        Debug.Log($"took {damage} damage, health at {health}");
        lerpTimer = 0f;
        UpdateHealthUI();
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(Cooldown);
        canTakeDamage = true;
    }


    //POWERUPS ----------------------------------------------------------------

    public void DoubleSpeed(float multiplier)
    {
        speed *= multiplier;
        Invoke("ResetPowerups", 6f);
    }

    public void DoubleJumpHeight(float multiplier)
    {
        speed += 2f;
        jumpHeight *= multiplier;
        gravity = -20f;
        Invoke("ResetPowerups", 10f); 
    }

    private void ResetPowerups()
    {
        speed = 11f;
        jumpHeight = 4f;
        gravity = -40f;
    }


    //UI ----------------------------------------------------------------

    public void UpdateHealthUI()
    {
        Debug.Log(health);
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
    }


    //MISC. MOVEMENT  ----------------------------------------------------------------

    public bool IsGrounded()
    {
        return Physics.CheckSphere(ground.position, distanceToGround, groundMask);
    }

    private void Grav()
    {
        if (IsGrounded() && velocity.y <= 0)
        {
            velocity.y = -2f;
            jumpAllowance = 1;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (IsGrounded() && controls.Player.Jump.triggered && jumpAllowance > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}