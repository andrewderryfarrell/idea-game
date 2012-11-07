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
    class FuelBar
    {
        //The texture object used when drawing the sprite
        private Texture2D mFuelBar;

        //The variable to keep track of health
        private int mCurrentFuel = 1000;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mFuelBar = theContentManager.Load<Texture2D>(theAssetName);
        }
        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch, GameWindow window)
        {
            //Draw the box around the health bar
            Rectangle fuelbarectangle = new Rectangle(window.ClientBounds.Width - mFuelBar.Width, mFuelBar.Height + 4, mFuelBar.Width, mFuelBar.Height);
            theSpriteBatch.Draw(mFuelBar, fuelbarectangle, Color.White);

            //Draw Background
            Rectangle backgroundrectangle = new Rectangle((window.ClientBounds.Width - mFuelBar.Width) + 1, mFuelBar.Height + 4, mFuelBar.Width - 1, mFuelBar.Height - 1);
            theSpriteBatch.Draw(mFuelBar, backgroundrectangle, Color.Gray);

            //Draw the current health level based on the current Health
            Rectangle variablehealthrectangle = new Rectangle((window.ClientBounds.Width - mFuelBar.Width) + 1, mFuelBar.Height + 4, (int)((mFuelBar.Width - 1) * ((double)mCurrentFuel / 1000)), mFuelBar.Height - 1);
            theSpriteBatch.Draw(mFuelBar, variablehealthrectangle, Color.OrangeRed);

        }
        //Change Health
        public void changeFuel(int healthchange)
        {

            mCurrentFuel += healthchange;

            //Force the health to remain between 0 and 100
            mCurrentFuel = (int)MathHelper.Clamp(mCurrentFuel, 0, 1000);

        }
    }
}
