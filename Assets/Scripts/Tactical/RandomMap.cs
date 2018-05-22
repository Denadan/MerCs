using UnityEngine;

namespace Mercs.Tactical
{
    public class RandomMap : Map
    {
        public override void Generate(HexOrinetation orientaion)
        {
            map = new TileInfo[SizeX, SizeY];
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    map[i, j] = new TileInfo()
                    {
                        Feature = TileFeature.None,
                        CellCoord = new Vector3Int(i, j, Random.Range(0, 5))
                    };
                    switch (map[i, j].Height)
                    {
                        case 0:
                            map[i, j].Feature = TileFeature.Water;
                            break;
                        case 1:
                        case 2:
                            map[i, j].Feature = TileFeature.None;
                            break;
                        case 3:
                            map[i, j].Feature = TileFeature.Forest;
                            break;

                        case 4:
                            map[i, j].Feature = TileFeature.Rough;
                            break;

                    }
                }
        }
    }
}