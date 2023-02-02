using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCharamon;

public class Wilds
{
    public static float[,] preMap = new float[64, 64];
    public static char[][] map = new char[64][];

    public static void Generate()
    {
        Random random = new Random();
    findMap:

        SetValues();
        /*for (int x = 0; x < 64; x++)
        {
            char[] chars = new char[64];
            for (int y = 0; y < 64; y++)
            {
                chars[y] = map[x][y];
            }
            Console.WriteLine(chars);
        }*/
        if (!IsPath())
        {
            Noise2d.Reseed();
            goto findMap;
        }
        SetPool();
    }

    static void SetPool()
    {
        Random random = new Random();
        for (int i = 0; i < Maps.WildsPool.GetLength(1); i++)
        {
            float proba = (100f / Maps.WildsPool.GetLength(1)) * (i + 1);
            Maps.WildsPool[0, i] = random.Next(809);
            Maps.WildsPool[1, i] = proba;
        }
    }

    static bool IsaPath(int i, int j, int[,] visited)
    {

        if ((i >= 1 && i < 63 && j >= 0 && j < 63) && map[i][j] != 't' && map[i][j] != 'm' && visited[i, j] == 0)
        {
            visited[i, j] = 1;
            if (map[i][j] == 'I') return true;

            bool left = IsaPath(i - 1, j, visited);
            if (left) return true;

            bool up = IsaPath(i, j - 1, visited);
            if (up) return true;

            bool right = IsaPath(i + 1, j, visited);
            if (right) return true;

            bool down = IsaPath(i, j + 1, visited);
            if (down) return true;
        }
        return false;
    }

    static bool IsPath()
    {
        int[,] visited = new int[64, 64];

        bool flag = false;
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                if (map[i][j] == 'H' && visited[i, j] == 0)
                {
                    if (IsaPath(i, j, visited))
                    {
                        flag = true;
                        break;
                    }
                }
            }
        }
        return flag;
    }
    public static void SetValues()
    {
        for (int i = 0; i < 64; i++)
        {
            char[] chars = new char[64];
            for (int j = 0; j < 64; j++)
            {
                preMap[i, j] = Noise2d.Noise(i / 64.0f, j / 64.0f) * 6;
                if (preMap[i, j] < 0)
                {
                    preMap[i, j] *= -1;
                }

                if (i == 63 || i == 0 || j == 63 || j == 0) chars[j] = 'm';
                else chars[j] = FromFloatToChar(preMap[i, j]);
            }
            map[i] = chars;
        }
        map[1][32] = 'I';
        map[62][32] = 'H';
    }
    public static char FromFloatToChar(float n)
    {
        char c;
        if (n > 2) c = 'm';
        else if (n > 1) c = 'g';
        else c = ' ';

        if (n > 1.9f && n < 2) c = 't';

        return c;
    }
}

/// implements improved Perlin noise in 2D. 
/// Transcribed from http://www.siafoo.net/snippet/144?nolinenos#perlin2003
/// </summary>
public static class Noise2d
{
    private static Random _random = new Random();
    private static int[] _permutation;

    private static Vector2[] _gradients;

    static Noise2d()
    {
        CalculatePermutation(out _permutation);
        CalculateGradients(out _gradients);
    }

    private static void CalculatePermutation(out int[] p)
    {
        p = Enumerable.Range(0, 256).ToArray();

        /// shuffle the array
        for (var i = 0; i < p.Length; i++)
        {
            var source = _random.Next(p.Length);

            var t = p[i];
            p[i] = p[source];
            p[source] = t;
        }
    }

    /// <summary>
    /// generate a new permutation.
    /// </summary>
    public static void Reseed()
    {
        CalculatePermutation(out _permutation);
    }

    private static void CalculateGradients(out Vector2[] grad)
    {
        grad = new Vector2[256];

        for (var i = 0; i < grad.Length; i++)
        {
            Vector2 gradient;

            do
            {
                gradient = new Vector2((float)(_random.NextDouble() * 2 - 1), (float)(_random.NextDouble() * 2 - 1));
            }
            while (gradient.LengthSquared() >= 1);

            gradient /= gradient.Length();

            grad[i] = gradient;
        }

    }

    private static float Drop(float t)
    {
        t = Math.Abs(t);
        return 1f - t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Q(float u, float v)
    {
        return Drop(u) * Drop(v);
    }

    public static float Noise(float x, float y)
    {
        var cell = new Vector2((float)Math.Floor(x), (float)Math.Floor(y));

        var total = 0f;

        var corners = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

        foreach (var n in corners)
        {
            var ij = cell + n;
            var uv = new Vector2(x - ij.X, y - ij.Y);

            var index = _permutation[(int)ij.X % _permutation.Length];
            index = _permutation[(index + (int)ij.Y) % _permutation.Length];

            var grad = _gradients[index % _gradients.Length];

            total += Q(uv.X, uv.Y) * Vector2.Dot(grad, uv);
        }

        return Math.Max(Math.Min(total, 1f), -1f);
    }

}