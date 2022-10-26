using NUnit.Framework;

public class DifficultyProgressionTest
{
    private DifficultyProgression _progression;

    [SetUp]
    public void Init()
    {
        _progression = new DifficultyProgression(GetNumerOfPixels);
    }

    [TestCase(1, ExpectedResult = 3)]
    [TestCase(3, ExpectedResult = 3)]
    [TestCase(4, ExpectedResult = 2)]
    [TestCase(7, ExpectedResult = 1)]
    [TestCase(10, ExpectedResult = .5f)]
    [TestCase(110, ExpectedResult = .5f)]
    public float GenericLevelTestCellSize(int level)
    {
        for (int i = 1; i < level; i++)
        {
            _progression.Advance();
        }

        var current = _progression.GetGeneratorParameters();

        return current.Item1;
    }

    [TestCase(1, ExpectedResult = 3)]
    [TestCase(2, ExpectedResult = 6)]
    [TestCase(3, ExpectedResult = 9)]
    [TestCase(4, ExpectedResult = 18)]
    public int GenericLevelTestPixelNumber(int level)
    {
        for (int i = 1; i < level; i++)
        {
            _progression.Advance();
        }

        var current = _progression.GetGeneratorParameters();

        return current.Item2;
    }

    // A Test behaves as an ordinary method
    [Test]
    public void FirstLevelTestCellSize()
    {
        var current = _progression.GetGeneratorParameters();

        Assert.That(current.Item1, Is.EqualTo(3));
    }

    [Test]
    public void FirstLevelTestPixelNumber()
    {
        var current = _progression.GetGeneratorParameters();

        Assert.That(current.Item2, Is.EqualTo(3));
    }

    private int GetNumerOfPixels(float cellSize)
    {
        switch (cellSize)
        {
            case 3:
                return 15;
            case 2:
                return 80;
            case 1:
                return 250;
            case 0.5f:
            default:
                return 500;
        }
    }
}
