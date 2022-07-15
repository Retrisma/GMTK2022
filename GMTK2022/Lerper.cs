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
    internal class Lerper
    {

        Vector2 StartPos { get; set; }
        Vector2 EndPos { get; set; }
        public float Age = 0;
        public float EndAge = 60;

        public Lerper(Vector2 startPos, Vector2 endPos)
        {
            StartPos = startPos;
            EndPos = endPos;
        }

        public Vector2 Lerp()
        {
            Age++;

            Vector2 x = StartPos + ((EndPos - StartPos) * (float)(Age / EndAge));

            x += new Vector2(0, 0.01f * Age * (Age - 60));

            return x;
        }
    }
}
