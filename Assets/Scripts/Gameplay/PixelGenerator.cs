using System;
using System.Collections.Generic;
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

            stringBuilder.AppendLine($"left: {leftMaxPos} right: {rightMaxPos}");
            stringBuilder.AppendLine($"up: {upMaxPos} down: {downMaxPos}");

            return stringBuilder.ToString();
        }
    }

    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private BoxCollider2D _bounds;
    [SerializeField] private Grid _grid;

    [SerializeField] private int _numPixels = 100;
    [SerializeField] private float _cellSize = .5f;

    private List<Vector3Int> occupiedCells = new List<Vector3Int>();
    private Vector2Int _numCells;

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
    }

    private GridBounds CalculateGridBounds()
    {


        var cellMaxPosHorizontalX = Mathf.FloorToInt(_numCells.x / 2);
        var cellMaxVertical = Mathf.RoundToInt(_numCells.y / 2);

        var rightHorizontalShift = 1;
        var leftHorizontalShift = _numCells.x % 2 == 0 ? 0 : 1;
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
        return gridBounds;
    }

    private void Reset()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        occupiedCells.Clear();

        _grid.cellSize = new Vector3(_cellSize, _cellSize, 1);

        var numCells = _bounds.bounds.size / _cellSize;
        _numCells = new Vector2Int(Mathf.FloorToInt(numCells.x), Mathf.FloorToInt(numCells.y));

        _numPixels = Mathf.Min(_numPixels, _numCells.x * _numCells.y);
    }

    private Vector3Int GetUniqueRandomCellPosition(GridBounds gridBounds)
    {
        var randomX = UnityEngine.Random.Range(gridBounds.leftMaxPos, gridBounds.rightMaxPos + 1);
        var randomY = UnityEngine.Random.Range(gridBounds.downMaxPos, gridBounds.upMaxPos + 1);

        var cellPosition = new Vector3Int(randomX, randomY);

        var isOccupied = occupiedCells.Contains(cellPosition);

        if (isOccupied)
        {
            return GetUniqueRandomCellPosition(gridBounds);
        }

        occupiedCells.Add(cellPosition);
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
