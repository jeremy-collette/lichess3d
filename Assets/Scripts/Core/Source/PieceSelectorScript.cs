using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;

public class PieceSelectorScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private const float HoldHeight = 0.5f;

    private float initialY;

    private bool holding = false;

    private bool dropped = false;

    private ILichessClient lichessClient;

    private GameObject aboveTile = null;

    void Start()
    {
        this.initialY = this.gameObject.transform.position.y;
        this.lichessClient = GameObject.Find("Game").GetComponent<ILichessClient>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != InputButton.Left)
        {
            return;
        }

        this.holding = true;
        var piece = this.gameObject.GetComponent<IChessPiece>();
        //Debug.Log($"Picked up {piece.Player} {piece.PieceType}!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != InputButton.Left)
        {
            return;
        }

        this.holding = false;
        this.dropped = true;
        var piece = this.gameObject.GetComponent<IChessPiece>();
        //Debug.Log($"Put down {piece.Player} {piece.PieceType}!");
    }

    void Update()
    {
        if (this.holding)
        {
            this.HandleHolding();
        }
        else if (this.dropped)
        {
            this.HandleDropped();
        }
    }

    private void HandleHolding()
    {
        // Ray cast to the board to find which tile we are above
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, maxDistance: 100f, layerMask: LayerMask.GetMask("BoardTiles")))
        {
            // If we can't find a tile, take the "CurrentTile" of the piece (the tile the piece is moving from).
            if (this.aboveTile == null)
            {
                this.aboveTile = this.GetChessPiece().CurrentTile.GameObject;
            }
        }
        else
        {
            // Hold the piece in the air above the tile
            var holdPoint = hit.point;
            holdPoint.y = HoldHeight;
            this.gameObject.transform.position = holdPoint;

            // Debug stuff
            Debug.DrawRay(holdPoint, Vector3.down, Color.red);
            //Debug.Log($"Above {hit.collider.gameObject.name} from layer {hit.collider.gameObject.layer}");

            // If the tile that we are above changed, stop highlighting the old one
            if (this.aboveTile != null && hit.collider.gameObject != this.aboveTile)
            {
                this.aboveTile.GetComponent<MeshRenderer>().enabled = false;
            }
            this.aboveTile = hit.collider.gameObject;
        }

        // Highlight the tile we are on top of
        this.aboveTile.GetComponent<MeshRenderer>().enabled = true;
    }

    private void HandleDropped()
    {
        // If we were above a tile, make sure we stop highlighting it
        if (this.aboveTile != null)
        {
            this.aboveTile.GetComponent<MeshRenderer>().enabled = false;
        }

        // Treat this as a move, even if we are going to the same tile
        this.HandleUserPieceMove();

        // Reset dropped flag
        this.dropped = false;
    }

    private void HandleUserPieceMove()
    {
        var piece = this.gameObject.GetComponent<IChessPiece>();
        var currentBoardTile = piece.CurrentTile;
        var newBoardTile = this.aboveTile.GetComponent<IBoardTile>();

        var moveString = $"{currentBoardTile.Position.Alphabetical}{currentBoardTile.Position.Numeral}"
            + $"{newBoardTile.Position.Alphabetical}{newBoardTile.Position.Numeral}";
        Debug.Log($"Player moved {piece.Player} {piece.PieceType} {moveString}");

        // Update the chessboard (this will correct the positioning etc)
        this.GetChessboard().Move(currentBoardTile.Position, newBoardTile.Position);

        // Send to Lichess
        this.lichessClient.SendMove(moveString);
    }

    private IChessPiece GetChessPiece() => this.gameObject.GetComponent<IChessPiece>();

    private IChessboard GetChessboard() =>  GameObject.Find("Chessboard").GetComponent<IChessboard>();
}
