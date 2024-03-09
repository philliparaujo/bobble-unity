using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButtonHandler : MonoBehaviour
{
    public Button button;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (button != null) {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    // Update is called once per frame
    void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        gameManager.ReleaseTeam("Red Players");
        gameManager.ReleaseTeam("Blue Players");
    }
}
