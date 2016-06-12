using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChunkPool
{
    private static Pool<Chunk> pool = new Pool<Chunk>();

    public static Chunk Catch()
    {
        lock(pool)
        {
            return pool.Catch();
        }
    }

    public static void Release(Chunk chunk)
    {
        lock(pool)
        {
            pool.Release(chunk);
        }
    }
}
