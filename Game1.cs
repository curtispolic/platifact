using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class Game1 : Game
{
    GameObject player = new GameObject();
    GameObject[,] squares;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Constructor
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 640;
        _graphics.PreferredBackBufferHeight = 480;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    // Run once on initialization of the game
    protected override void Initialize()
    {
        // Set the players starting parameteres
        player.position = new Vector2(_graphics.PreferredBackBufferWidth / 2, 0);
        player.jumpAccel = new Vector2(0, 0);

        // Set the grid for objects
        squares = new GameObject[(_graphics.PreferredBackBufferHeight / 64), (_graphics.PreferredBackBufferWidth / 64)];

        // Create the platform for the player to stand on
        for (int i = 0; i < squares.GetLength(0); i++)
        {
            for (int j = 0; j < squares.GetLength(1); j++)
            {
                squares[i, j] = new GameObject();
                squares[i, j].position = new Vector2(j * 64, i * 64);
                if (i == 5)
                {
                    squares[i, j].isNothing = false;
                }
                else
                {
                    squares[i, j].isNothing = true;
                }
            }     
        }

        base.Initialize();
    }

    // Does what it says on the box, loads all the content
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        player.texture = Content.Load<Texture2D>("player");
        foreach (GameObject block in squares)
        {
            if (!block.isNothing)
            {
                block.texture = Content.Load<Texture2D>("block");
            }
        }
    }

    protected override void Update(GameTime gameTime)
    {
        // Quit on escape or gamepad back
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        // Set gravity and player speed for the frame
        Vector2 gravity = new Vector2(0f, (float)(600 * gameTime.ElapsedGameTime.TotalSeconds));
        player.speed = new Vector2(0, 0);

        // Get keyboard buttons being pressed
        var kstate = Keyboard.GetState();
        var mstate = Mouse.GetState();

        // Movement
        if (kstate.IsKeyDown(Keys.W) && player.jumpAccel.Y == 0)
            player.jumpAccel.Y = -25;

        if (kstate.IsKeyDown(Keys.A))
            player.speed -= new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);

        if (kstate.IsKeyDown(Keys.D))
            player.speed += new Vector2((float)(300 * gameTime.ElapsedGameTime.TotalSeconds), 0);
        
        // Out of bounds restrictions
        if (player.position.X + player.texture.Width > _graphics.PreferredBackBufferWidth)
            player.position.X = _graphics.PreferredBackBufferWidth - player.texture.Width;
        
        else if (player.position.X < 0)
            player.position.X = 0;
        
        if (player.position.Y + player.texture.Height > _graphics.PreferredBackBufferHeight)
            player.position.Y = _graphics.PreferredBackBufferHeight - player.texture.Height;
       
        else if (player.position.Y < 0)
            player.position.Y = 0;

        // Adjust jump acceleration for "jump feel"
        if (player.jumpAccel.Y < 0)
            player.jumpAccel.Y += (float)(40 * gameTime.ElapsedGameTime.TotalSeconds);

        if (player.jumpAccel.Y > 0)
            player.jumpAccel.Y = 0;

        // Gravity applies only if not clipping in order for accurate intersection detection
        if (!player.isClipping)
        {
            player.speed += gravity;
        }

        player.speed += player.jumpAccel;
        player.desiredPosition = player.position + player.speed;

        player.isClipping = false;

        // Detecting collision with blocks
        foreach (GameObject block in squares)
        {
            if (!block.isNothing)
            {
                if (player.DesiredBounds().Intersects(block.CurrentBounds()))
                {
                    // Intersect of the two objects
                    Rectangle intersect = Rectangle.Intersect(player.DesiredBounds(), block.CurrentBounds());

                    // Check which direction coming from and the bigger clipping
                    // Player on the right
                    if (intersect.Width < intersect.Height && player.position.X > block.position.X)
                    {
                        player.desiredPosition.X += intersect.Width;
                    }

                    // Player on the left
                    if (intersect.Width < intersect.Height && player.position.X < block.position.X)
                    {
                        player.desiredPosition.X -= intersect.Width;
                    }

                    // Player on the bottom
                    if (intersect.Height < intersect.Width && player.position.Y > block.position.Y)
                    {
                        player.desiredPosition.Y += intersect.Height;
                        // Also have to reset jumpAccel to stop hangtime, but only if it's outdoing gravity
                        if (player.jumpAccel.Y < gravity.Y * -1)
                            player.jumpAccel = gravity * -1;
                    }

                    // Player on top
                    if (intersect.Height < intersect.Width && player.position.Y < block.position.Y)
                    {
                        // Only count clipping on the top to prevent wall clinging
                        player.isClipping = true;

                        // Stop JumpAccel to stop jitters
                        player.jumpAccel.Y = 0;

                        player.desiredPosition.Y -= intersect.Height;
                    }
                    
                }
            }
        }

        if (mstate.LeftButton == ButtonState.Pressed)
        {
            int x = mstate.X / 64;
            int y = mstate.Y / 64;
            GameObject clicked = squares[y, x];
            if (clicked.isNothing)
            {
                clicked.isNothing = false;
                clicked.texture = Content.Load<Texture2D>("ball");
            }
        }
        
        player.position = player.desiredPosition;
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Draw background colour
        GraphicsDevice.Clear(Color.LightPink);

        _spriteBatch.Begin();

        // Draw player
        _spriteBatch.Draw(
            player.texture,
            player.position,
            null,
            Color.White,
            0f,
            new Vector2(0, 0),
            Vector2.One,
            SpriteEffects.None,
            0f
        );

        // Draw blocks
        foreach (GameObject block in squares)
        {
            if (!block.isNothing)
            {
                _spriteBatch.Draw(
                    block.texture,
                    block.position,
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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

