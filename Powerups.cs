using System.Collections;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public AudioSource powerUp, powerDown;
    public GameObject superSpeedShroom, jumpCoil, Player, activePU = null;
    public PlayerController playerController;
    public float zOffset = 60f;

    void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
        Invoke("SpawnPowerup", 5f);
    }

    void Update()
    {
        if (activePU == null) return;

        if (activePU.transform.position.z < Player.transform.position.z)
        {
            activePU.SetActive(false);
            SpawnPowerup();
            Debug.Log($"MISSED, respawning");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "superSpeed":
                    Debug.Log("picked up shrronm");
                    powerUp.Play();
                    playerController.DoubleSpeed(1.5f);
                    Invoke("DropShroom", 6f);
                    break;

                case "highJump":
                    Debug.Log("picked up coil.");
                    powerUp.Play();
                    playerController.DoubleJumpHeight(1.8f);
                    Invoke("DropCoil", 10f);
                    break;
            }
        }
    }

    private void SpawnPowerup()
    {
        bool chosen = Random.Range(0, 2) == 0;

        if (chosen)
        {
            superSpeedShroom.SetActive(true);
            activePU = superSpeedShroom;
            jumpCoil.SetActive(false);
            SpawnPos(superSpeedShroom);
            Debug.Log("shroom active");
        }
        else
        {
            jumpCoil.SetActive(true);
            activePU = jumpCoil;
            superSpeedShroom.SetActive(false);
            SpawnPos(jumpCoil);
            Debug.Log("COIL ACTIVE");
        }
    }

    private void SpawnPos(GameObject powerup)
    {
        if (playerController.IsGrounded())
        {
        Vector3 spawnPosition = Player.transform.position + new Vector3(Random.Range(0, 5), 0.5f, zOffset);
        powerup.transform.position = spawnPosition;
        Debug.Log($"spawned onv at {spawnPosition}");
        }
    }

    private void DropShroom()
    {
        powerDown.Play();
        superSpeedShroom.SetActive(false);
        Debug.Log("shroom dropped");
        Invoke("SpawnPowerup", 20f);
    }

    private void DropCoil()
    {
        powerDown.Play();
        jumpCoil.SetActive(false);
        Debug.Log("Coil deactivated");
        Invoke("SpawnPowerup", 20f);
    }
}