using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplaySettings : MonoBehaviour
{
    [SerializeField] GameObject StartScreen, ReadyTitle, StartButton;
    [SerializeField] TextMeshProUGUI CountdownText;
    [SerializeField] MoveOverTime CamMoveScript;
    [SerializeField] float StartDelay = 5.0f;
    float CountdownTime = 0.0f;
    bool bIsCountingDown = false;


    private void Start()
    {
        CountdownTime = StartDelay;
    }

    private void Update()
    {
        if (bIsCountingDown)
        {
            CountdownTime -= Time.deltaTime;
            int displayTime = (int)CountdownTime + 1;
            CountdownText.text = displayTime.ToString();
        }
    }

    public void StartGame()
    {
        DisableStartTitleDisplay();
        CountdownText.gameObject.SetActive(true);
        CamMoveScript.StartMovementWithDelay(StartDelay);
        bIsCountingDown = true;
        StartCoroutine(StopCountdownText());
    }

    void DisableStartTitleDisplay()
    {
        ReadyTitle.SetActive(false);
        StartButton.SetActive(false);
    }

    IEnumerator StopCountdownText()
    {
        yield return new WaitForSeconds(StartDelay);
        bIsCountingDown = false;
        StartScreen.SetActive(false );
    }
}
