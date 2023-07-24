using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_PowerDisplay;
    [SerializeField] int PowerCount = 0;


    void Start()
    {
        m_PowerDisplay.text = PowerCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            int playerPower = player.PowerCount;

            if (playerPower >= PowerCount) {
                player.IncreasePower(PowerCount);
                OnDestroy();
            }
            else
                player.Defeat();
        }
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
