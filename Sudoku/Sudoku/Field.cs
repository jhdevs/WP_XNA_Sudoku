using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sudoku
{
    public class Field
    {
        public string txt;
        public int i;
        public Vector2 size;
        public Vector2 position;
        public Rectangle bounds;
        public bool preset;
        float shiftY = 3.0f;
        
        public Field(int num, Rectangle parent, float offset)
        {
            i = 0;
            txt = "";
            preset = false;
            int x = (int)num % 9;
            int y = (int)num / 9;
            bounds = new Rectangle((int)(x * offset + parent.X), (int)(y * offset + parent.Y), (int)offset, (int)offset);
        }
        public void SetNum(int num, SpriteFont fnt, bool pre)
        {
            preset = pre;
            SetNum(num, fnt);
        }
        public void SetNum(int num, SpriteFont fnt)
        {
            i = num;
            if (i == 0)
            {
                txt = "";
            }
            else
            {
                txt = num.ToString();
                size = fnt.MeasureString(txt);
                position = new Vector2(bounds.Left + (bounds.Width - size.X) / 2, bounds.Top + (bounds.Height - size.Y) / 2 + shiftY);
            }
        }
    }
}
