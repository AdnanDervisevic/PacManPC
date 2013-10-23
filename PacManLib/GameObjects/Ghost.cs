#region File Description
    //////////////////////////////////////////////////////////////////////////
   // Ghost                                                                //
  //                                                                      //
 // Copyright (C) Veritas. All Rights reserved.                          //
//////////////////////////////////////////////////////////////////////////
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion End of Using Statements

namespace PacManLib.GameObjects
{
    public sealed class Ghost : Character
    {
        #region Constructors

        /// <summary>
        /// Constructs a new player.
        /// </summary>
        /// <param name="gameManager">The game manager.</param>
        /// <param name="origin">The origin of the player.</param>
        /// <param name="texture">The players spritesheet.</param>
        /// <param name="frameWidth">The width of a single frame.</param>
        /// <param name="frameHeight">The height of a single frame.</param>
        public Ghost(GameManager gameManager, Vector2 position, Texture2D texture, int frameWidth, int frameHeight)
            : base(gameManager, position, texture, frameWidth, frameHeight)
        {
            this.Speed = 140;
        }

        #endregion
    }
}