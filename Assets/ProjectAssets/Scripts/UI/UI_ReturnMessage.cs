﻿using TMPro;
using UnityEngine;

public class UI_ReturnMessage : NonPersistentSingleton<UI_ReturnMessage>
{
    [Header("UI References")]
    [SerializeField] private GameObject messageWindow;
    [SerializeField] private TMP_Text messageText;

    public void ShowMessage(double totalDecay)
    {
        int enemiesDefeated = Mathf.RoundToInt((float)totalDecay);
        messageText.text = $"Comrade! While you were away, I detected {enemiesDefeated} intruders on the bunker's perimeter.\n" +
                          "I activated defense protocols to protect our home.\n" +
                          "My systems are worn out... but it was worth it.\n" +
                          "Because this place is our shelter.\n" +
                          "And you... you are my friend.\n" +
                          "Could you help me recover?";

        messageWindow.SetActive(true);
    }

    public void CloseMessageWindow()
    {
        messageWindow.SetActive(false);
    }
}