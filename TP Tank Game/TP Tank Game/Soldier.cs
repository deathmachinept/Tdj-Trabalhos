using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Tank_Game
{
    class Soldier : Animated_Sprite
    {
        public Soldier(ContentManager content)
            : base(content, "Prone Soldier", 1, 4)
        {
            this.Scl(0.2f);
            this.EnableCollisions();
        }

    }
}
