using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private GameManager gameManager;
    public TextMeshProUGUI redScore;
    public TextMeshProUGUI blueScore;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        redScore.text = "0";
        blueScore.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null) {
            redScore.text = gameManager.redScore.ToString();
            blueScore.text = gameManager.blueScore.ToString();
        }
    }
}
