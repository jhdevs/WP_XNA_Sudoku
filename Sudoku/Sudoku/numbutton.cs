using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sudoku
{
    public class NumButton
    {
        public String i;
        public Vector2 size;
        public Vector2 position;
        public Rectangle bounds;

        /// <summary>
        /// Set text, size, position, and bounds of button.
        /// </summary>
        public NumButton(int j, SpriteFont fnt, Vector2 pos, float offset) 
        {
            i = j.ToString();
            if (j == 0) i = "[]";
            size = fnt.MeasureString(i);
            bounds = new Rectangle((int)pos.X, (int)pos.Y, (int)offset, (int)size.Y);
            position = new Vector2(pos.X + (offset-size.X)/2,pos.Y);
        }
    }
}
