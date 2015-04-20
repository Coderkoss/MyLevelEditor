using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace myLevelEditor
{
    class TileMap
    {
        List<Tile> tiles;
        int[,] map;
        int height;
        int width;
        int mapLengthInPixelsX;
        int mapLengthInPixelsY;
        public int MapLengthInPixelsX 
        {
            get { return mapLengthInPixelsX; }
            set { mapLengthInPixelsX = value; }
        }
        public int MapLengthInPixelsY
        {
            get { return mapLengthInPixelsY; }
            set { mapLengthInPixelsY = value; }
        }
        public int Width 
        {
            get { return map.GetLength(1); }
            
        }
        public int Height 
        {
            get { return map.GetLength(0); }
            
        }
        public int WidthInPixels 
        {
            get { return width * Tile.WIDTH; }
        }
        public int HeightInPixels 
        {
            get { return height * Tile.HEIGHT; }
        }
        public List<Tile> Tiles 
        {
            get { return tiles; }
            set { tiles = value; }
        }
        public int[,] Map 
        {
            get { return map; }
            set { map = value;}
        }
        public TileMap(int width, int height) 
        {
            tiles = new List<Tile>();
            map = new int[height, width];
            width = map.GetLength(1);
            height = map.GetLength(0);

            mapLengthInPixelsX = width * Tile.WIDTH;
            mapLengthInPixelsY = height * Tile.HEIGHT;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[y, x] = 4;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    
                    tiles.Add(new Tile(new Vector2(x * Tile.WIDTH, y * Tile.HEIGHT), map[y,x], Color.White));
                }
            }
        }
        public TileMap(int[,] existingMap) 
        {
            //TODO: check the diff between map = existingMap; && (int[,])existingMap.Clone();
            tiles = new List<Tile>();
            map = (int[,])existingMap.Clone();
            width = map.GetLength(1);
            height = map.GetLength(0);

            mapLengthInPixelsX = width * Tile.WIDTH;
            mapLengthInPixelsY = height * Tile.HEIGHT;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles.Add(new Tile(new Vector2(x * Tile.WIDTH, y * Tile.HEIGHT ), Map[y, x], Color.White));
                }
            }
            
        }       
        public static int [,] FromFile(string filename) 
        {
            int[,] tempMap;
            bool readingLayout = false;
            List<List<int>> tempLayout = new List<List<int>>();

            using (StreamReader reader = new StreamReader(filename))
            {
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    if(string.IsNullOrEmpty(line))
                        continue;

                    if(line.Contains("[Layout]"))
                    {
                        readingLayout = true;
                    }
                    else if(readingLayout)
                    {
                        List<int>row = new List<int>();
                        string[]cells = line.Split(' ');
                        foreach (string c in cells)
                        {
                            if(!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }
                        tempLayout.Add(row);
    
                    }
                }
             }
            int width = tempLayout[0].Count;
            int height = tempLayout.Count;
            tempMap = new int[height,width];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tempMap[y,x] =  SetCellIndex( tempLayout[y][x]);
                }
            }
            return tempMap;
           
        }
        public void Save(string filename) 
        {            
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Layout]");
                for (int y = 0; y < Height; y++)
                {
                    string line = string.Empty;
                    for (int x = 0; x < Width; x++)
                    {
                        line += map[y, x].ToString() + " ";
                    }
                    writer.WriteLine(line);
                }
            }
        }       
        public void LoadTileTextures(ContentManager content, params string[] textureNames) 
        {
            //not used
            Texture2D texture;
            foreach (string textureName in textureNames)
            {
                texture = content.Load<Texture2D>(textureName);
               // tileTextures.Add(texture);
            }
        }
        public static int SetCellIndex( int cellIndex) 
        {
            return cellIndex;
        }
        public void SetCellIndex(int cx,int cy,int index) 
        {            
            tiles.Clear();
            Map[cy, cx] = index;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {                    
                    Tile tempTile = new Tile(new Vector2(x * Tile.WIDTH, y * Tile.HEIGHT), map[y, x], Color.White);
                    Tiles.Add(tempTile);
                }
            }
        }                   
                    
        public void Draw(SpriteBatch spriteBatch,Camera camera) 
        {
            foreach (Tile tile in Tiles)
            {
                tile.Draw(spriteBatch,camera);
            }            
        }

    }
}
