using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class PlatifactGame : Game
{
    Player player = new Player();
    StaticPlatifactObject[,] squares;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Constructor
    public PlatifactGame()
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
        
        // Set the grid for objects
        squares = new StaticPlatifactObject[(_graphics.PreferredBackBufferHeight / 64 + 1), (_graphics.PreferredBackBufferWidth / 64 + 1)];

        // Create the platform for the player to stand on
        for (int i = 0; i < squares.GetLength(0); i++)
        {
            for (int j = 0; j < squares.GetLength(1); j++)
            {
                if (i == 5)
                {
                    squares[i, j] = new Platform();
                }
                else
                {
                    squares[i, j] = new StaticPlatifactObject();
                }
                squares[i, j].position = new Vector2(j * 64, i * 64);
            }     
        }

        base.Initialize();
    }

    // Does what it says on the box, loads all the content
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        player.texture = Content.Load<Texture2D>("player");
        foreach (StaticPlatifactObject block in squares)
        {
            if (block is Platform)
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

        // Get the state of the mouse
        var mstate = Mouse.GetState();

        // Handle player movement input
        player.HandleMovement(gameTime, _graphics);

        // Detecting collision with blocks
        foreach (StaticPlatifactObject block in squares)
        {
            if ((!block.isNothing) && block.isBlocking)
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
                        if (player.jumpAccel.Y < player.gravity.Y * -1)
                            player.jumpAccel = player.gravity * -1;
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

        // Only handle clicks inside the game window
        if (0 <= mstate.X && mstate.X <= _graphics.PreferredBackBufferWidth && 0 <= mstate.Y && mstate.Y <= _graphics.PreferredBackBufferHeight)
        {
            // Left click handling
            if (mstate.LeftButton == ButtonState.Pressed)
            {
                int x = mstate.X / 64;
                int y = mstate.Y / 64;
                StaticPlatifactObject clicked = squares[y, x];

                // Handle clicking on empty space
                if (clicked.isNothing)
                {
                    Miner newClicked = new Miner(squares[y+1, x]);
                    if (squares[y + 1, x] is Platform)
                    {
                        newClicked.isRunning = true;
                    }
                    newClicked.position = clicked.position;
                    newClicked.texture = Content.Load<Texture2D>("miner_spritesheet");
                    squares[y, x] =  newClicked;
                }
            }

            // Right click handling
            else if (mstate.RightButton == ButtonState.Pressed)
            {
                int x = mstate.X / 64;
                int y = mstate.Y / 64;
                StaticPlatifactObject clicked = squares[y, x];
                if (!clicked.isNothing)
                {
                    StaticPlatifactObject newClicked = new StaticPlatifactObject();
                    newClicked.position = clicked.position;
                    squares[y, x] = newClicked;
                }
            }
        }

        // Updating miners
        foreach (StaticPlatifactObject block in squares)
        {
            if (block is Miner)
            {
                StaticPlatifactObject below = squares[(int)block.position.Y / 64 + 1, (int)block.position.X / 64];
                ((Miner)block).Update(gameTime, below);
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
        player.Draw(_spriteBatch);

        // Draw blocks
        foreach (StaticPlatifactObject block in squares)
        {
            if (!block.isNothing)
            {
                block.Draw(_spriteBatch);
            }   
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

