using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpotASheepWindows
{
  public static class Menu
  {   
    static Texture2D _NewGame, _HighScore, _Title;
    static bool _IsMenuShown;
    static DisplayMode displayMode;
    static Rectangle[] pulsanti = new Rectangle[3];

    public static bool IsMenuShown
    {
      get { return Menu._IsMenuShown; }
      set { Menu._IsMenuShown = value; }
    }
    public static void InitializeMenu(ContentManager Content)
    {
      displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
      _Title = Content.Load<Texture2D>("Title");
      _NewGame = Content.Load<Texture2D>("NewGame");
      _HighScore = Content.Load<Texture2D>("HighScores");
      pulsanti[0] = new Rectangle((displayMode.Width - _Title.Width) / 2, 20, 389, 167);
      pulsanti[1] = new Rectangle((displayMode.Width - _NewGame.Width) / 2, 220, 371, 66);
      pulsanti[2] = new Rectangle((displayMode.Width - _HighScore.Width) / 2, 320, 371, 66);
      _IsMenuShown = true;
    }
    
    public static void DrawMenu(SpriteBatch sb)
    {
      sb.Begin();
      sb.Draw(_Title, pulsanti[0], Color.White);
      sb.Draw(_NewGame, pulsanti[1], Color.White);
      sb.Draw(_HighScore, pulsanti[2], Color.White);
      sb.End();
    }

    public static int MenuTouchInput(MouseState tl)
    {
      Rectangle tmpRett = new Rectangle((int)tl.X, (int)tl.Y, 1,1);
      if(tmpRett.Intersects(pulsanti[1]))
        return 1;
      else if (tmpRett.Intersects(pulsanti[2]))
        return 2;
      else
        return -1;
    }
  }
}
