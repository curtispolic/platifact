using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platifact;

public class Miner : StaticPlatifactObject
{
    public int containedOreAmount;
    public string oreMining;
    public bool isRunning;
    public StaticPlatifactObject belowObject;

    // Constructor
    public Miner(StaticPlatifactObject belowObj)
    {
        isRunning = false;
        containedOreAmount = 0;
        oreMining = CheckMining();
        isNothing = false;
        isBlocking = false;
        belowObject = belowObj;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Draw new sprite based on running or not
        switch(isRunning)
        {
            case true:
                spriteBatch.Draw(
                    this.texture,
                    this.position,
                    new Rectangle(64, 0, 64, 64),
                    Color.White,
                    0f,
                    new Vector2(0, 0),
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                );
                break;

            case false:
                spriteBatch.Draw(
                    this.texture,
                    this.position,
                    new Rectangle(0, 0, 64, 64),
                    Color.White,
                    0f,
                    new Vector2(0, 0),
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                );
                break;
        }
    }

    public void Update(GameTime gameTime, StaticPlatifactObject belowObj)
    {
        // Update whatever is below
        if (belowObject != belowObj)
        {
            belowObject = belowObj;
        }

        // Only run if plaform is below
        if (belowObject is Platform)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public string CheckMining()
    {
        return "stone";
    }

}
