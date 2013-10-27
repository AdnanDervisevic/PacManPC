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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PacManLib.Map;
using PacManLib.GameObjects;
using System.Timers;
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

        public const int GhostScore = 200;
        public const int RingScore = 10;
        public const int DotScore = 50;

        public const int GodModeActiveInSeconds = 10;

        #endregion

        #region Fields

        private Timer godmodeOverTimer = null;

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

        private SoundEffect soundGodMode;
        private SoundEffectInstance soundEngine;
        private SoundEffect soundChomp;
        private SoundEffect soundEatScore;
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
                    { 5, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6 },
                    { 2, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 2 },
                    { 2, 0, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 8, 0, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 5, 1, 1, 1, 1, 1, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 2, 16, 0, 0, 0, 0, 16, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 0, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 5, 1, 1, 6, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0 },
                    { 2, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 1, 1, 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 2, 16, 0, 0, 0, 0, 16, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1, 1, 1, 1, 1, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 14, 14, 14, 14, 14, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2 },
                    { 2, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 0, 2 },
                    { 2, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 2 },
                    { 3, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 }
                });

            this.player = new Player(this.gameManager, new Vector2(60, 40),
                this.gameManager.ContentManager.Load<Texture2D>("Pacman"), PacManSX.CharacterWidth, PacManSX.CharacterHeight) { Direction = Direction.Right };

            this.blueGhost = new Ghost(this.gameManager, new Vector2(560, 40),
                this.gameManager.ContentManager.Load<Texture2D>("BlueGhost"), PacManSX.CharacterWidth, PacManSX.CharacterHeight) { Direction = Direction.Right };

            this.greenGhost = new Ghost(this.gameManager, new Vector2(560, 40),
                this.gameManager.ContentManager.Load<Texture2D>("BlueGhost"), PacManSX.CharacterWidth, PacManSX.CharacterHeight) { Direction = Direction.Right };
            this.greenGhost.Alive = false;

            this.yellowGhost = new Ghost(this.gameManager, new Vector2(560, 40),
                this.gameManager.ContentManager.Load<Texture2D>("BlueGhost"), PacManSX.CharacterWidth, PacManSX.CharacterHeight) { Direction = Direction.Right };
            this.yellowGhost.Alive = false;

            this.purpleGhost = new Ghost(this.gameManager, new Vector2(560, 40),
                this.gameManager.ContentManager.Load<Texture2D>("BlueGhost"), PacManSX.CharacterWidth, PacManSX.CharacterHeight) { Direction = Direction.Right };
            this.purpleGhost.Alive = false;
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
            this.greenGhost.Update(elapsedGameTime);
            this.yellowGhost.Update(elapsedGameTime);
            this.purpleGhost.Update(elapsedGameTime);
            
            // If the player is alive then handle the movement.
            if (this.player.Alive)
                PlayerMovement(elapsedGameTime);

            if (this.blueGhost.Alive)
                BlueGhostMovement(elapsedGameTime);

            PlayerGhostHitbox();
        }
        
        /// <summary>
        /// Allows the game to draw itself.
        /// </summary>
        /// <param name="elapsedGameTime">Elapsed time since the last draw.</param>
        public void Draw(TimeSpan elapsedGameTime)
        {
            // Clear the screen to black and draw the map and player.
            this.gameManager.SpriteBatch.GraphicsDevice.Clear(new Color(68, 68, 68));
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
        /// Method runs when the player has walked over a dot tile.
        /// </summary>
        private void PlayerHasEatenDot(Tile tile)
        {
            // Change the tile to either a turn or path tile.
            if (tile.TileContent == TileContent.DotTurn)
                tile.TileContent = TileContent.Turn;
            else if (tile.TileContent == TileContent.Dot)
                tile.TileContent = TileContent.Path;

            // Add the score you get from a dot.
            score += DotScore;

            // Turn on godMode.
            player.GodMode = true;

            //Play godmode music
            soundGodMode = gameManager.ContentManager.Load<SoundEffect>("god_mode");
            soundGodMode.Play();
            soundEngine = soundGodMode.CreateInstance();

            // If the godmodeOverTimer is not null then stop it.
            if (godmodeOverTimer != null)
                godmodeOverTimer.Stop();
            else
            {
                // If it's null then create it.
                godmodeOverTimer = new Timer(1000 * GodModeActiveInSeconds);
                godmodeOverTimer.AutoReset = false;
                godmodeOverTimer.Elapsed += godModeOver;
            }

            // Start the timer.
            godmodeOverTimer.Start();
        }

        /// <summary>
        /// Method runs when the player has walked over a ring tile.
        /// </summary>
        private void PlayerHasEatenRing(Tile tile)
        {
            // Change the tile to either a turn or path tile.
            if (tile.TileContent == TileContent.RingTurn)
                tile.TileContent = TileContent.Turn;
            else if (tile.TileContent == TileContent.Ring)
                tile.TileContent = TileContent.Path;

            // Add the score you get from a ring.
            score += RingScore;

            // Sound for walking over a coin/ring
            soundEatScore = gameManager.ContentManager.Load<SoundEffect>("coin");

            soundEatScore.Play();

        }

        /// <summary>
        /// Method runs when the godmode should be over.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void godModeOver(object sender, ElapsedEventArgs e)
        {
            // Turn of godMode.
            this.player.GodMode = false;
        }

        /// <summary>
        /// Method for handling the blue ghosts movement, temporary method.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        private void BlueGhostMovement(TimeSpan elapsedGameTime)
        {
            // Player coordinates for targeting
            Point playerCoords = PacManSX.ConvertPositionToCell(this.player.Center);

            Tile targetTile = null;

            // behaviour of current blue ghost will actually be red ghost
            Direction direction = this.blueGhost.Direction;
            Vector2 motion = this.blueGhost.Motion;

            // Converts the center of the ghost to the ghost's tile coordinates.
            Point ghostCoords = PacManSX.ConvertPositionToCell(this.blueGhost.Center);
            Tile ghostTile = tileMap.GetTile(ghostCoords); // Get the tile the ghost is located at.

            // Check if the tile is a turn or path tile.
            if (ghostTile.TileContent == TileContent.Turn || ghostTile.TileContent == TileContent.Path
                || ghostTile.TileContent >= TileContent.Ring && ghostTile.TileContent <= TileContent.DotTurn)
            {
                // Convert the cell to a position.
                Vector2 ghostTilePosition = PacManSX.ConvertCellToPosition(ghostCoords);

                // Check if the ghost is right ontop of the tile.
                if (ghostTilePosition == this.blueGhost.Position)
                {
                    // Check if the ghost can move in that direction
                    /*
                     * 
                     * GHOST ERROR! LOLOLOL Direction och blueGhost.Direction kommer ALLTID vara samma så om denna är false kmr nästa IF-sats också vara false.
                     * direction måste sättas till det hållet som spöket försöker svänga åt nästa gång han kommer till en Turn path.
                     * 
                     * EVENTUELLT går det att fixa så att en  funktion körs så fort en spöke har en möjlighet att svänga, i denna funktionen kan du sedan beräkna åt vilket håll spöket ska röra sig mot
                     * kanske sparar prestanda? 
                     * 
                     * Så det finns två sätt: Bestäm åt vilket håll spöket ska svänga åt här med hjälp av
                     * CanCharacterMove(ghostCoords, "DET HÅLL DU VILL TESTA MOT", out motion) - Detta ger false om du inte kan flytta och om du kan flytta ger den true och motion sätts till 
                     * det värde som spöket ska röra sig i.
                     * 
                     * Det andra sättet är att en funktion körs så fort ett av spökena går på en turn platta, som inparameter till funktionen finns vilket spöke som kan svänga.
                     * Här kan du nu räkna ut om spöket ska svänga eller inte.
                     * 
                     * 
                     * */

                    #region Ghost pathfinding for red behaviour
                    if(ghostTile.TileContent == TileContent.Turn || ghostTile.TileContent == TileContent.Turn || ghostTile.TileContent == TileContent.DotTurn)
                    {
                        int xDelta = Math.Abs((ghostCoords.X - playerCoords.X));
                        int yDelta = Math.Abs((ghostCoords.Y - playerCoords.Y));

                        if (ghostCoords.X <= playerCoords.X && ghostCoords.Y >= playerCoords.Y)
                        {
                            if (xDelta > yDelta)
                            {
                                direction = Direction.Right;

                                if(player.GodMode)
                                    direction = reverseMovement(direction);

                                if(!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Up;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }

                            else
                            {
                                direction = Direction.Up;

                                if (player.GodMode)
                                    direction = reverseMovement(direction);

                                if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Right;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }
                        }
                        
                        if (ghostCoords.X <= playerCoords.X && ghostCoords.Y <= playerCoords.Y)
                        {
                            if (xDelta > yDelta)
                            {
                                direction = Direction.Right;

                                if (player.GodMode)
                                    direction = reverseMovement(direction);

                                if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Down;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }

                            else
                            {
                                direction = Direction.Down;

                                if (player.GodMode)
                                    direction = reverseMovement(direction);

                                if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Right;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }
                        }

                        if (ghostCoords.X >= playerCoords.X && ghostCoords.Y <= playerCoords.Y)
                        {
                            if (xDelta > yDelta)
                            {
                                direction = Direction.Left;

                                if (player.GodMode)
                                    direction = reverseMovement(direction);

                                if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Down;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }

                            else
                            {
                                direction = Direction.Down;

                                if (player.GodMode)
                                    direction = reverseMovement(direction);

                                if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Left;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }
                        }

                        if(ghostCoords.X >= playerCoords.X && ghostCoords.Y >= playerCoords.Y)
                        {
                            if (xDelta > yDelta)
                            {
                                direction = Direction.Left;

                                if (player.GodMode)
                                    direction = reverseMovement(direction);

                                if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Up;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }

                            else
                            {
                                direction = Direction.Up;

                                if (player.GodMode)
                                    direction = reverseMovement(direction);

                                if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                {
                                    direction = Direction.Left;

                                    if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                    {
                                        direction = reverseMovement(direction);

                                        if (!CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                                        {
                                            direction = reverseMovement(blueGhost.Direction);
                                            CanCharacterMove(ghostCoords, direction, out motion, out targetTile);
                                        }

                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    if (CanCharacterMove(ghostCoords, direction, out motion, out targetTile))
                    {
                        this.blueGhost.Motion = motion;
                        this.blueGhost.Direction = direction;
                    }
                    else
                    {
                        // If the ghost can't move in that direction then check if the player can move in the old direction.
                        if (CanCharacterMove(ghostCoords, this.blueGhost.Direction, out motion, out targetTile))
                        {
                            this.blueGhost.Motion = motion;
                        }
                    }
                    // If the ghost can't move in the old direction or the new then just stand still.
                }
                else
                {
                    // If the ghost is not right ontop of the tile then continue to move.
                    if (this.blueGhost.Direction == Direction.Up)
                        motion.Y = -1;
                    else if (this.blueGhost.Direction == Direction.Down)
                        motion.Y = 1;
                    else if (this.blueGhost.Direction == Direction.Left)
                        motion.X = -1;
                    else if (this.blueGhost.Direction == Direction.Right)
                        motion.X = 1;
                }
            }

            // Check if we should move.
            if (motion != Vector2.Zero)
            {
                // Normalize the motion vector and move the ghost.
                motion.Normalize();

                this.blueGhost.Position.X += (float)Math.Round((motion * this.blueGhost.Speed * (float)elapsedGameTime.TotalSeconds).X);
                this.blueGhost.Position.Y += (float)Math.Round((motion * this.blueGhost.Speed * (float)elapsedGameTime.TotalSeconds).Y);
            }
        }

        private Direction reverseMovement(Direction direction)
        {
            if (direction == Direction.Up)
                return Direction.Down;
            
            if(direction == Direction.Down)
                return Direction.Up;
            
            if(direction == Direction.Left)
                return Direction.Right;

            if(direction == Direction.Right)
                return Direction.Left;
            else
            {
                return Direction.None;
            }
        }

        /// <summary>
        /// Method for handling movement and input.
        /// </summary>
        private void PlayerMovement(TimeSpan elapsedGameTime)
        {
            // If the player goes outside the playfield, teleport the player to the other side.
            if (this.player.Position.X >= 772)
                this.player.Position.X = 0;
            else if (this.player.Position.X <= 0)
                this.player.Position.X = 772;

            if (this.player.Position.Y >= 452)
                this.player.Position.Y = PacManSX.TitleHeight;
            else if (this.player.Position.Y <= 0 + PacManSX.TitleHeight)
                this.player.Position.Y = 452;

            KeyboardState keyboardState = Keyboard.GetState();

            Direction direction = this.player.NextDirection;
            Vector2 motion = this.player.Motion;

            // Converts the center of the player to the players tile coordinates.
            Point playerCoords = PacManSX.ConvertPositionToCell(this.player.Center);
            Tile playerTile = tileMap.GetTile(playerCoords); // Get the tile the player is located at.
            Tile targetTile = null;

            // Check for input, should we change direction?
            if (keyboardState.IsKeyDown(Keys.W))
                direction = Direction.Up;
            else if (keyboardState.IsKeyDown(Keys.S))
                direction = Direction.Down;
            else if (keyboardState.IsKeyDown(Keys.A))
                direction = Direction.Left;
            else if (keyboardState.IsKeyDown(Keys.D))
                direction = Direction.Right;
            
            // Check if the tile is a turn or path tile.
            if (playerTile.TileContent == TileContent.Turn || playerTile.TileContent == TileContent.Path
                || playerTile.TileContent >= TileContent.Ring && playerTile.TileContent <= TileContent.DotTurn)
            {
                // Convert the cell to a position.
                Vector2 playerTilePosition = PacManSX.ConvertCellToPosition(playerCoords);

                // Check if the player is right ontop of the tile.
                if (playerTilePosition == this.player.Position)
                {
                    // Check if the player can move in that direction
                    if (CanCharacterMove(playerCoords, direction, out motion, out targetTile))
                    {
                        this.player.Motion = motion;
                        this.player.Direction = direction;
                        this.player.NextDirection = direction;
                    }
                    else
                    {
                        // If the player can't move in that direction then check if the player can move in the old direction.
                        if (CanCharacterMove(playerCoords, this.player.Direction, out motion, out targetTile))
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
                    {
                        // Did we change direction to the opposite direction, then move in the opposite direction.
                        if (direction == Direction.Down)
                        {
                            motion.Y = 1;
                            this.player.Direction = direction;
                        }
                        else
                            motion.Y = -1;
                    }
                    else if (this.player.Direction == Direction.Down)
                    {
                        // Did we change direction to the opposite direction, then move in the opposite direction.
                        if (direction == Direction.Up)
                        {
                            motion.Y = -1;
                            this.player.Direction = direction;
                        }
                        else
                            motion.Y = 1;
                    }
                    else if (this.player.Direction == Direction.Left)
                    {
                        // Did we change direction to the opposite direction, then move in the opposite direction.
                        if (direction == Direction.Right)
                        {
                            motion.X = 1;
                            this.player.Direction = direction;
                        }
                        else
                            motion.X = -1;
                    }
                    else if (this.player.Direction == Direction.Right)
                    {
                        // Did we change direction to the opposite direction, then move in the opposite direction.
                        if (direction == Direction.Left)
                        {
                            motion.X = -1;
                            this.player.Direction = direction;
                        }
                        else
                            motion.X = 1;
                    }

                    // Update next direction.
                    this.player.NextDirection = direction;
                }

                // If the player walks over a ring or dot, run the PlayerHasEatenRing or PlayerHasEatenDot method.
                if (targetTile != null)
                {
                    if (targetTile.TileContent == TileContent.Ring || targetTile.TileContent == TileContent.RingTurn)
                        PlayerHasEatenRing(targetTile);
                    else if (targetTile.TileContent == TileContent.Dot || targetTile.TileContent == TileContent.DotTurn)
                        PlayerHasEatenDot(targetTile);
                }
            }

            // Check if we should move.
            if (motion != Vector2.Zero)
            {
                // Normalize the motion vector and move the player.
                motion.Normalize();

                this.player.Position.X += (float)Math.Round((motion * this.player.Speed * (float)elapsedGameTime.TotalSeconds).X);
                this.player.Position.Y += (float)Math.Round((motion * this.player.Speed * (float)elapsedGameTime.TotalSeconds).Y);
            }
        }

        /// <summary>
        /// Helper for checking if someone can move in a given direction.
        /// </summary>
        /// <param name="coords">The coordinates of the tile.</param>
        /// <param name="direction">The direction to be checked.</param>
        /// <param name="motion">If the player couldn't move then it's set to (0, 0); otherwise it's set to the direction.</param>
        /// <returns>True if the player can move in that direction; otherwise false.</returns>
        private bool CanCharacterMove(Point coords, Direction direction, out Vector2 motion, out Tile targetTile)
        {
            motion = Vector2.Zero;
            targetTile = null;
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
            targetTile = this.tileMap.GetTile(target);

            if (targetTile == null)
            {
                motion = Vector2.Zero;
                targetTile = null;
                return false;
            }

            // If the player can move then return true.
            if (targetTile.TileContent == TileContent.Path || targetTile.TileContent == TileContent.Turn
                || targetTile.TileContent >= TileContent.Ring && targetTile.TileContent <= TileContent.DotTurn)
                return true;

            // else set motion to (0, 0) and return false.
            motion = Vector2.Zero;
            targetTile = null;
            return false;
        }

        /// <summary>
        /// Method handling the collision detection between the player and a ghost.
        /// </summary>
        private void PlayerGhostHitbox()
        {
            if (!this.player.Alive)
                return;

            // Check if the player is in god mode.
            if (this.player.GodMode)
            {
                // If he is in godmode then he should be able to eat the ghost.
                // If the player and a ghost collides, kill the ghost and recieve score.
                if (this.blueGhost.Alive && this.blueGhost.Bounds.Intersects(this.player.Bounds))
                {
                    this.blueGhost.Alive = false;
                    this.score += PacManSX.GhostScore;
                }
                else if (this.greenGhost.Alive && this.greenGhost.Bounds.Intersects(this.player.Bounds))
                {
                    this.greenGhost.Alive = false;
                    this.score += PacManSX.GhostScore;
                }
                else if (this.yellowGhost.Alive && this.yellowGhost.Bounds.Intersects(this.player.Bounds))
                {
                    this.yellowGhost.Alive = false;
                    this.score += PacManSX.GhostScore;
                }
                else if (this.purpleGhost.Alive && this.purpleGhost.Bounds.Intersects(this.player.Bounds))
                {
                    this.purpleGhost.Alive = false;
                    this.score += PacManSX.GhostScore;
                }
            }
            else
            {
                // If the player and a ghost collides, remove a life from the player and kill it.
                if (this.blueGhost.Alive && this.blueGhost.Bounds.Intersects(this.player.Bounds) ||
                    this.greenGhost.Alive && this.greenGhost.Bounds.Intersects(this.player.Bounds) ||
                    this.yellowGhost.Alive && this.yellowGhost.Bounds.Intersects(this.player.Bounds) ||
                    this.purpleGhost.Alive && this.purpleGhost.Bounds.Intersects(this.player.Bounds))
                {
                    this.lives--;
                    this.player.Alive = false;

                    soundChomp = gameManager.ContentManager.Load<SoundEffect>("chomp");
                    soundChomp.Play();
                }
            }
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
            return new Vector2(cell.X * PacManSX.TileWidth, cell.Y * PacManSX.TileHeight + PacManSX.TileHeight);
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