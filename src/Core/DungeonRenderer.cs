using Dungeon_Crawl.src.Character;
using Dungeon_Crawl.src.Dungeon;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Crawl.src
{
    /// <summary>
    /// The DungeonRenderer class is responsible for rendering a dungeon in a game. It takes in a dungeon object and uses various rendering techniques to display the dungeon on the screen.
    /// </summary>
    ///
    internal class DungeonRenderer
    {
        private Bitmap drawingBitmap;
        private Image wallTexture;
        private float fov = (float)Math.PI / 4f;
        private MapData map; // Your map data
        private double playerX, playerY, dirX, dirY, cameraPlaneX, cameraPlaneY;
        private int playerAngle;

        private float offsetX = .50f;
        private float offsetY = .50f;
        private static DungeonRenderer instance;

        public static DungeonRenderer Instance { get => instance; private set => instance = value; }

        public DungeonRenderer(int width, int height, Image texture)
        {
            instance = this;
            this.drawingBitmap = new Bitmap(1920, 1080);
            this.wallTexture = texture;
            PlayerMovement playerMovement = Player.Instance.GetComponent<PlayerMovement>();
            playerX = playerMovement.GetPositionX();
            playerY = playerMovement.GetPositionY();

            playerMovement.OnMove += OnPlayerMovement;
            playerMovement.OnTurn += OnPlayerTurn;
        }

        private void OnPlayerMovement(Point newPosition)
        {
            // Update player position
            playerX = newPosition.X + offsetX;
            playerY = newPosition.Y + offsetY;
            DungeonForm form = DungeonForm.ActiveForm as DungeonForm;
            form.InvalidateDungeonBackground();
        }

        private void OnPlayerTurn(int direction)
        {
            // Update player angle
            playerAngle = direction * 90;
            DungeonForm form = DungeonForm.ActiveForm as DungeonForm;
            form.InvalidateDungeonBackground();
        }

        public Bitmap RenderFrame()
        {
            DrawDungeon();
            return drawingBitmap;
        }

        // Paint event handler for PictureBox
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            DrawDungeon();
            e.Graphics.DrawImage(drawingBitmap, 0, 0);
        }

        private void DrawDungeon()
        {
            // Clear the bitmap with white color
            using (Graphics gfx = Graphics.FromImage(drawingBitmap))
            {
                gfx.Clear(Color.White);
            }

            int screenWidth = drawingBitmap.Width;
            int screenHeight = drawingBitmap.Height;

            // Initialize the Z-buffer
            double[] zBuffer = new double[screenWidth];
            for (int i = 0; i < screenWidth; i++)
            {
                zBuffer[i] = float.MaxValue; // A very high number, signifying a 'far away' depth
            }

            // Lock the bitmap data
            BitmapData data = drawingBitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight),
                ImageLockMode.ReadWrite, drawingBitmap.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(data.PixelFormat) / 8;
            byte[] pixels = new byte[data.Height * data.Stride];
            IntPtr ptrFirstPixel = data.Scan0;

            System.Runtime.InteropServices.Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            map = MapData.Get; // Assuming MapData.Get returns a valid map object

            // Update camera direction and camera plane based on player angle
            double playerAngleRadians = playerAngle * Math.PI / 180;
            dirX = Math.Cos(playerAngleRadians);
            dirY = Math.Sin(playerAngleRadians);
            cameraPlaneX = -dirY * fov;
            cameraPlaneY = dirX * fov;

            for (int x = 0; x < screenWidth; x++)
            {
                double cameraX = 2 * x / (double)screenWidth - 1; // x-coordinate in camera space
                double rayDirX = dirX + cameraPlaneX * cameraX;
                double rayDirY = dirY + cameraPlaneY * cameraX;

                int mapX = (int)playerX;
                int mapY = (int)playerY;

                double deltaDistX = Math.Abs(1 / rayDirX);
                double deltaDistY = Math.Abs(1 / rayDirY);

                double perpWallDist;

                int stepX, stepY;
                bool hit = false;
                int side = 0;

                double sideDistX, sideDistY;
                if (rayDirX < 0)
                {
                    stepX = -1;
                    sideDistX = (playerX - mapX) * deltaDistX;
                }
                else
                {
                    stepX = 1;
                    sideDistX = (mapX + 1.0 - playerX) * deltaDistX;
                }
                if (rayDirY < 0)
                {
                    stepY = -1;
                    sideDistY = (playerY - mapY) * deltaDistY;
                }
                else
                {
                    stepY = 1;
                    sideDistY = (mapY + 1.0 - playerY) * deltaDistY;
                }

                // DDA Algorithm
                while (!hit)
                {
                    if (sideDistX < sideDistY)
                    {
                        sideDistX += deltaDistX;
                        mapX += stepX;
                        side = 0;
                    }
                    else
                    {
                        sideDistY += deltaDistY;
                        mapY += stepY;
                        side = 1;
                    }
                    if (map.GetTile(mapX, mapY) > 0) hit = true;
                }
                if (side == 0) perpWallDist = (mapX - playerX + (1 - stepX) / 2) / rayDirX;
                else perpWallDist = (mapY - playerY + (1 - stepY) / 2) / rayDirY;

                if (perpWallDist < zBuffer[x])
                {
                    // Update Z-buffer with the depth of the current wall slice
                    zBuffer[x] = perpWallDist;

                    int lineHeight = (int)(screenHeight / perpWallDist);

                    int drawStart = -lineHeight / 2 + screenHeight / 2;
                    if (drawStart < 0) drawStart = 0;
                    int drawEnd = lineHeight / 2 + screenHeight / 2;
                    if (drawEnd >= screenHeight) drawEnd = screenHeight - 1;

                    double wallX; // Where exactly the wall was hit
                    if (side == 0) wallX = playerY + perpWallDist * rayDirY;
                    else wallX = playerX + perpWallDist * rayDirX;
                    wallX -= Math.Floor(wallX);

                    for (int y = drawStart; y < drawEnd; y++)
                    {
                        float sampleY = (y - drawStart) / (float)lineHeight;
                        Color textureColor = GetTextureColor(wallTexture, (float)wallX, sampleY);

                        int pixelIndex = y * data.Stride + x * bytesPerPixel;
                        pixels[pixelIndex + 2] = textureColor.R; // Red
                        pixels[pixelIndex + 1] = textureColor.G; // Green
                        pixels[pixelIndex] = textureColor.B; // Blue
                        if (bytesPerPixel == 4)
                        {
                            pixels[pixelIndex + 3] = textureColor.A; // Alpha (for 32bpp)
                        }
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            drawingBitmap.UnlockBits(data);
        }

        private Color GetTextureColor(Image texture, float wallX, float sampleY)
        {
            Bitmap bmp = texture as Bitmap;
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                              ImageLockMode.ReadOnly, bmp.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            byte[] pixelData = new byte[bmpData.Height * bmpData.Stride];
            IntPtr ptrFirstPixel = bmpData.Scan0;

            System.Runtime.InteropServices.Marshal.Copy(ptrFirstPixel, pixelData, 0, pixelData.Length);

            int textureX = (int)Clamp((wallX * bmp.Width), 0, bmp.Width - 1);
            int textureY = (int)Clamp((sampleY * bmp.Height), 0, bmp.Height - 1);
            int pixelIndex = textureY * bmpData.Stride + textureX * bytesPerPixel;

            byte blue = pixelData[pixelIndex];
            byte green = pixelData[pixelIndex + 1];
            byte red = pixelData[pixelIndex + 2];
            byte alpha = bytesPerPixel == 4 ? pixelData[pixelIndex + 3] : (byte)255;

            bmp.UnlockBits(bmpData);

            return Color.FromArgb(alpha, red, green, blue);
        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            else if (value > max)
                value = max;

            return value;
        }

        public void UpdateViewData(PlayerView view)
        {
            // Update player position and view data
            playerX = view.PlayerPosition.X + offsetX;
            playerY = view.PlayerPosition.Y + offsetY;
            dirX = view.DirX;
            dirY = view.DirY;
            cameraPlaneX = view.CameraPlaneX;
            cameraPlaneY = view.CameraPlaneY;
            playerAngle = (int)view.FacingDirection * 90;
        }
    }
}