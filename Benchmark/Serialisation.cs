namespace Benchmark;

public static class Serialisation
{
    private const string FileName = "unicolour-benchmark-seed";
    private static readonly string Folder = Path.GetTempPath();
    private static readonly string FilePath = Path.Combine(Folder, FileName);

    public static void WriteSeed(int seed)
    {
        File.WriteAllText(FilePath, seed.ToString());
    }

    public static int ReadSeed()
    {
        return int.Parse(File.ReadAllText(FilePath));
    }
}