using System;
using System.Collections.Generic;


using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Drawing;

namespace myLevelEditor
{
    public partial class Form1 : Form
    {
        Camera camera = new Camera();
        SpriteBatch spriteBatch;
        TileMap level1;
        int[,] tempMap;
        int maxWidth = 0;
        int maxHeight = 0;
        int cellX,cellY;
        // Dictionary<int, Image> previewDict = new Dictionary<int, Image>();
        List<Image> TileImageList = new List<Image>();
        public static Texture2D tileSheet;

        public GraphicsDevice GraphicsDevice 
        {
            get { return tileDisplay1.GraphicsDevice; }
        }
        public Form1()
        {
            
            InitializeComponent();
            tileDisplay1.OnInitialize += new EventHandler(tileDisplay1_OnInitialize);
            tileDisplay1.OnDraw += new EventHandler(tileDisplay1_OnDraw);

            Application.Idle += delegate { tileDisplay1.Invalidate();};
            openFileDialog1.Filter = "Tile Map|*.map";
            saveFileDialog1.Filter = "Tile Map|*.map";
            Mouse.WindowHandle = tileDisplay1.Handle;



        }
        
        void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            Logic();
            Render();
            
        }

        private void Render()
        {
            spriteBatch.Begin();
            if (level1 != null) 
            {
                  level1.Draw(spriteBatch,camera);
            }          
            spriteBatch.End();           
            
            spriteBatch.Begin();

            if (level1 != null)
            {
                if (cellX != -1 && cellY != -1)
                {
                    spriteBatch.Draw(tileSheet,
                        new Microsoft.Xna.Framework.Rectangle(
                        cellX * Tile.WIDTH - (int)camera.Position.X,
                        cellY * Tile.HEIGHT - (int)camera.Position.Y,
                        Tile.WIDTH,
                        Tile.HEIGHT),
                        Microsoft.Xna.Framework.Color.Red);
                    spriteBatch.Draw(tileSheet,
                        new Vector2(cellX * Tile.WIDTH - (int)camera.Position.X, cellY * Tile.HEIGHT - camera.Position.Y),
                        new Microsoft.Xna.Framework.Rectangle(Tile.WIDTH * 9, 0, Tile.WIDTH, Tile.HEIGHT),
                        Microsoft.Xna.Framework.Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0f);
                } 
            }
                
            spriteBatch.End();
                 
            
        }

        private void Logic()
        {
            camera.Position.X = hScrollBar1.Value * Tile.WIDTH;
            camera.Position.Y = vScrollBar1.Value * Tile.HEIGHT;
            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;
            if (level1 != null)
            {
                if (mx >= 0 && mx < tileDisplay1.Width &&
                       my >= 0 && my < tileDisplay1.Height)
                {
                    cellX = mx / Tile.WIDTH;
                    cellY = my / Tile.HEIGHT;
                    cellX += hScrollBar1.Value;
                    cellY += vScrollBar1.Value;

                    cellX = (int)MathHelper.Clamp(cellX, 0, level1.Width);
                    cellY = (int)MathHelper.Clamp(cellY, 0, level1.Height);
                    if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && cellX != -1 && cellY != -1)
                    {
                        if (DrawRadioButton.Checked)
                        {
                            level1.SetCellIndex(cellX, cellY, listBox1.SelectedIndex);
                        }
                        else if (EraseRadioButton.Checked)
                        {
                            level1.SetCellIndex(cellX, cellY, 10);
                        }
                    }
                }
                else
                {
                    cellX = cellY = 10;
                }
               

            }
        }

        void tileDisplay1_OnInitialize(object sender, EventArgs e)
        {
            //level1 = new TileMap( TileMap.FromFile(@"c:\users\kevin\documents\Visual Studio 2010\Projects\myLevelEditor\myLevelEditor\Maps\TextFile1.map"));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileSheet = Texture2DFromFile(GraphicsDevice, @"c:\users\kevin\documents\Visual Studio 2010\Projects\myLevelEditor\myLevelEditor\Content\TileMap.png");
            Image imgsrc = Image.FromFile(@"c:\users\kevin\documents\Visual Studio 2010\Projects\myLevelEditor\myLevelEditor\Content\TileMap.png");
            Bitmap bmpimg = new Bitmap(imgsrc); 
            int[] NumberofTiles = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int x = 0; x < NumberofTiles.Length; x++)
            {
                Bitmap crop = bmpimg.Clone(new System.Drawing.Rectangle(x * Tile.WIDTH, 0, Tile.WIDTH, Tile.HEIGHT), imgsrc.PixelFormat);
                TileImageList.Add(crop);                    
            }
            pictureBox1.Image = TileImageList[0];          
            
            foreach (int num in NumberofTiles)
            {
                listBox1.Items.Add(num) ;
            }          

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        public Texture2D Texture2DFromFile(GraphicsDevice GraphicsDevice, string fileName) 
        {            
            Texture2D texture = null;       
            FileStream stream = new FileStream(fileName, FileMode.Open);
            texture = Texture2D.FromStream(GraphicsDevice, stream);
            stream.Close();            
            return texture;
        }

        private void newTileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTileMap form = new NewTileMap();
            form.ShowDialog();

            if (form.OKPressed) 
            {
                TileMap tempLevel = new TileMap(
                    int.Parse(form.widthTextBox.Text),
                    int.Parse(form.heightTextBox.Text));
                level1 = tempLevel;
                AdjustScrollBars(level1);
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                string filename = openFileDialog1.FileName;
                level1 = new TileMap(TileMap.FromFile(filename));

                AdjustScrollBars(level1);
            }
        }

        private void AdjustScrollBars(TileMap tilemap)
        {
            if (tilemap.MapLengthInPixelsX > tileDisplay1.Width)
            {
                maxWidth = (int)Math.Max(tilemap.Width, maxWidth);

                hScrollBar1.Visible = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = maxWidth;

            }
            if (tilemap.MapLengthInPixelsY > tileDisplay1.Height)
            {
                maxHeight = (int)Math.Max(tilemap.Height, maxHeight);
                vScrollBar1.Visible = true;
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = maxHeight;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null) 
            {
                int tileIndex = listBox1.SelectedIndex;
                saveFileDialog1.FileName = "MapLevel";
                TileMap tilemap = level1;
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                {
                    tilemap.Save(saveFileDialog1.FileName);
                }
               
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null) 
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (listBox1.SelectedIndex == i)
                    {
                        pictureBox1.Image = TileImageList[i];
                    }    
                }               
            }
        }
    }
}
