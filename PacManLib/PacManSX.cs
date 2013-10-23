#region File Description
    //////////////////////////////////////////////////////////////////////////
   // PacManSX                                                             //
  //                                                                      //
 // Copyright (C) Veritas. All Rights reserved.                          //
//////////////////////////////////////////////////////////////////////////
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PacManLib.Map;
using PacManLib.GameObjects;
#endregion End of Using Statements

namespace PacManLib
{
    /// <summary>
    /// This is the main game class.
    /// </summary>
    public sealed class PacManSX
    {
        #region Consts

        public const int TitleHeight = 20;

        public const int TileWidth = 20;
        public const int TileHeight = 20;

        public const int CharacterWidth = 28;
        public const int CharacterHeight = 28;

        #endregion

        #region Fields

        private TileMap tileMap = null;
        private GameManager gameManager = null;

        private int lives = 3;
        private int score = 0;
        private Player player = null;

        private Ghost purpleGhost = null;
        private Ghost yellowGhost = null;
        private Ghost blueGhost = null;
        private Ghost greenGhost = null;

        private SpriteFont font = null;
        private Vector2 levelPosition;
        private Vector2 scorePosition;

        private Texture2D lifeTexture;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new PacMan Shooter Extreme object.
        /// </summary>
        /// <param name="gameManager">The game manager.</param>
        public PacManSX(GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.font = this.gameManager.ContentManager.Load<SpriteFont>("Font");
            this.levelPosition = new Vector2(4, 0);
            this.scorePosition = new Vector2(120, 0);

            this.lifeTexture = this.gameManager.ContentManager.Load<Texture2D>("Life");

            this.tileMap = new TileMap(gameManager, 1, new int[,]
                {
                    { 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6 },
                    { 2, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 2 },
                    { 2, 0, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 8, 0, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 1, 1, 1, 1, 1, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 8, 0, 0, 0, 0, 8, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 5, 1, 1, 6, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 1, 1, 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 8, 0, 0, 0, 0, 8, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1, 1, 1, 1, 1, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 0, 2 },
                    { 2, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 2 },
                    { 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 }
                });

            this.player = new Player(this.gameManager, new Vector2(60, 40),
                this.gameManager.ContentManager.Load<Texture2D>("Pacman"), PacManSX.CharacterWidth, PacManSX.CharacterHeight) { Direction = Direction.Right };

            this.blueGhost = new Ghost(this.gameManager, new Vector2(560, 40),
                this.gameManager.ContentManager.Load<Texture2D>("BlueGhost"), PacManSX.CharacterWidth, PacManSX.CharacterHeight) { Direction = Direction.Right };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Allows the game to update itself.
        /// </summary>
        /// <param name="elapsedGameTime">Elapsed time since the last update.</param>
        public void Update(TimeSpan elapsedGameTime)
        {
            // Updates the player and movement.
            this.player.Update(elapsedGameTime);
            this.blueGhost.Update(elapsedGameTime);
            Movement(elapsedGameTime);
        }
        
        /// <summary>
        /// Allows the game to draw itself.
        /// </summary>
        /// <param name="elapsedGameTime">Elapsed time since the last draw.</param>
        public void Draw(TimeSpan elapsedGameTime)
        {
            // Clear the screen to black and draw the map and player.
            this.gameManager.SpriteBatch.GraphicsDevice.Clear(Color.Black);
            this.tileMap.Draw(elapsedGameTime);
            this.player.Draw(elapsedGameTime);
            this.blueGhost.Draw(elapsedGameTime);

            // Draw the GUI.
            this.gameManager.SpriteBatch.Begin();
            this.gameManager.SpriteBatch.DrawString(this.font, "Level: " + this.tileMap.Level, this.levelPosition, Color.White);
            for (int i = 0; i < this.lives; i++)
                this.gameManager.SpriteBatch.Draw(this.lifeTexture, new Vector2(gameManager.ScreenWidth - lives * 20 - 4, 4) + new Vector2(20 * i, 0), Color.White);
            this.gameManager.SpriteBatch.DrawString(this.font, "Score: " + this.score, this.scorePosition, Color.White);
            this.gameManager.SpriteBatch.End();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Method for handling movement and input.
        /// </summary>
        private void Movement(TimeSpan elapsedGameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Direction direction = this.player.NextDirection;
            Vector2 motion = this.player.Motion;

            // Converts the center of the player to a cell, to get the cell the player is most in.
            Point playerCoords = PacManSX.ConvertPositionToCell(this.player.Center);
            Tile playerTile = tileMap.GetTile(playerCoords); // Get the tile the player is located at.
            
            // Check if the tile is a turn or path tile.
            if (playerTile.TileContent == TileContent.Turn || playerTile.TileContent == TileContent.Path)
            {
                // Convert the cell to a position.
                Vector2 playerTilePosition = PacManSX.ConvertCellToPosition(playerCoords);

                // Check if the player is right ontop of the tile.
                if (playerTilePosition == this.player.Position - new Vector2(0, PacManSX.TitleHeight))
                {
                    // Check for input, should we change direction?
                    if (keyboardState.IsKeyDown(Keys.W))
                        direction = Direction.Up;
                    else if (keyboardState.IsKeyDown(Keys.S))
                        direction = Direction.Down;
                    else if (keyboardState.IsKeyDown(Keys.A))
                        direction = Direction.Left;
                    else if (keyboardState.IsKeyDown(Keys.D))
                        direction = Direction.Right;

                    // Check if the player can move in that direction
                    if (CanCharacterMove(playerCoords, direction, out motion))
                    {
                        this.player.Motion = motion;
                        this.player.Direction = direction;
                        this.player.NextDirection = direction;
                    }
                    else
                    {
                        // If the player can't move in that direction then check if the player can move in the old direction.
                        if (CanCharacterMove(playerCoords, this.player.Direction, out motion))
                        {
                            this.player.NextDirection = direction;
                            this.player.Motion = motion;
                        }
                    }
                    // If the player can't move in the old direction or the new then just stand still.
                }
                else
                {
                    // If the player is not right ontop of the tile then continue to move.
                    if (this.player.Direction == Direction.Up)
                        motion.Y--;
                    else if (this.player.Direction == Direction.Down)
                        motion.Y++;
                    else if (this.player.Direction == Direction.Left)
                        motion.X--;
                    else if (this.player.Direction == Direction.Right)
                        motion.X++;
                }
            }

            // Check if we should move.
            if (motion != Vector2.Zero)
            {
                // Normalize the motion vector and move the player.
                motion.Normalize();

                this.player.Position.X += (float)Math.Round((motion * 300 * (float)elapsedGameTime.TotalSeconds).X);
                this.player.Position.Y += (float)Math.Round((motion * 300 * (float)elapsedGameTime.TotalSeconds).Y);
            }
        }

        /// <summary>
        /// Helper for checking if someone can move in a given direction.
        /// </summary>
        /// <param name="coords">The coordinates of the tile.</param>
        /// <param name="direction">The direction to be checked.</param>
        /// <param name="motion">If the player couldn't move then it's set to (0, 0); otherwise it's set to the direction.</param>
        /// <returns>True if the player can move in that direction; otherwise false.</returns>
        private bool CanCharacterMove(Point coords, Direction direction, out Vector2 motion)
        {
            motion = Vector2.Zero;
            Point target = new Point();

            // Set the target tile depending on the direction.
            if (direction == Direction.Up)
            {
                target = new Point(0, -1);
                target.X += coords.X;
                target.Y += coords.Y;
                motion.Y--;
            }
            else if (direction == Direction.Down)
            {
                target = new Point(0, 1);
                target.X += coords.X;
                target.Y += coords.Y;
                motion.Y++;
            }
            else if (direction == Direction.Left)
            {
                target = new Point(-1, 0);
                target.X += coords.X;
                target.Y += coords.Y;
                motion.X--;
            }
            else if (direction == Direction.Right )
            {
                target = new Point(1, 0);
                target.X += coords.X;
                target.Y += coords.Y;
                motion.X++;
            }
            else
                return false;

            // Get the tile at the target.
            Tile tile = this.tileMap.GetTile(target);

            // If the player can move then return true.
            if (tile.TileContent == TileContent.Path || tile.TileContent == TileContent.Turn)
                return true;

            // else set motion to (0, 0) and return false.
            motion = Vector2.Zero;
            return false;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Converts a pixel to the tile coordinates.
        /// </summary>
        /// <param name="position">The pixel position.</param>
        /// <returns>The tile cell coordinates.</returns>
        public static Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)((position.Y - PacManSX.TitleHeight) / (float)TileHeight));
        }

        /// <summary>
        /// Converts a tile coordinate to the position of the tile.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static Vector2 ConvertCellToPosition(Point cell)
        {
            return new Vector2(cell.X * PacManSX.TileWidth, cell.Y * PacManSX.TileHeight);
        }

        /// <summary>
        /// Creates a rectangle around a specific tile cell.
        /// </summary>
        /// <param name="tileCoordinates">The tile cell.</param>
        /// <returns>The rectangle around the given tile cell point.</returns>
        public static Rectangle CreateRectForTile(Point tileCoordinates)
        {
            return new Rectangle(
                tileCoordinates.X * TileWidth, 
                tileCoordinates.Y * TileHeight,
                TileWidth, TileHeight);
        }

        #endregion
    }
}