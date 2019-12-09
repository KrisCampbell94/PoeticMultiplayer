using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public Text npcCountText, timerText;
    public static PlayerCanvas playerCanvas;

    private Text timerShadow, npcCountShadow;

    private void Awake()
    {
        playerCanvas = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        timerShadow = GameObject.Find("TimerCount_Shadow").GetComponent<Text>();
        npcCountShadow = GameObject.Find("AllyCount_Shadow").GetComponent<Text>();
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
}
