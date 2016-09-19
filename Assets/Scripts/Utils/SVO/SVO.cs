using UnityEngine;
using System.Collections;
using System;

public class SVO {

    private SparseTexture sparseTexture;

    private int pixelsWide;

    private int tilesWide;

    private int tilesHigh;

    private int tileSize;

    private int brickSideLength;

    private int bricksWide;

    private int bricksHigh;

    public SVO(int resolution)
    {
        // The number of voxels along one edge
        pixelsWide = 4 *  (int) Math.Pow(2, resolution);

        sparseTexture = new SparseTexture(pixelsWide, pixelsWide, TextureFormat.R16, 0);

        tileSize = sparseTexture.tileWidth * sparseTexture.tileHeight;

        tilesWide = pixelsWide / sparseTexture.tileWidth;

        tilesHigh = pixelsWide / sparseTexture.tileHeight;

        brickSideLength = (int)Math.Floor(Math.Pow(tilesWide * tilesHigh, 1.0f / 3.0f));

        bricksWide = (pixelsWide / brickSideLength) / 4;
    }

    public void SetBrickAt(int x, int y, int z, SVOBrick brick)
    {
        int index = x +  y * bricksWide + z * bricksWide * bricksWide;

        int indexX = index % bricksWide;

        int indexY = bricksWide / bricksWide;

        sparseTexture.UpdateTileRaw(indexX, indexY, 0, brick.AsByteArray());
    }

}


/*
}

SVOBrick svoBrick = new SVOBrick(40, 40, 40);
svoBrick.SetAt(2, 2, 2, 100);
UInt16 t = svoBrick.GetAt(2, 2, 2);
SVO svo = new SVO(10);
svo.SetBrickAt(2, 2, 2, svoBrick);
*/
