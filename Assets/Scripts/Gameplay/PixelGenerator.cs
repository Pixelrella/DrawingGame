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

    private void Spawn()
    {
        Reset();

        GridBounds gridBounds = CalculateGridBounds(_cellSize);

        for (int i = 0; i < _numPixels; i++)
        {
            SpawnPixelInCell(GetUniqueRandomCellPosition(gridBounds));
        }
    }

    private GridBounds CalculateGridBounds(float cellSize)
    {
        var numCellsFloat = _bounds.bounds.size / cellSize;
        var numCells = new Vector2Int(Mathf.FloorToInt(numCellsFloat.x), Mathf.FloorToInt(numCellsFloat.y));

        var cellMaxPosHorizontalX = Mathf.FloorToInt(numCells.x / 2);
        var cellMaxVertical = Mathf.RoundToInt(numCells.y / 2);

        // Accounting for shifts in grid placement
        var rightHorizontalShift = 1;
        var leftHorizontalShift = 0;
        var upperVerticalShift = 2;
        var lowerVerticalShift = -1;

        var leftMaxPos = -cellMaxPosHorizontalX - leftHorizontalShift;
        var rightMaxPos = cellMaxPosHorizontalX - rightHorizontalShift;
        var upMaxPos = cellMaxVertical - upperVerticalShift;
        var downMaxPos = -cellMaxVertical - lowerVerticalShift;

        var gridBounds = new GridBounds(leftMaxPos, rightMaxPos, upMaxPos, downMaxPos);
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

    public int GetNumberOfPixels(float cellSize)
    {
        GridBounds gridBounds = CalculateGridBounds(cellSize);
        return gridBounds.GetNumGridCells();
    }

    public void Generate((float, int) parameters)
    {
        _cellSize = parameters.Item1;
        _numPixels = parameters.Item2;

        Spawn();
    }
}
