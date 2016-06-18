using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BrickGrid : Grid3D<Brick>
{
    private BrickPool pool;
    private Noise2D noise;

    public BrickGrid(Vector3i brickResolution, Noise2D noise) : base((int)Math.Pow(2, brickResolution.x), (int)Math.Pow(2, brickResolution.y), (int)Math.Pow(2, brickResolution.z))
    {
        this.noise = noise;
        pool = new BrickPool(brickResolution);
    }
}
