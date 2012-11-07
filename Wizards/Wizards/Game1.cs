using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Wizards
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        List<Fire> balls = new List<Fire>();
        List<Enemy> enemies = new List<Enemy>();
        TimeSpan minForFire = TimeSpan.FromMilliseconds(200);
        TimeSpan totalForFireElapsed;
        TimeSpan minForSpawn = TimeSpan.FromMilliseconds(5000);
        TimeSpan totalForSpawnElapsed;
        Knight player = new Knight();
        MouseIcon mMouseIconSprite = new MouseIcon();
        //Define new Healthbar object
        HealthBar mHealthbar;
        FuelBar mFuelbar;
        Vector2 oldposition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //balls.Add(new Knight());
            //balls.Add(new MouseIcon());
            player = new Knight();
            mMouseIconSprite = new MouseIcon();
            totalForFireElapsed = TimeSpan.FromMilliseconds(200);
            totalForSpawnElapsed = TimeSpan.Zero;
            //Create Healthbar object
            mHealthbar = new HealthBar();
            mFuelbar = new FuelBar();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            /*foreach (Fire a in balls)
            {
                a.LoadContent(this.Content);
            }*/
            player.LoadContent(this.Content);
            mMouseIconSprite.LoadContent(this.Content);

            //Load Content for HealthBar
            mHealthbar.LoadContent(this.Content, "healthbartest");

            //Load Content for FuelBar
            mFuelbar.LoadContent(this.Content, "healthbartest");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //Fuel change
            if(!player.Position.Equals(oldposition))
            {
                mFuelbar.changeFuel(-1);
            }
            oldposition = player.Position;
            mMouseIconSprite.Update();
            player.Update(gameTime, graphics);
            MouseState ms = Mouse.GetState();
            UpdateFire(ms, gameTime);
            SpawnEnemy(gameTime);
            foreach (Fire a in balls) // Loop through List with foreach
            {
                a.Update(gameTime);
            }
            foreach (Enemy a in enemies) // Loop through List with foreach
            {
                if(!checkEnemyPlayerCollision(a))
                {
                    a.Update(gameTime, player.Position, player.Size);
                }
            }
            removeLostBalls();
            checkFireEnemyCollision();

            //Change health on contact
            if (checkEnemyPlayerCollision(null))
            {
                mHealthbar.changeHealth(-1);
            }

            base.Update(gameTime);
        }

        private Boolean checkEnemyPlayerCollision(Enemy x)
        {
            float enemyPosX, enemyPosY, playerOffsetX, playerOffsetY, playerPosX, playerPosY;
            playerPosX = player.Position.X;
            playerPosY = player.Position.Y;
            playerOffsetX = player.Position.X + player.Size.Width;
            playerOffsetY = player.Position.Y + player.Size.Width;
            if (x == null)
            {
                foreach (Enemy b in enemies)
                {
                    enemyPosX = b.Position.X + (b.Size.Width / 2);
                    enemyPosY = b.Position.Y + (b.Size.Width / 2);
                    //If collides with enemy
                    if (enemyPosX > playerPosX && enemyPosX < playerOffsetX && enemyPosY > playerPosY && enemyPosY < playerOffsetY)
                    {
                        return true;
                    }
                }
            }
            else
            {
                enemyPosX = x.Position.X + (x.Size.Width / 2);
                enemyPosY = x.Position.Y + (x.Size.Width / 2);
                if (enemyPosX > playerPosX && enemyPosX < playerOffsetX && enemyPosY > playerPosY && enemyPosY < playerOffsetY)
                {
                    return true;
                }
            }
            return false;
        }

        private void checkFireEnemyCollision()
        {
            List<Fire> fireToDelete = new List<Fire>();
            List<Enemy> enemyToDelete = new List<Enemy>();
            float firePosX, firePosY, enemyPosX, enemyPosY, enemyOffsetX, enemyOffsetY;
            foreach (Fire a in balls)
            {
                firePosX = a.Position.X + (a.Size.Width/2);
                firePosY = a.Position.Y + (a.Size.Width/2);
                foreach (Enemy b in enemies)
                {
                    enemyPosX = b.Position.X;
                    enemyPosY = b.Position.Y;
                    enemyOffsetX = b.Position.X + b.Size.Width;
                    enemyOffsetY = b.Position.Y + b.Size.Width;
                    if (firePosX > enemyPosX && firePosX < enemyOffsetX && firePosY > enemyPosY && firePosY < enemyOffsetY)
                    {
                        fireToDelete.Add(a);
                        enemyToDelete.Add(b);
                    }
                }
            }
            foreach (Fire a in fireToDelete)
            {
                balls.Remove(a);
            }
            foreach (Enemy a in enemyToDelete)
            {
                enemies.Remove(a);
                //Every time an enemy is remove, lower total health by 5
                //mHealthbar.changeHealth(-5);
            }
        }

        private void removeLostBalls()
        {
            List<Fire> toDelete = new List<Fire>();
            foreach (Fire a in balls)
            {
                // If the fireball is off the screen - delete it.
                if (a.Position.X < 0 || a.Position.Y < 0 || a.Position.X > graphics.GraphicsDevice.Viewport.Width || a.Position.Y > graphics.GraphicsDevice.Viewport.Height)
                {
                    toDelete.Add(a);
                }
            }
            foreach (Fire a in toDelete)
            {
                balls.Remove(a);
            }
        }

        private void SpawnEnemy(GameTime gameTime)
        {
            if (((totalForSpawnElapsed += gameTime.ElapsedGameTime) > minForSpawn) && enemies.Count < 5)
            {
                Enemy a = new Enemy(graphics);
                a.LoadContent(this.Content);
                totalForSpawnElapsed = TimeSpan.Zero;
                enemies.Add(a);
            }
        }

        private void UpdateFire(MouseState ms, GameTime gameTime)
        {
            if (ms.LeftButton == ButtonState.Pressed && (totalForFireElapsed += gameTime.ElapsedGameTime) > minForFire)
            {
                Vector2 adjustedPlayPos = player.Position;
                adjustedPlayPos.X += (player.Size.Width/2);
                adjustedPlayPos.Y += (player.Size.Height/2);
                Fire a = new Fire(adjustedPlayPos, mMouseIconSprite.Position);
                a.LoadContent(this.Content);
                totalForFireElapsed = TimeSpan.Zero;
                balls.Add(a);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            foreach (Fire a in balls) // Loop through List with foreach
            {
                a.Draw(spriteBatch);
            }
            foreach (Enemy a in enemies) // Loop through List with foreach
            {
                a.Draw(spriteBatch);
            }
            
           

            //Draw the healthbar on the screen
            mHealthbar.Draw(spriteBatch, this.Window);

            //Draw the fuelbar to the screen
            mFuelbar.Draw(spriteBatch, this.Window);
            mMouseIconSprite.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
