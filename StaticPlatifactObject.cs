using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platifact;

public class StaticPlatifactObject
{
    public Texture2D texture;
    public Vector2 position;
    public bool isNothing;

    public Rectangle CurrentBounds()
    {
        return new Rectangle(((int) this.position.X),
                             ((int) this.position.Y),
                             ((int) this.texture.Width),
                             ((int) this.texture.Height));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            this.texture,
            this.position,
            null,
            Color.White,
            0f,
            new Vector2(0, 0),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

}
