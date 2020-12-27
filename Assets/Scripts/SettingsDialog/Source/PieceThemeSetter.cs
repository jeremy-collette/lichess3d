using UnityEngine;

public class PieceThemeSetter : MonoBehaviour, IPieceThemeSetter
{
    public string Name { get => this.NameUi; }

    public string NameUi;

    public Material BlackPieceMaterialUi;

    public Material WhitePieceMaterialUi;

    public Material PieceBaseMaterialUi;

    public void SetPieceTheme()
    {
        // Set pieces
        foreach (var chessPieceGameObject in GameObject.FindGameObjectsWithTag("ChessPiece"))
        {
            foreach (var mat in chessPieceGameObject.GetComponent<Renderer>().materials)
            {
                Destroy(mat);
            }

            var chessPiece = chessPieceGameObject.GetComponent<IChessPiece>();
            var materials = new Material[]
            {
                this.PieceBaseMaterialUi,
                chessPiece.Player == Player.White
                    ? this.WhitePieceMaterialUi
                    : this.BlackPieceMaterialUi
            };
            chessPieceGameObject.GetComponent<Renderer>().materials = materials;
        }
    }
}