using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FormationManager : MonoBehaviour
{
    private Button prevButton = null;
    public Button selectedButton = null;

    public Color defaultColor;
    public Color pressedColor;

    public Button button1;
    public Button button2;
    public Button button3;

    public GameObject panel;

    public delegate void FormationChangedHandler(string newFormation);
    public event FormationChangedHandler OnFormationChanged;

    public float[][][] defensePositions = new float[][][]
    {
        new float[][] // Defense level 1
        {
            new float[] { 9.5f, 0f } // One pair of coordinates
        },
        new float[][] // Defense level 2
        {
            new float[] { 9.5f, -2f }, // First pair
            new float[] { 9.5f, 2f }  // Second pair
        },
        new float[][] // Defense level 3
        {
            new float[] { 9.5f, 0f }, // First pair
            new float[] { 8.5f, -2f }, // Second pair
            new float[] { 8.5f, 2f }  // Third pair
        }
    };

    public float[][][] midfieldPositions = new float[][][]
    {
        new float[][] // Midfield level 1
        {
            new float[] { 5.5f, 0f } // One pair of coordinates
        },
        new float[][] // Midfield level 2
        {
            new float[] { 5.5f, -4f }, // First pair
            new float[] { 5.5f, 4f }  // Second pair
        },
        new float[][] // Midfield level 3
        {
            new float[] { 5.5f, 0f }, // First pair
            new float[] { 5.5f, -4f }, // Second pair
            new float[] { 5.5f, 4f }  // Third pair
        }
    };

    public float[][][] forwardPositions = new float[][][]
    {
        new float[][] // Midfield level 1
        {
            new float[] { 3f, 0f } // One pair of coordinates
        },
        new float[][] // Midfield level 2
        {
            new float[] { 2.5f, -2f }, // First pair
            new float[] { 2.5f, 2f }  // Second pair
        },
        new float[][] // Midfield level 3
        {
            new float[] { 3f, 0f }, // First pair
            new float[] { 1.5f, -2f }, // Second pair
            new float[] { 1.5f, 2f }  // Third pair
        }
    };


    // Start is called before the first frame update
    void Start()
    {
        TMP_Text text1 = button1.GetComponentInChildren<TMP_Text>();
        TMP_Text text2 = button2.GetComponentInChildren<TMP_Text>();
        TMP_Text text3 = button3.GetComponentInChildren<TMP_Text>();

        generateThreeRandomNumbers(text1);
        
        generateThreeRandomNumbers(text2);
        while (text2.text == text1.text) {
            generateThreeRandomNumbers(text2);
        }

        generateThreeRandomNumbers(text3);
        while (text3.text == text1.text || text3.text == text2.text) {
            generateThreeRandomNumbers(text3);
        }

        selectNewButton(button1);
    }

    public void OnOption1Selected() {
        selectNewButton(button1);
    }
    public void OnOption2Selected() {
        selectNewButton(button2);
    }
    public void OnOption3Selected() {
        selectNewButton(button3);
    }

    public void onConfirmSelected() {
        // make panel invisible
        if (panel != null) {
            panel.gameObject.SetActive(false);
        }
    }

    public void selectNewButton(Button newButton) {
        prevButton = selectedButton;
        selectedButton = newButton;

        resetColors(prevButton);
        updateColors(selectedButton);

        // trigger event that formation changed
        string newFormation = newButton.GetComponentInChildren<TMP_Text>().text;
        OnFormationChanged?.Invoke(newFormation);
    }

    private void resetColors(Button button) {
        if (button == null) return;

        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null) {
            buttonImage.color = defaultColor;
        }
    }

    private void updateColors(Button button) {

        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null) {
            buttonImage.color = pressedColor;
        }
    }

    private void generateThreeRandomNumbers(TMP_Text buttonText) {
        int first = Random.Range(0,4);
        int second = Random.Range(Mathf.Max(0, 1-first), Mathf.Min(3, 4-first));
        int third = 4-first-second;

        string formation = first + "-" + second + "-" + third;
        buttonText.text = formation;
    }
}
