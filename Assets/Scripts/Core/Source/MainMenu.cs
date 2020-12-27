using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, ILichessLoginRetriever
{
    private Text apiKeyText;

    private Text gameIdText;

    private bool playButtonClicked = false;

    private GameObject settingsButton;

    public void PlayButtonClicked()
    {
        this.playButtonClicked = true;
        this.gameObject.SetActive(false);
        this.settingsButton.SetActive(true);

        Time.timeScale = 1;
    }

    public bool TryGetLichessLogin(out LichessLogin login)
    {
        login = new LichessLogin();
        if (!this.playButtonClicked)
        {
            return false;
        }

        login = new LichessLogin
        {
            ApiKey = this.apiKeyText.text,
            GameId = this.gameIdText.text
        };
        return true;
    }

    void Start()
    {
        this.apiKeyText = GameObject.Find("ApiKeyInputText").GetComponent<Text>();
        this.gameIdText = GameObject.Find("GameIdInputText").GetComponent<Text>();
        this.settingsButton = GameObject.Find("SettingsButton");
        this.settingsButton.SetActive(false);

        Time.timeScale = 0;
    }
}
