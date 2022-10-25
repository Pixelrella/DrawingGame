using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    private List<Vector3Int> occupiedCells = new List<Vector3Int>();
    private List<Pixel> coloredPixels = new List<Pixel>();

    public delegate void RoundAction();
    public RoundAction RoundEnd;
    public RoundAction RoundStart;

    public void Reset()
    {
        occupiedCells.Clear();
        coloredPixels.Clear();
    }

    public bool IsOccupied(Vector3Int cell)
    {
        return occupiedCells.Contains(cell);
    }

    public void Add(Vector3Int cell)
    {
        occupiedCells.Add(cell);
    }

    public void TryAddColoredCell(Pixel pixel)
    {
        if (coloredPixels.Contains(pixel))
        {
            return;
        }

        coloredPixels.Add(pixel);

        if (coloredPixels.Count == occupiedCells.Count)
        {
            RoundEnd.Invoke();
        }
    }

}