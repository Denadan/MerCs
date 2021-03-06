﻿using Tools;
using UnityEngine;

namespace Mercs.Tactical
{
    public class PerlinNoiseMap : Map
    {
        delegate float coord_delegate(int x, int y);
        delegate float exp_delegate(int i, int j, int n);

        public int MaxHeight = 5;
        public float ForestTrashHold = 0.3f;
        public float RoughTrashHold = 0.3f;

        public override void Generate(HexGrid grid)
        {
            map = new TileInfo[SizeX, SizeY];
            float siny = Mathf.Sin(Mathf.PI / 3);

            var max_c = grid.CellToMap(SizeX, SizeY);

            float[,,] temp_map = new float[SizeX, SizeY, 3];

            Noise height_noise = new PerlinNoise(5);
            Noise forest_noise = new PerlinNoise(10);
            Noise rough_noise = new PerlinNoise(10);

            Noise[] all_noises = { height_noise, forest_noise, rough_noise };

            float[] min = { 1, 1, 1 };
            float[] max = { -1, -1, -1 };

            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    map[i, j] = new TileInfo();

                    var coord = grid.CellToMap(i, j);

                    float x = coord.x / max_c.x;
                    float y = coord.y / max_c.y;
                    for (int a = 0; a < 3; a++)
                    {
                        float val = temp_map[i, j, a] = all_noises[a][x, y];
                        min[a] = min[a] > val ? val : min[a];
                        max[a] = max[a] < val ? val : max[a];
                    }

                }

            exp_delegate fit_value = (int x, int y, int n) => (temp_map[x, y, n] - min[n]) / (max[n] - min[n]);

            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    var tile = map[i, j];
                    int height = (int)(fit_value(i, j, 0) * MaxHeight);

                    tile.CellCoord = new Vector3Int(i, j, height);
                    if (height == 0)
                        tile.Feature = TileFeature.Water;
                    else if (fit_value(i, j, 1) > ForestTrashHold)
                        tile.Feature = TileFeature.Forest;
                    else if (fit_value(i, j, 2) > RoughTrashHold)
                        tile.Feature = TileFeature.Rough;

                }
        }
    }
}