using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject player;
    public GameObject rangeTable;
    public GameObject scoreText;
    public TMPro.TextMeshProUGUI enemyInfoText;
    public GameObject enemyPrefab;
    public Vector2 enemySpawnRangeLimits = new Vector2(150, 500);
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = GameObject.Find("Player");
        var enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy == null)
        {
            SpawnEnemy();
        }
        else
        {
            ShowEnemyInfoText(enemy.transform.position);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rangeTable.SetActive(!rangeTable.activeSelf);
        }
    }

    private void OnEnable()
    {
        MortarShell.OnHitEnemy += AddScore;
    }

    private void OnDisable()
    {
        MortarShell.OnHitEnemy -= AddScore;
    }

    private void AddScore()
    {
        score += 1;
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        // Choose a random point on the circle around the player but within the range limits
        float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        float distance = UnityEngine.Random.Range(enemySpawnRangeLimits.x, enemySpawnRangeLimits.y);
        Vector3 spawnPosition = player.transform.position + new Vector3(Mathf.Cos(angle), 20f, Mathf.Sin(angle)) * distance;
        // Raycast downwards to find the ground. We are in a 3d environment
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit))
        {
            spawnPosition = hit.point + new Vector3(0, 2f, 0);
        } 
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        ShowEnemyInfoText(spawnPosition);
    }

    private void ShowEnemyInfoText(Vector3 spawnPosition)
    {
        // Get the direction vector from the player to the spawn position
        Vector3 directionToTarget = spawnPosition - player.transform.position;

        // Calculate the signed angle in degrees from the right direction (East)
        float azimuthDegrees = Vector3.SignedAngle(Vector2.right, directionToTarget, Vector3.up);

        // Convert degrees to mils (1 degree ~= 17.7777778 mils)
        float azimuthMils = azimuthDegrees * 6400 / 360;

        // Ensure azimuth is positive
        if (azimuthMils < 0)
        {
            azimuthMils += 6400;
        }
        
        // Calculate the distance to the target
        float distanceM = Vector3.Distance(player.transform.position, spawnPosition);
        
        // Log the target information
        Debug.Log($"Target Azimuth: {azimuthMils.ToString()}, Distance: {distanceM.ToString()}");

        // Display the enemy information text
        enemyInfoText.text =
            $"Target spotted on azimuth: {Mathf.Round(azimuthMils).ToString()}, distance: {Mathf.Round(distanceM).ToString()}";
        enemyInfoText.outlineWidth = 0.2f;
    }

}