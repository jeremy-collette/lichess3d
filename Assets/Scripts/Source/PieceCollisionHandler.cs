using UnityEngine;

public class PieceCollisionHandler : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision triggered!");
        this.transform.Translate(Vector3.right * 0.60f);
    }
}
