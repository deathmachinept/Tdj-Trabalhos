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
            : base(content, "soldado", 1, 3)
        {
            this.Scl(0.2f);
            this.EnableCollisions();
        }

    }
}
