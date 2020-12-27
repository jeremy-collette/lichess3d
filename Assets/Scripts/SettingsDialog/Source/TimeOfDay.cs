using UnityEngine;
using UnityEngine.UI;

public class TimeOfDay : MonoBehaviour
{
    private Slider slider;

    private GameObject sun;

    void Start()
    {
        this.slider = this.GetComponent<Slider>();
        this.sun = GameObject.Find("Sun");
    }

    public void HandleNewTime()
    {
        var value = this.slider.value;
        var rot = this.sun.transform.rotation;
        rot = Quaternion.Euler(value, 20f, 90f);
        this.sun.transform.rotation = rot;
    }
}
