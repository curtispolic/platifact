using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platifact;

public class InventoryItem
{
    public Texture2D texture;
    public StaticPlatifactObject item;
    public int amount;

    public InventoryItem(StaticPlatifactObject i)
    {
        item = i;
        amount = 0;
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(
            this.texture,
            pos,
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
