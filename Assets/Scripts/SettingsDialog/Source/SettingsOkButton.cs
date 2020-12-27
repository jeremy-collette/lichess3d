using UnityEngine;

public class SettingsOkButton : MonoBehaviour
{
    private GameObject settingsButton;

    private GameObject settingsPanel;

    void Awake()
    {
        this.settingsButton = GameObject.Find("SettingsButton");
        this.settingsPanel = GameObject.Find("SettingsPanel");
    }

    public void HandleClick()
    {
        this.settingsButton.SetActive(true);
        this.settingsPanel.SetActive(false);
    }
}
