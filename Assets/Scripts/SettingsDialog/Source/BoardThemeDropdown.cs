using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class BoardThemeDropdown : MonoBehaviour
{
    public int InitialIndex;

    private Dropdown dropdown;

    private IBoardThemeSetter[] themeSetters;

    private GameObject settingsMenu;

    public void Start()
    {
        this.dropdown = this.GetComponent<Dropdown>();
        this.settingsMenu = GameObject.Find("SettingsMenu");
        this.themeSetters = this.settingsMenu.GetComponents<IBoardThemeSetter>();
        this.dropdown.options = this.themeSetters.Select(ts => new OptionData(ts.Name)).ToList();
        this.dropdown.value = this.InitialIndex;
    }

    public void HandleThemeSelection()
    {
        var themeIndex = this.dropdown.value;
        Assert.IsTrue(themeIndex >= 0 && themeIndex < this.themeSetters.Length);
        this.themeSetters[themeIndex].SetBoardTheme();
    }
}
