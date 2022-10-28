using UnityEngine;

public class PixelGeneratorDebug : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _bounds;
    [SerializeField] private Grid _grid;
    [SerializeField] private bool _isEnabled;

    private Vector3 _cellSize;
    private Vector2 _numCells;

    public void OnDrawGizmos()
    {
        if (!_isEnabled)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_bounds.bounds.center, _bounds.bounds.size);

        if (_cellSize.x != _grid.cellSize.x && _grid.cellSize.x == _grid.cellSize.y)
        {
            _cellSize = _grid.cellSize;

            _numCells = _bounds.size / _cellSize;
            Debug.Log($"Num cells (float) {_numCells}");
            _numCells = new Vector2(Mathf.FloorToInt(_numCells.x), Mathf.FloorToInt(_numCells.y));
            Debug.Log($"Cells {_numCells}");
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_grid.CellToWorld(Vector3Int.zero), _cellSize);

        var _cellPosX = Mathf.FloorToInt(_numCells.x / 2);
        Debug.Log($"_cellPosX: {_cellPosX} | {_numCells.x}");

        var _cellPosY = Mathf.RoundToInt(_numCells.y / 2);
        Debug.Log($"_cellPosY: {_cellPosY}");

        // Accounting for shifts in grid placement
        var rightHorizontalShift = 1;
        var leftHorizontalShift = 0;
        var upperVerticalShift = 2;
        var lowerVerticalShift = -1;

        var upperRightCell = new Vector3Int(_cellPosX - rightHorizontalShift, _cellPosY - upperVerticalShift);
        Debug.Log($"upperRightCell {upperRightCell}");
        Gizmos.DrawWireCube(_grid.GetCellCenterWorld(upperRightCell), _cellSize);

        var upperLeftCell = new Vector3Int(-_cellPosX - leftHorizontalShift, _cellPosY - upperVerticalShift);
        Debug.Log($"upperLeftCell {upperLeftCell}");
        Gizmos.DrawWireCube(_grid.GetCellCenterWorld(upperLeftCell), _cellSize);

        var lowerLeftCell = new Vector3Int(-_cellPosX - leftHorizontalShift, -_cellPosY - lowerVerticalShift);
        Debug.Log($"lowerLeftCell {lowerLeftCell}");
        Gizmos.DrawWireCube(_grid.GetCellCenterWorld(lowerLeftCell), _cellSize);

        var lowerRightCell = new Vector3Int(_cellPosX - rightHorizontalShift, -_cellPosY - lowerVerticalShift);
        Debug.Log($"lowerRightCell {lowerRightCell}");
        Gizmos.DrawWireCube(_grid.GetCellCenterWorld(lowerRightCell), _cellSize);

    }
}