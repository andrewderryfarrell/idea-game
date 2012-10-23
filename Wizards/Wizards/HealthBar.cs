using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;



namespace Wizards
{
    class HealthBar
    {
        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        private Texture2D mHealthBar;

        //The variable to keep track of health
        private int mCurrentHealth = 100;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mHealthBar = theContentManager.Load<Texture2D>(theAssetName);
        }
        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch, GameWindow window)
        {
            theSpriteBatch.Draw(mHealthBar, new Rectangle(window.ClientBounds.Width / 2 - mHealthBar.Width / 2, 30, mHealthBar.Width, 44), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Gray);
            
            //Draw the current health level based on the current Health
            theSpriteBatch.Draw(mHealthBar, new Rectangle(window.ClientBounds.Width / 2 - mHealthBar.Width / 2, 30, (int)(mHealthBar.Width * ((double)mCurrentHealth / 100)), 44), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Red);

            //Draw the box around the health bar
            theSpriteBatch.Draw(mHealthBar, new Rectangle(window.ClientBounds.Width / 2 - mHealthBar.Width / 2, 30, mHealthBar.Width, 44), new Rectangle(0, 0, mHealthBar.Width, 44), Color.White);
        }
        //Change Health
        public void changeHealth(int healthchange)
        {

            mCurrentHealth += healthchange;

            //Force the health to remain between 0 and 100
            mCurrentHealth = (int)MathHelper.Clamp(mCurrentHealth, 0, 100);

        }
    }
}
