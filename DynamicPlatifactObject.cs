using Microsoft.Xna.Framework;

namespace platifact;

public class DynamicPlatifactObject : StaticPlatifactObject
{
    public Vector2 desiredPosition;
    public Vector2 speed;

    public Rectangle DesiredBounds()
    {
        return new Rectangle(((int)this.desiredPosition.X),
                             ((int)this.desiredPosition.Y),
                             ((int)this.texture.Width),
                             ((int)this.texture.Height));
    }

}
