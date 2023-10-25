using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class Game1 : Game
{
    GameObject ball = new GameObject();
    GameObject block = new GameObject();

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Constructor
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        block.position = new Vector2(300, 300);
        ball.position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        ball.jumpAccel = new Vector2(0, 0);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ball.texture = Content.Load<Texture2D>("ball");
        block.texture = Content.Load<Texture2D>("block");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Vector2 gravity = new Vector2(0f, (float)(600 * gameTime.ElapsedGameTime.TotalSeconds));
        ball.speed = new Vector2(0, 0);

        // Get keyboard buttons being pressed
        var kstate = Keyboard.GetState();

        // Movement
        if (kstate.IsKeyDown(Keys.W) && ball.jumpAccel.Y == 0)
            ball.jumpAccel.Y = -25;

        if (kstate.IsKeyDown(Keys.A))
            ball.speed -= new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);

        if (kstate.IsKeyDown(Keys.D))
            ball.speed += new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);
        
        // Out of bounds restrictions
        if (ball.position.X > _graphics.PreferredBackBufferWidth - ball.texture.Width / 2)
            ball.position.X = _graphics.PreferredBackBufferWidth - ball.texture.Width / 2;
        
        else if (ball.position.X < ball.texture.Width / 2)
            ball.position.X = ball.texture.Width / 2;
        
        if (ball.position.Y > _graphics.PreferredBackBufferHeight - ball.texture.Height / 2)
            ball.position.Y = _graphics.PreferredBackBufferHeight - ball.texture.Height / 2;
       
        else if (ball.position.Y < ball.texture.Height / 2)
            ball.position.Y = ball.texture.Height / 2;

        // Adjust jump acceleration for "jump feel"
        if (ball.jumpAccel.Y < 0)
            ball.jumpAccel.Y += (float)(40 * gameTime.ElapsedGameTime.TotalSeconds);

        if (ball.jumpAccel.Y > 0)
            ball.jumpAccel.Y = 0;

        ball.speed += gravity;
        ball.speed += ball.jumpAccel;
        ball.desiredPosition = ball.position + ball.speed;

        if (ball.DesiredBounds().Intersects(block.CurrentBounds()))
        {
            Rectangle intersect = Rectangle.Intersect(ball.DesiredBounds(), block.CurrentBounds());
            if (intersect.Height > intersect.Width)
            {
                ball.desiredPosition.X = ball.position.X;
            }
            else
            {
                ball.desiredPosition.Y = ball.position.Y;
            }

        }

        ball.position = ball.desiredPosition;
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightPink);

        _spriteBatch.Begin();
        _spriteBatch.Draw(
            ball.texture,
            ball.position,
            null,
            Color.White,
            0f,
            new Vector2(ball.texture.Width / 2, ball.texture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.Draw(
            block.texture,
            block.position,
            null,
            Color.White,
            0f,
            new Vector2(block.texture.Width / 2, block.texture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

