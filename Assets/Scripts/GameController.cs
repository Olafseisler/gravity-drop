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
    public int score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
    }
    
}
