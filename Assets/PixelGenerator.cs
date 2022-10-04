using UnityEngine;

public class PixelGenerator : MonoBehaviour
{
    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private Collider2D _bounds;
    [SerializeField] private Grid _grid;

    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnPixelAtRandom();
        }
    }

    private void SpawnPixelAtRandom()
    {
        var randomX = Random.Range(_bounds.bounds.min.x, _bounds.bounds.max.x);
        var randomY = Random.Range(_bounds.bounds.min.y, _bounds.bounds.max.y);

        SpawnPixelAt(new Vector3(randomX, randomY));
    }

    private void SpawnPixelAt(Vector3 position)
    {
        Vector3Int cellPosition = _grid.WorldToCell(position);
        position = _grid.CellToWorld(cellPosition);

        Instantiate(_pixelPrefab, position, Quaternion.identity, transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_bounds.bounds.center, _bounds.bounds.size);
    }
}
