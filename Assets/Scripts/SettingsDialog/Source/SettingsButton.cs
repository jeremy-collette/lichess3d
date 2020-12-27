using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    private GameObject settingsPanel;

    public void Start()
    {
        this.settingsPanel = GameObject.Find("SettingsPanel");
        this.settingsPanel.SetActive(false);
    }

    public void HandleClick()
    {
        this.settingsPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
