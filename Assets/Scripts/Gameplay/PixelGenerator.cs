using System;
using System.Text;
using UnityEngine;

public class PixelGenerator : MonoBehaviour
{
    private struct GridBounds
    {
        public int leftMaxPos;
        public int rightMaxPos;
        public int upMaxPos;
        public int downMaxPos;

        public GridBounds(int leftMaxPos, int rightMaxPos, int upMaxPos, int downMaxPos)
        {
            this.leftMaxPos = leftMaxPos;
            this.rightMaxPos = rightMaxPos;
            this.upMaxPos = upMaxPos;
            this.downMaxPos = downMaxPos;

            // upperRight = new Vector3Int(rightMaxPos, upMaxPos);
            // upperLeft = new Vector3Int(leftMaxPos, upMaxPos);
            // lowerLeft = new Vector3Int(leftMaxPos, downMaxPos);
            // lowerRight = new Vector3Int(rightMaxPos, downMaxPos);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"left: {leftMaxPos} right: {rightMaxPos}");
            stringBuilder.Append(" | ");
            stringBuilder.Append($"up: {upMaxPos} down: {downMaxPos}");
            stringBuilder.Append(" | ");
            stringBuilder.Append($"size: {GetNumGridCells()}");

            return stringBuilder.ToString();
        }

        public int GetNumGridCells()
        {
            var sizeHorizonal = Mathf.FloorToInt(Mathf.Abs(leftMaxPos - rightMaxPos)) + 1;
            var sizeVertical = Mathf.FloorToInt(MathF.Abs(upMaxPos - downMaxPos)) + 1;

            return sizeHorizonal * sizeVertical;
        }
    }

    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private BoxCollider2D _bounds;
    [SerializeField] private Grid _grid;

    [SerializeField] private int _numPixels = 100;
    [SerializeField] private float _cellSize = .5f;
    [SerializeField] private Round _round;

    public float CellSize => _cellSize;
    public int Number => _numPixels;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Reset();

        GridBounds gridBounds = CalculateGridBounds();

        for (int i = 0; i < _numPixels; i++)
        {
            SpawnPixelInCell(GetUniqueRandomCellPosition(gridBounds));
        }

        _round.RoundStart();
    }

    private GridBounds CalculateGridBounds()
    {
        // num upper: 480 @ 0.5

        var numCellsFloat = _bounds.bounds.size / _cellSize;
        var numCells = new Vector2Int(Mathf.FloorToInt(numCellsFloat.x), Mathf.FloorToInt(numCellsFloat.y));

        var cellMaxPosHorizontalX = Mathf.FloorToInt(numCells.x / 2);
        var cellMaxVertical = Mathf.RoundToInt(numCells.y / 2);

        var rightHorizontalShift = 1;
        var leftHorizontalShift = numCells.x % 2 == 0 ? 0 : 1;
        if (_grid.cellSize.x <= Mathf.Abs(_bounds.offset.x))
        {
            var shift = Mathf.FloorToInt(Mathf.Abs(_bounds.offset.x) / _grid.cellSize.x);
            rightHorizontalShift += shift;
            leftHorizontalShift += shift;
        }

        var leftMaxPos = -cellMaxPosHorizontalX - leftHorizontalShift;
        var rightMaxPos = cellMaxPosHorizontalX - rightHorizontalShift;
        var upMaxPos = cellMaxVertical - 1;
        var downMaxPos = -cellMaxVertical;

        var gridBounds = new GridBounds(leftMaxPos, rightMaxPos, upMaxPos, downMaxPos);

        _numPixels = Mathf.Min(_numPixels, gridBounds.GetNumGridCells());
        //TODO: Feed back to UI
        return gridBounds;
    }

    private void Reset()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        _round.Reset();

        _grid.cellSize = new Vector3(_cellSize, _cellSize, 1);
    }

    private Vector3Int GetUniqueRandomCellPosition(GridBounds gridBounds)
    {
        var randomX = UnityEngine.Random.Range(gridBounds.leftMaxPos, gridBounds.rightMaxPos + 1);
        var randomY = UnityEngine.Random.Range(gridBounds.downMaxPos, gridBounds.upMaxPos + 1);

        var cellPosition = new Vector3Int(randomX, randomY);

        var isOccupied = _round.IsOccupied(cellPosition);

        if (isOccupied)
        {
            return GetUniqueRandomCellPosition(gridBounds);
        }

        _round.Add(cellPosition);
        return cellPosition;
    }

    private void SpawnPixelInCell(Vector3Int cellPosition)
    {
        var position = _grid.GetCellCenterWorld(cellPosition);
        var scale = _grid.cellSize;

        var pixel = Instantiate(_pixelPrefab, position, Quaternion.identity, transform);
        pixel.transform.localScale = scale;
        pixel.name = $"Pixel{cellPosition}";
    }

    public void ChangeNumber(string newNumberText)
    {
        if (Int32.TryParse(newNumberText, out int newNumber))
        {
            _numPixels = newNumber;
        }
    }

    public void ChangeCellSize(string newCellSizeText)
    {
        if (float.TryParse(newCellSizeText, out float newCellSize))
        {
            newCellSize = Mathf.Max(newCellSize, .1f);
            newCellSize = Mathf.Min(newCellSize, 3f);

            _cellSize = newCellSize;
        }
    }
}
