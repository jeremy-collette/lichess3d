using UnityEngine;

public class BoardThemeSetter : MonoBehaviour, IBoardThemeSetter
{
    public string Name { get => this.NameUi; }

    public string NameUi;

    public Material BlackTileMaterialUi;

    public Material WhiteTileMaterialUi;

    public Material OutlineMaterialUi;

    public Material BorderMaterialUi;

    public void SetBoardTheme()
    {
        // Set board
        var chessboardGameObject = GameObject.Find("Chess Board");
        foreach (var mat in chessboardGameObject.GetComponent<Renderer>().materials)
        {
            Destroy(mat);
        }

        var materials = new Material[]
        {
            this.BlackTileMaterialUi,
            this.WhiteTileMaterialUi,
            this.BorderMaterialUi,
            this.OutlineMaterialUi
        };
        chessboardGameObject.GetComponent<Renderer>().materials = materials;
    }
}