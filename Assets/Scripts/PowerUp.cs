using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PowerDisplay;
    [SerializeField] int PowerCount = 0;

    private void Start()
    {
        PowerDisplay.text = PowerCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            player.IncreasePower(PowerCount);
            Destroy(this.gameObject);
        }
    }
}
