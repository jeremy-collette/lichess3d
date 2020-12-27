using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class PieceThemeDropdown : MonoBehaviour
{
    private Dropdown dropdown;

    private IPieceThemeSetter[] themeSetters;

    private GameObject settingsMenu;

    public void Start()
    {
        this.dropdown = this.GetComponent<Dropdown>();
        this.settingsMenu = GameObject.Find("SettingsMenu");
        this.themeSetters = this.settingsMenu.GetComponents<IPieceThemeSetter>();
        this.dropdown.options = this.themeSetters.Select(ts => new OptionData(ts.Name)).ToList();
    }

    public void HandleThemeSelection()
    {
        var themeIndex = this.dropdown.value;
        Assert.IsTrue(themeIndex >= 0 && themeIndex < this.themeSetters.Length);
        this.themeSetters[themeIndex].SetPieceTheme();
    }
}
