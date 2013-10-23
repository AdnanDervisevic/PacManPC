#region File Description
    //////////////////////////////////////////////////////////////////////////
   // GamePage                                                             //
  //                                                                      //
 // Copyright (C) Veritas. All Rights reserved.                          //
//////////////////////////////////////////////////////////////////////////
#endregion

#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PacManLib;
#endregion End of Using Statements

namespace PacMan
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GamePage : Game
    {
        private PacManSX pacManSX = null;
        private GraphicsDeviceManager graphics = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GamePage()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            this.Content.RootDirectory = "Content";
        }

        #region Methods

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.pacManSX = new PacManSX(new GameManager(
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight,
                new SpriteBatch(this.GraphicsDevice), this.Content));
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            this.pacManSX.Update(gameTime.ElapsedGameTime);
            base.Update(gameTime);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.pacManSX.Draw(gameTime.ElapsedGameTime);
            base.Draw(gameTime);
        }

        #endregion

        #region Entry Point

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GamePage game = new GamePage())
            {
                game.Run();
            }
        }

        #endregion
    }
}