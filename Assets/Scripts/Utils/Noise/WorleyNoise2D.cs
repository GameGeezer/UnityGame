using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WorleyNoise2D : Noise2D
{
    private int featurePoints, width, height;

    private float[,] texture;

    private KDTree<byte> tree = new KDTree<byte>();

    public WorleyNoise2D(int featurePoints, int width, int height, int depth, float scale, float mag, float exp)
    {
        this.featurePoints = featurePoints;
        this.width = width;
        this.height = height;

        for(int i = 0; i < featurePoints; ++i)
        {
            float x = UnityEngine.Random.Range(0, width);
            float y = UnityEngine.Random.Range(0, height);
            float z = UnityEngine.Random.Range(0, depth);

            tree.Insert(0, new Vector3(x, y, z));
        }

        tree.Refresh();

        texture = new float[width, height];

        Vector3 dummyPosition = new Vector3();
        float highest = 0.0f;
        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                dummyPosition.Set(x / scale, y / scale, 0);

                KDTreeEntry<byte> entry = tree.NearestNeighbor(dummyPosition);

                float distanceFrom = (entry.position - dummyPosition).magnitude;

                highest = distanceFrom > highest ? distanceFrom : highest;
                texture[x, y] = (float)(Mathf.Pow( distanceFrom * mag, exp));
            }
        }

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                texture[x, y] /= highest;

                texture[x, y] *= scale;
            }
        }

    }

    public override int generate(float x, float y)
    {
        return (int) texture[(int)x, (int)y];
    }
}
