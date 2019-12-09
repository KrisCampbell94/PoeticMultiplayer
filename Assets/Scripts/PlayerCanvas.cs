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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateNPCCount(int number)
    {
        npcCountText.text = "ALLY COUNT: " + number.ToString("00");
        npcCountShadow.text = npcCountText.text;
    }
    public void UpdateTimer(float number)
    {
        int seconds = Mathf.FloorToInt(number);
        int minutes = 0;
        string timerString = "";
        while ((seconds -= 60) >= 0)
        {
            minutes += 1;
            seconds -= 60;
        } 
        timerString = minutes.ToString("00") + ":" + seconds.ToString("00");
        timerText.text = "TIMER: [" + timerString + "]";
        timerShadow.text = timerText.text;
    }

}
