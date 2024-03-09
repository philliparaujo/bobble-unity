using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float goalHandlingDuration = 2.0f;
    public float gameEndDuration = 3.0f;

    public int maxScore = 3;

    public int redScore = 0;
    public int blueScore = 0;

    private bool redShowing = true;
    private bool blueShowing = true;

    public AudioSource audioSource;
    public AudioClip goalSound;
    public AudioClip gameEndSound;

    public Toggle redToggle;
    public Toggle blueToggle;

    public FormationManager redFormationManager;
    public FormationManager blueFormationManager;

    void Start() {
        // subscribe to red formation manager
        if (redFormationManager != null) {
            redFormationManager.OnFormationChanged += RedFormationChanged;
        }

        // subscribe to blue formation manager
        if (blueFormationManager != null) {
            blueFormationManager.OnFormationChanged += BlueFormationChanged;
        }

        ResetGame();
    }

    IEnumerator WaitAndReset(float seconds, bool redScored) {
        yield return new WaitForSeconds(seconds);

        if (redScore >= maxScore || blueScore >= maxScore) {
            HandleGameEnd(redScore >= maxScore);
        } else {
            ResetBall(redScored);
            ResetPlayers();
            ShowPanel(redFormationManager);
            ShowPanel(blueFormationManager);
        }
    }

    IEnumerator WaitAndEndGame(float seconds, bool redWon) {
        yield return new WaitForSeconds(seconds);

        // TODO: display that red won in some way?
        ResetGame();
    }

    IEnumerator WaitForFormationSelection(string teamName, FormationManager formationManager, bool invert) {
        yield return new WaitUntil(() => formationManager.selectedButton != null);
       
        Debug.Log("done waiting for " + teamName);

        Button selectedButton = formationManager.selectedButton;
        string formation = selectedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        string[] splitFormation = formation.Split("-"); 

        int numDefense = int.Parse(splitFormation[0]);
        int numMid = int.Parse(splitFormation[1]);
        int numForward = int.Parse(splitFormation[2]);

        float multiplier = 1;
        if (invert) multiplier = -1;

        GameObject players = GameObject.Find(teamName);

        int playerIndex = 0;
        int currentCount = 0;
        while (playerIndex < numDefense) {
            Transform player = players.transform.GetChild(playerIndex);

            float[] coordinates = formationManager.defensePositions[numDefense-1][currentCount];
            placePlayer(player, coordinates[0]*multiplier, coordinates[1]);

            playerIndex++;
            currentCount++;
        }

        currentCount = 0;
        while (playerIndex < numDefense + numMid) {
            Transform player = players.transform.GetChild(playerIndex);

            float[] coordinates = formationManager.midfieldPositions[numMid-1][currentCount];
            placePlayer(player, coordinates[0]*multiplier, coordinates[1]);

            playerIndex++;
            currentCount++;
        }

        currentCount = 0;
        while (playerIndex < numDefense + numMid + numForward) {
            Transform player = players.transform.GetChild(playerIndex);
            
            float[] coordinates = formationManager.forwardPositions[numForward-1][currentCount];
            placePlayer(player, coordinates[0]*multiplier, coordinates[1]);

            playerIndex++;
            currentCount++;
        }
    }

    public void ResetGame() {
        Debug.Log("Game reset");
        ResetBall(false);
        ResetPlayers();
        ResetScore();
        ShowPanel(redFormationManager);
        ShowPanel(blueFormationManager);
    }

    public void HandleGoal(bool redScored) {
        Debug.Log("Handling goal");     
        IncrementScore(redScored);
        if (audioSource != null && goalSound != null) {
            audioSource.PlayOneShot(goalSound);
        }
        StartCoroutine(WaitAndReset(goalHandlingDuration, redScored));
    }

    public void HandleGameEnd(bool redWon) {
        Debug.Log("Handling game end");
        if (audioSource != null && gameEndSound != null) {
            audioSource.PlayOneShot(gameEndSound);
        }
        StartCoroutine(WaitAndEndGame(gameEndDuration, redWon));
    }

    public void ResetPlayers() {
        Debug.Log("Players reset");

        ResetTeam("Red Players", redFormationManager, true);
        ResetTeam("Blue Players", blueFormationManager, false);
    }

    private void placePlayer(Transform player, float x, float z) {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        player.position = new Vector3(x, 0, z);

    }

    public void ResetTeam(string teamName, FormationManager formationManager, bool invert) {
        StartCoroutine(WaitForFormationSelection(teamName, formationManager, invert));     
    }

    private void RedFormationChanged(string newFormation) {
        Debug.Log("New red formation: " + newFormation);
        ResetTeam("Red Players", redFormationManager, true);
    }

    private void BlueFormationChanged(string newFormation) {
        Debug.Log("New red formation: " + newFormation);
        ResetTeam("Blue Players", blueFormationManager, false);
    }

    public void ReleaseTeam(string teamName) {
        Debug.Log(teamName + " launching");

        GameObject players = GameObject.Find(teamName);
        for (int i=0; i<players.transform.childCount; i++) {
            Transform player = players.transform.GetChild(i);
            SlingshotController slingshot = player.GetComponent<SlingshotController>();
            if (slingshot != null) {
                slingshot.releaseArrow();
            }
        }

        if (redToggle != null) {
            redToggle.isOn = true;
        }
        if (blueToggle != null) {
            blueToggle.isOn = true;
        }
    }

    public void ToggleRedArrows() {
        ToggleTeamArrows("Red Players", !redShowing);
        redShowing = !redShowing;
    }

    public void ToggleBlueArrows() {
        ToggleTeamArrows("Blue Players", !blueShowing);
        blueShowing = !blueShowing;
    }

    public void ToggleTeamArrows(string teamName, bool showing) {
        Debug.Log(teamName + " toggling to " + showing);
        GameObject players = GameObject.Find(teamName);
        for (int i=0; i<players.transform.childCount; i++) {
            Transform player = players.transform.GetChild(i);
            SlingshotController slingshot = player.GetComponent<SlingshotController>();
            if (slingshot != null && slingshot.isReadyToLaunch()) {
                LineRenderer lineRenderer = player.GetComponent<LineRenderer>();
                if (lineRenderer != null) {
                    lineRenderer.enabled = showing;
                }

            }
        }
    }

    public void ResetBall(bool redScored) {
        Debug.Log("Ball reset");

        GameObject ball = GameObject.FindWithTag("ball");
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();

        float ballX = redScored ? 1.5f : -1.5f;

        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ball.transform.position = new Vector3(ballX,0,0);
    }

    public void ResetScore() {
        Debug.Log("Score reset");
        redScore = 0;
        blueScore = 0;
    }

    public void IncrementScore(bool redScored) {
        Debug.Log("Incrementing score");
        if (redScored) {
            redScore += 1;
        } else {
            blueScore += 1;
        }
    }

    private void HidePanel(FormationManager formationManager) {
        Debug.Log("hiding panel");

        GameObject panel = formationManager.panel;
        panel.gameObject.SetActive(false);
    }

    private void ShowPanel(FormationManager formationManager) {
        Debug.Log("showing panel");

        GameObject panel = formationManager.panel;
        panel.gameObject.SetActive(true);

        formationManager.selectNewButton(formationManager.button1);
    }
}
