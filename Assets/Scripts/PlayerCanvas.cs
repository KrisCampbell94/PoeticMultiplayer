using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas playerCanvas;

    public GameObject HUD;
    public GameObject Win;
    public GameObject Lose;

    public Text npcCountText, timerText;
    public Text timerShadow, npcCountShadow;

    private void Awake()
    {
        playerCanvas = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateNPCCount(int alliesHave, int alliesRequired)
    {
        npcCountText.text = $"ALLY COUNT: {alliesHave} / {alliesRequired}";
        npcCountShadow.text = npcCountText.text;
    }

    public void UpdateTimer(float timeRemaining) {
		// https://answers.unity.com/questions/45676/making-a-timer-0000-minutes-and-seconds.html
		string minutes = Mathf.Floor(timeRemaining / 60).ToString("00");
		string seconds = Mathf.Floor(timeRemaining % 60).ToString("00");

        timerText.text = $"TIMER: [{minutes}:{seconds}]";
        timerShadow.text = timerText.text;
    }

    //

    public void ToggleHUD(bool toggle)
    {
        HUD.SetActive(toggle);
    }

    public void ToggleWin(bool toggle)
    {
        Win.SetActive(toggle);
    }

    public void ToggleLose(bool toggle)
    {
        Lose.SetActive(toggle);
    }
}
