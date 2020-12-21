using UnityEngine;
using UnityEngine.EventSystems;

public class PieceSelectorScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private const float HoldHeight = 0.1f;

    private bool holding = false;

    void Start()
    {
    }

    void Update()
    {
        if (this.holding)
        {
            var pos = Input.mousePosition;
            pos.z = 1.0f;
            var holdPoint = Camera.main.ScreenToWorldPoint(pos);
            holdPoint.y += HoldHeight;
            this.gameObject.transform.position = holdPoint;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.holding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.holding = false;
        /* this.transform.Translate(Vector3.up * -1);*/
        var newPos = this.transform.position;
        newPos.y = 0;
        this.transform.position = newPos;
    }
}
