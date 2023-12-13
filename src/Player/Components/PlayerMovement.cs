using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dungeon_Crawl.src.Dungeon;

namespace Dungeon_Crawl.src.PlayerCore.Components
{
    public class PlayerMovement : IComponent
    {
        public delegate void MoveEventHandler(Point newPosition);

        public delegate void TurnEventHandler(int direction);

        public event MoveEventHandler OnMove;

        public event TurnEventHandler OnTurn;

        public delegate void ViewChangedEventHandler(PlayerView newView);

        public event ViewChangedEventHandler OnViewChanged;

        public enum Direction
        { North, East, South, West }

        private Direction currentDirection = Direction.North;
        private Point currentPosition;

        // Direction vector components
        public double dirX, dirY;

        // Camera plane vector components
        public double cameraPlaneX, cameraPlaneY;

        private double playerAngle = 0;

        public PlayerMovement()

        {
        }

        public void Initialize()
        {
            currentPosition.X = 2;
            currentPosition.Y = 2;
            SetInitialDirectionAndPlane();
        }

        private void SetInitialDirectionAndPlane()
        {
            // Assuming North is Up (+Y), East is Right (+X), South is Down (-Y), West is Left (-X)
            switch (currentDirection)
            {
                case Direction.North:
                    dirX = 0; dirY = 1;
                    cameraPlaneX = 0.66f; cameraPlaneY = 0;
                    break;

                case Direction.East:
                    dirX = 1; dirY = 0;
                    cameraPlaneX = 0; cameraPlaneY = -0.66f;
                    break;

                case Direction.South:
                    dirX = 0; dirY = -1;
                    cameraPlaneX = -0.66f; cameraPlaneY = 0;
                    break;

                case Direction.West:
                    dirX = -1; dirY = 0;
                    cameraPlaneX = 0; cameraPlaneY = 0.66f;
                    break;
            }
        }

        public void Update()
        {
            return;
        }

        public void MoveForward()
        {
            switch (currentDirection)
            {
                case Direction.North:
                    Move(1, 0);
                    break;

                case Direction.East:
                    Move(0, 1);
                    break;

                case Direction.South:
                    Move(-1, 0);
                    break;

                case Direction.West:
                    Move(0, -1);
                    break;
            }
        }

        public void MoveBackward()
        {
            switch (currentDirection)
            {
                case Direction.North:
                    Move(-1, 0);
                    break;

                case Direction.East:
                    Move(0, -1);
                    break;

                case Direction.South:
                    Move(1, 0);
                    break;

                case Direction.West:
                    Move(0, 1);
                    break;
            }
        }

        public void MoveLeft()
        {
            switch (currentDirection)
            {
                case Direction.North:
                    Move(0, -1);
                    break;

                case Direction.East:
                    Move(1, 0);
                    break;

                case Direction.South:
                    Move(0, 1);
                    break;

                case Direction.West:
                    Move(-1, 0);
                    break;
            }
        }

        public void MoveRight()
        {
            switch (currentDirection)
            {
                case Direction.North:
                    Move(0, 1);
                    break;

                case Direction.East:
                    Move(-1, 0);
                    break;

                case Direction.South:
                    Move(0, -1);
                    break;

                case Direction.West:
                    Move(1, 0);
                    break;
            }
        }

        private void Move(int xChange, int yChange)
        {
            if (!MapData.Get.IsWalkable(currentPosition.X + xChange, currentPosition.Y + yChange))
                return;

            currentPosition.X += xChange;
            currentPosition.Y += yChange;
            // Add boundary checks and collision detection as necessary
            OnMove?.Invoke(currentPosition);
            UpdateRendererView();
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_step);
        }

        public void TurnLeft()
        {
            // Rotate vectors left (90 degrees)
            RotateLeft();
            UpdateDirectionAfterRotation();
        }

        public void TurnRight()
        {
            // Rotate vectors right (90 degrees)
            RotateRight();
            UpdateDirectionAfterRotation();
        }

        private void RotateLeft()
        {
            // Temporary storage for direction
            double oldDirX = dirX;
            dirX = -dirY;
            dirY = oldDirX;

            // Rotate the camera plane
            double oldPlaneX = cameraPlaneX;
            cameraPlaneX = -cameraPlaneY;
            cameraPlaneY = oldPlaneX;
            switch (currentDirection)
            {
                case Direction.North:
                    currentDirection = Direction.West;
                    break;

                case Direction.East:
                    currentDirection = Direction.North;
                    break;

                case Direction.South:
                    currentDirection = Direction.East;
                    break;

                case Direction.West:
                    currentDirection = Direction.South;
                    break;
            }
        }

        private void RotateRight()
        {
            // Temporary storage for direction
            double oldDirX = dirX;
            dirX = dirY;
            dirY = -oldDirX;

            // Rotate the camera plane
            double oldPlaneX = cameraPlaneX;
            cameraPlaneX = cameraPlaneY;
            cameraPlaneY = -oldPlaneX;

            switch (currentDirection)
            {
                case Direction.North:
                    currentDirection = Direction.East;
                    break;

                case Direction.East:
                    currentDirection = Direction.South;
                    break;

                case Direction.South:
                    currentDirection = Direction.West;
                    break;

                case Direction.West:
                    currentDirection = Direction.North;
                    break;
            }
        }

        private void UpdateDirectionAfterRotation()
        {
            UpdateRendererView();
            OnTurn?.Invoke((int)currentDirection);
        }

        internal float GetPositionX()
        {
            return currentPosition.X;
        }

        internal float GetPositionY()
        {
            return currentPosition.Y;
        }

        public void UpdateRendererView()
        {
            PlayerView view = new PlayerView(dirX, dirY, cameraPlaneX, cameraPlaneY, currentDirection, currentPosition);
            OnViewChanged?.Invoke(view);
        }

        public double GetDirX() => dirX;

        public double GetDirY() => dirY;

        public double GetCameraPlaneX() => cameraPlaneX;

        public double GetCameraPlaneY() => cameraPlaneY;

        private void OnPlayerTurn(int direction)
        {
            playerAngle = direction * 90;
            DungeonForm form = DungeonForm.ActiveForm as DungeonForm;
            form.InvalidateDungeonBackground();
        }
    }

    public struct PlayerView
    {
        public double DirX { get; private set; }
        public double DirY { get; private set; }
        public double CameraPlaneX { get; private set; }
        public double CameraPlaneY { get; private set; }
        public PlayerMovement.Direction FacingDirection { get; private set; }

        public Point PlayerPosition { get; private set; }

        public PlayerView(double dirX, double dirY, double cameraPlaneX, double cameraPlaneY, PlayerMovement.Direction facingDirection, Point playerPosition)
        {
            DirX = dirX;
            DirY = dirY;
            CameraPlaneX = cameraPlaneX;
            CameraPlaneY = cameraPlaneY;
            FacingDirection = facingDirection;
            PlayerPosition = playerPosition;
        }
    }
}