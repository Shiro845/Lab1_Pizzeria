namespace Pizzeria;
public class IdGenerator
{
    public static int GenerateNewId(string path)
    {
        if (!File.Exists(path))
            return 1;

        var lines = File.ReadAllLines(path).Skip(1);

        int max = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (int.TryParse(parts[0], out int id))
            {
                if (id > max)
                    max = id;
            }
        }

        return max + 1;
    }
}