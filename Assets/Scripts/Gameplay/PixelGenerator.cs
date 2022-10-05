using UnityEngine;

public class PixelGenerator : MonoBehaviour
{
    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private Collider2D _bounds;
    [SerializeField] private Grid _grid;

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

        for (int i = 0; i < 100; i++)
        {
            SpawnPixelAtRandom();
        }
    }

    private void SpawnPixelAtRandom()
    {
        var randomX = Random.Range(_bounds.bounds.min.x, _bounds.bounds.max.x);
        var randomY = Random.Range(_bounds.bounds.min.y, _bounds.bounds.max.y);

        //TODO: do not spawn where other pixel is

        SpawnPixelAt(new Vector3(randomX, randomY));
    }

    private void SpawnPixelAt(Vector3 position)
    {
        Vector3Int cellPosition = _grid.WorldToCell(position);
        position = _grid.CellToWorld(cellPosition);

        var pixel = Instantiate(_pixelPrefab, position, Quaternion.identity, transform);
        pixel.name = $"Pixel{cellPosition}";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_bounds.bounds.center, _bounds.bounds.size);
    }
}
