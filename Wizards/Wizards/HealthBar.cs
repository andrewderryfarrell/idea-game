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
        private Texture2D mHealth;

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
            //Draw the box around the health bar
            Rectangle healthbarectangle = new Rectangle(window.ClientBounds.Width - mHealthBar.Width, 2, mHealthBar.Width, mHealthBar.Height);
            theSpriteBatch.Draw(mHealthBar, healthbarectangle, Color.White);

            //Draw Background
            Rectangle backgroundrectangle = new Rectangle((window.ClientBounds.Width - mHealthBar.Width) + 1 , 2, mHealthBar.Width - 1, mHealthBar.Height - 1);
            theSpriteBatch.Draw(mHealthBar, backgroundrectangle, Color.Gray);
            
            //Draw the current health level based on the current Health
            Rectangle variablehealthrectangle = new Rectangle((window.ClientBounds.Width - mHealthBar.Width) + 1, 2, (int)((mHealthBar.Width - 1) * ((double)mCurrentHealth / 100)), mHealthBar.Height - 1);
            theSpriteBatch.Draw(mHealthBar, variablehealthrectangle, Color.Red);

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
