using System.Collections.Generic;
using UnityEngine;

public class PixelGenerator : MonoBehaviour
{
    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private Collider2D _bounds;
    [SerializeField] private Grid _grid;

    [SerializeField] private int numPixels = 100;
    [SerializeField] private float cellSize = .5f;

    private List<Vector3Int> occupiedCells = new List<Vector3Int>();

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        occupiedCells.Clear();


        _grid.cellSize = new Vector3(cellSize, cellSize, 1);
        for (int i = 0; i < numPixels; i++)
        {
            SpawnPixelInCell(GetUniqueRandomCellPosition());
        }
    }

    private Vector3Int GetUniqueRandomCellPosition()
    {
        var randomX = Random.Range(_bounds.bounds.min.x + cellSize, _bounds.bounds.max.x + cellSize);
        var randomY = Random.Range(_bounds.bounds.min.y + cellSize, _bounds.bounds.max.y + cellSize);

        var cellPosition = _grid.WorldToCell(new Vector3(randomX, randomY));

        var isOccupied = occupiedCells.Contains(cellPosition);

        if (isOccupied)
        {
            return GetUniqueRandomCellPosition();
        }

        occupiedCells.Add(cellPosition);
        return cellPosition;
    }

    private void SpawnPixelInCell(Vector3Int cellPosition)
    {
        var position = _grid.CellToWorld(cellPosition) - new Vector3(cellSize / 2, cellSize / 2, 0);

        var scale = _grid.cellSize;

        var pixel = Instantiate(_pixelPrefab, position, Quaternion.identity, transform);
        pixel.transform.localScale = scale;
        pixel.name = $"Pixel{cellPosition}";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_bounds.bounds.center, _bounds.bounds.size);
    }
}
