﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platifact;

public class Game1 : Game
{
    GameObject ball = new GameObject();
    Vector2 ballPosition; 
    float ballSpeed;

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
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        ballSpeed = 100f;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ball.texture = Content.Load<Texture2D>("ball");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var kstate = Keyboard.GetState();

        // Movement
        if (kstate.IsKeyDown(Keys.W))
        {
            ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (kstate.IsKeyDown(Keys.S))
        {
            ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (kstate.IsKeyDown(Keys.A))
        {
            ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (kstate.IsKeyDown(Keys.D))
        {
            ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        // Out of bounds restrictions
        if (ballPosition.X > _graphics.PreferredBackBufferWidth - ball.texture.Width / 2)
        {
            ballPosition.X = _graphics.PreferredBackBufferWidth - ball.texture.Width / 2;
        }

        else if (ballPosition.X < ball.texture.Width / 2)
        {
            ballPosition.X = ball.texture.Width / 2;
        }

        if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ball.texture.Height / 2)
        {
            ballPosition.Y = _graphics.PreferredBackBufferHeight - ball.texture.Height / 2;
        }

        else if (ballPosition.Y < ball.texture.Height / 2)
        {
            ballPosition.Y = ball.texture.Height / 2;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(
            ball.texture,
            ballPosition,
            null,
            Color.White,
            0f,
            new Vector2(ball.texture.Width / 2, ball.texture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

