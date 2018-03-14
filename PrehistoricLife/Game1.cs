using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrehistoricLife
{
    public class Game1 : Game
    {
        public Point position = new Point(0, 0);
        Point worldScreenSize = new Point(600, 600);
        Point screenSize = new Point(800, 600);
        Texture2D atlasTexture;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Simulation simulation = new Simulation();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenSize.X;
            graphics.PreferredBackBufferHeight = screenSize.Y;
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            atlasTexture = Content.Load<Texture2D>("textures");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyBoardState = Keyboard.GetState();
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                position.Y--;
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                position.Y++;
            }
            if (keyBoardState.IsKeyDown(Keys.Left))
            {
                position.X--;
            }
            if (keyBoardState.IsKeyDown(Keys.Right))
            {
                position.X++;
            }
            simulation.Go(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            simulation.world.Draw(spriteBatch,atlasTexture, position, 60, worldScreenSize);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
