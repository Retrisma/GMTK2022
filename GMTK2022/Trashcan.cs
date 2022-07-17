using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GMTK2022
{
    public class Trashcan : Sprite
    {
        public Trashcan()
        {
            Texture = Game1._spriteContent["TrashcanClosed"];
            Position = new Vector2(900, 20);
        }

        public override void Update(GameTime gt)
        {
            
        }
    }
}
