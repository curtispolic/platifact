using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class UIElement
{
    public Vector2 position;
    public Texture2D texture;


    // Constructor
    public UIElement()
    {
        
    }

    public virtual void Draw(SpriteBatch spriteBatch)
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
