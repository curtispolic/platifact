using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class Player : DynamicPlatifactObject
{
    public Vector2 jumpAccel;
    public Vector2 gravity;
    public bool isClipping;

    public void HandleMovement(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        // Set gravity and player speed for the frame
        Vector2 gravity = new Vector2(0f, (float)(600 * gameTime.ElapsedGameTime.TotalSeconds));
        this.speed = new Vector2(0, 0);

        // Get keyboard buttons being pressed
        var kstate = Keyboard.GetState();

        // Movement
        if (kstate.IsKeyDown(Keys.W) && this.jumpAccel.Y == 0)
            this.jumpAccel.Y = -25;

        if (kstate.IsKeyDown(Keys.A))
            this.speed -= new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);

        if (kstate.IsKeyDown(Keys.D))
            this.speed += new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);

        // Out of bounds restrictions
        if (this.position.X + this.texture.Width > graphics.PreferredBackBufferWidth)
            this.position.X = graphics.PreferredBackBufferWidth - this.texture.Width;

        else if (this.position.X < 0)
            this.position.X = 0;

        if (this.position.Y + this.texture.Height > graphics.PreferredBackBufferHeight)
            this.position.Y = graphics.PreferredBackBufferHeight - this.texture.Height;

        else if (this.position.Y < 0)
            this.position.Y = 0;

        // Adjust jump acceleration for "jump feel"
        if (this.jumpAccel.Y < 0)
            this.jumpAccel.Y += (float)(40 * gameTime.ElapsedGameTime.TotalSeconds);

        if (this.jumpAccel.Y > 0)
            this.jumpAccel.Y = 0;

        // Gravity applies only if not clipping in order for accurate intersection detection
        if (!this.isClipping)
        {
            this.speed += gravity;
        }

        this.speed += this.jumpAccel;
        this.desiredPosition = this.position + this.speed;

        this.isClipping = false;
    }
}
