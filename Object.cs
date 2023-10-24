using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace platifact;

public class GameObject
{
    public Texture2D texture;
    Vector2 position;
    Vector2 desiredPosition;
    Vector2 speed;

    public Rectangle CurrentBounds()
    {
        return new Rectangle(((int) this.position.X),
                             ((int) this.position.Y),
                             ((int) this.position.X + (int) this.texture.Width),
                             ((int) this.position.Y + (int) this.texture.Height));
    }

    public Rectangle DesiredBounds()
    {
        return new Rectangle(((int)this.desiredPosition.X),
                             ((int)this.desiredPosition.Y),
                             ((int)this.desiredPosition.X + (int)this.texture.Width),
                             ((int)this.desiredPosition.Y + (int)this.texture.Height));
    }

}
