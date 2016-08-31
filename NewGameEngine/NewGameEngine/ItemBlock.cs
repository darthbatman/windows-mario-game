using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Timers;

namespace NewGameEngine
{
    class ItemBlock
    {
        public Vector2 position;
        public Rectangle boundingRectangle;

        public bool isUp = false;

         public ItemBlock(int x, int y)
        {
            position = new Vector2(x, y);
            boundingRectangle = new Rectangle(x, y, 20, 20);
        }

        public void wasStruck(){
            if (!isUp)
            {
                position.Y -= 10;
                boundingRectangle.Y -= 10;
                isUp = true;
            }
        }

    }
}
