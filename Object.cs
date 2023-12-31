﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace platifact;

public class GameObject
{
    public Texture2D texture;
    public Vector2 position;
    public Vector2 desiredPosition;
    public Vector2 speed;
    public Vector2 jumpAccel;
    public bool isClipping;
    public bool isNothing;

    public Rectangle CurrentBounds()
    {
        return new Rectangle(((int) this.position.X),
                             ((int) this.position.Y),
                             ((int) this.texture.Width),
                             ((int) this.texture.Height));
    }

    public Rectangle DesiredBounds()
    {
        return new Rectangle(((int)this.desiredPosition.X),
                             ((int)this.desiredPosition.Y),
                             ((int)this.texture.Width),
                             ((int)this.texture.Height));
    }

}
