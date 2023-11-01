using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platifact;

public class Miner : StaticPlatifactObject
{
    public bool isRunning;
    public StaticPlatifactObject belowObject;
    private int mineTimer;
    public InventoryItem containedItem;

    // Constructor
    public Miner(StaticPlatifactObject belowObj)
    {
        isRunning = false;
        containedItem = new InventoryItem(CheckMining());
        isNothing = false;
        isBlocking = false;
        belowObject = belowObj;
        mineTimer = 0;
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
        if (belowObject is Ore)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Only update mineTimer if mining
        if (isRunning)
        {
            mineTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (mineTimer > 2000 && ((Ore)belowObject).oreAmount > 0)
            {
                mineTimer -= 2000;
                containedItem.amount += 1;
                ((Ore)belowObject).oreAmount--;
            }

            // Destroy below block when it's out of ore
            if (((Ore)belowObject).oreAmount == 0)
            {
                // TODO: manager class to handle changing of blocks from any class
                belowObject.isNothing = true;
                belowObject.isBlocking = false;
                isRunning = false;
            }
        }
    }

    public Ore CheckMining()
    {
        return new Ore();
    }

}
