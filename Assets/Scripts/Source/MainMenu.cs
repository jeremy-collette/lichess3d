using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, ILichessLoginRetriever
{
    private Text apiKeyText;

    private Text gameIdText;

    private bool playButtonClicked = false;

    public void PlayButtonClicked()
    {
        this.playButtonClicked = true;
        this.gameObject.SetActive(false);

        // TODO: move or abstract
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

        // TODO: move or abstract
        Time.timeScale = 0;
    }
}
