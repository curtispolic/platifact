using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class Player : DynamicPlatifactObject
{
    public Vector2 jumpAccel;
    public Vector2 gravity;
    public bool isClipping;
    public string objectHeld;


    // Constructor
    public Player()
    {
        objectHeld = null;
        jumpAccel = new Vector2(0, 0);
    }

    // Method to handle keyboard movement
    public void HandleMovement(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        // Set gravity player speed for the frame
        gravity = new Vector2(0f, (float)(600 * gameTime.ElapsedGameTime.TotalSeconds));
        speed = new Vector2(0, 0);

        // Get keyboard buttons being pressed
        var kstate = Keyboard.GetState();

        // Movement
        if (kstate.IsKeyDown(Keys.W) && jumpAccel.Y == 0)
            jumpAccel.Y = -25;

        if (kstate.IsKeyDown(Keys.A))
            speed -= new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);

        if (kstate.IsKeyDown(Keys.D))
            speed += new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);

        // Out of bounds restrictions
        if (position.X + texture.Width > graphics.PreferredBackBufferWidth)
            position.X = graphics.PreferredBackBufferWidth - texture.Width;

        else if (position.X < 0)
            position.X = 0;

        if (position.Y + texture.Height > graphics.PreferredBackBufferHeight)
            position.Y = graphics.PreferredBackBufferHeight - texture.Height;

        else if (position.Y < 0)
            position.Y = 0;

        // Adjust jump acceleration for "jump feel"
        if (jumpAccel.Y < 0)
            jumpAccel.Y += (float)(40 * gameTime.ElapsedGameTime.TotalSeconds);

        if (jumpAccel.Y > 0)
            jumpAccel.Y = 0;

        // Gravity applies only if not clipping in order for accurate intersection detection
        if (!isClipping)
        {
            speed += gravity;
        }

        speed += jumpAccel;
        desiredPosition = position + speed;

        isClipping = false;
    }
}
