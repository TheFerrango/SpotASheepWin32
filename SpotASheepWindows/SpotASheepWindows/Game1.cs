using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System.Media;

namespace SpotASheepWindows
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    public bool isSubmitting;
    bool attentiAlLupo;
    MouseState _oldPanel;
    bool scoreSubmitted;
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteFont sf;
    SoundEffect se;
    LupoCattivo lc;
    Texture2D nuvola, sfondo, boom, heart, lupo, perdente;
    Texture2D[] pecorelle;
    Color[] mappaNuvola;
    List<NuvolaGen> listaNuvole;
    List<NuvolaGen> listaPecorelle;
    List<Esplosione> listaBoom;
    Random rand;

    int punti;
    int vite;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

     graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);

      // Frame rate is 30 fps by default for Windows Phone.
      TargetElapsedTime = TimeSpan.FromTicks(333333);
      IsMouseVisible = true;
      // Extend battery life under lock.
      InactiveSleepTime = TimeSpan.FromSeconds(1);
    }

    void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
    {
      DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
      e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
      e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
      e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
      //e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = true;
    }

    #region System-generated functions

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      Menu.InitializeMenu(Content);
      pecorelle = new Texture2D[3];
      rand = new Random();
      _oldPanel = Mouse.GetState();

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
      nuvola = Content.Load<Texture2D>("nuvola");
      sf = Content.Load<SpriteFont>("SpriteFont1");
      sfondo = Content.Load<Texture2D>("levelBackGround");
      lupo = Content.Load<Texture2D>("lupo");
      perdente = Content.Load<Texture2D>("uLose");
      pecorelle[0] = Content.Load<Texture2D>("pecora1");
      pecorelle[1] = Content.Load<Texture2D>("pecora2");
      pecorelle[2] = Content.Load<Texture2D>("pecora3");
      boom = Content.Load<Texture2D>("Explosion");
      heart = Content.Load<Texture2D>("heart");
      se = Content.Load<SoundEffect>("Bomb");
      mappaNuvola = new Color[nuvola.Height * nuvola.Width];
      nuvola.GetData(mappaNuvola);
      // TODO: use this.Content to load your game content here
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
      {
        if (!Menu.IsMenuShown)
          Menu.IsMenuShown = true;
        else
          this.Exit();
      }
      if(!isSubmitting)
        TouchInput();

      if (!Menu.IsMenuShown)
      {
        if (vite >= 0)
        {
          killaPecora(gameTime);

          if (listaPecorelle.Count < 6)
            listaPecorelle.Add(new NuvolaGen(pecorelle[rand.Next(0, 3)], rand.Next(0, graphics.GraphicsDevice.DisplayMode.Height / 48) * 45, rand.Next(graphics.GraphicsDevice.DisplayMode.Width - 194, graphics.GraphicsDevice.DisplayMode.Width), (float)(rand.NextDouble() * 5) + 1.0f + (float)punti / 100, true));

          if (listaNuvole.Count < 15)
            listaNuvole.Add(new NuvolaGen(nuvola, rand.Next(0, graphics.GraphicsDevice.DisplayMode.Height / 48) * 45, rand.Next(graphics.GraphicsDevice.DisplayMode.Width - 194, graphics.GraphicsDevice.DisplayMode.Width), (float)(rand.NextDouble() * 3) + 1f + (float)punti / 100));

          UpdateParticles(gameTime);
          RemoveUselessNuvole();
          UpdatePosNuvole();
          RemoveUselessPecore();
          UpdatePosPecore();
          if (attentiAlLupo)
            UpdateLupoCattivo();
        }
        
          if (!scoreSubmitted && vite <0)
          {
            scoreSubmitted = true;
            isSubmitting = true;
            //graphics.ToggleFullScreen();
            new ScoreSubmit(punti, this).ShowDialog();
          }
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      spriteBatch.Begin();
      spriteBatch.Draw(sfondo, new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), Color.White);
      spriteBatch.End();
      // TODO: Add your drawing code here
      if (!Menu.IsMenuShown && vite >= 0)
      {
        DrawNuvole();
        DrawPecore();
        DrawExplosione();
        if (attentiAlLupo)
          DrawLupoCattivo();
        DrawHearts();
        DrawPoints();
      }
      else
      {
        if (vite < 0)
          DrawHaHaHaiPerso();
        else
          Menu.DrawMenu(spriteBatch);
      }
      base.Draw(gameTime);
    }

    #endregion

    #region Input & intersect

    public void TouchInput()
    {

      MouseState tc = Mouse.GetState();

      
        if (!Menu.IsMenuShown)
        {
          if (!attentiAlLupo)
          {
            if (!ilVecchioContiene(tc))
            {
              if (vite >= 0)
              {
                NuvolaGen toRemovePecora = IntersectPecorelle(tc);

                if (toRemovePecora != null)
                {
                  int quale = listaPecorelle.IndexOf(toRemovePecora);
                  listaPecorelle[quale].Touched = true;
                  punti += 10;
                }
                else
                {
                  NuvolaGen toRemoveNuvola = IntersectNuvolette(tc);
                  if (toRemoveNuvola != null)
                  {
                    int quale = listaNuvole.IndexOf(toRemoveNuvola);
                    listaNuvole[quale].Touched = true;
                    if (vite >= 0)
                    {
                      vite--;
                      if (vite >= 0)
                      {
                        for (int i = 0; i < listaPecorelle.Count; i++)
                          listaPecorelle[i].ScappaDalLupoCattivo(rand.Next(5, 11));
                        lc = new LupoCattivo();
                        attentiAlLupo = true;
                      }
                    }
                  }
                }
              }
              else
              {
               
                Menu.IsMenuShown = true;
                vite = 0;
              }
            }
          }
        }
        else
        {
          if (Menu.MenuTouchInput(tc) == 1 && !ilVecchioContiene(tc))
          {
            InizializzaGioco();
            Menu.IsMenuShown = false;
          }
          if (Menu.MenuTouchInput(tc) == 2 && !ilVecchioContiene(tc))
          {
            isSubmitting = true;
            new Form1(this).ShowDialog();
          }
        }
     

      _oldPanel = Mouse.GetState();
    }

    //public void BackToMenu

    public bool ilVecchioContiene(MouseState tl)
    {

      if (_oldPanel.LeftButton == tl.LeftButton || tl.LeftButton != ButtonState.Pressed)
          return true;
      return false;
    }

    public NuvolaGen IntersectNuvolette(MouseState tl)
    {
      for (int i = listaNuvole.Count - 1; i >= 0; i--)
      {
        Rectangle rettNuvola = new Rectangle((int)listaNuvole[i].Position.X, (int)listaNuvole[i].Position.Y, 194, 173);
        Rectangle rettTouch = new Rectangle((int)tl.X, (int)tl.Y, 1, 1);
        Color[] dataB = new Color[] { Color.Red };
        // Find the bounds of the rectangle intersection
        int top = Math.Max(rettNuvola.Top, rettTouch.Top);
        int bottom = Math.Min(rettNuvola.Bottom, rettTouch.Bottom);
        int left = Math.Max(rettNuvola.Left, rettTouch.Left);
        int right = Math.Min(rettNuvola.Right, rettTouch.Right);

        // Check every point within the intersection bounds
        for (int y = top; y < bottom; y++)
        {
          for (int x = left; x < right; x++)
          {
            // Get the color of both pixels at this point
            if (((x - rettNuvola.Left) + (y - rettNuvola.Top) * rettNuvola.Width) < mappaNuvola.Length)
            {
              Color colorA = mappaNuvola[(x - rettNuvola.Left) +
                                   (y - rettNuvola.Top) * rettNuvola.Width];
              Color colorB = dataB[(x - rettTouch.Left) +
                                   (y - rettTouch.Top) * rettTouch.Width];

              // If both pixels are not completely transparent,
              if (colorA.A != 0 && colorB.A != 0)
              {
                // then an intersection has been found
                //listaNuvole[i].Touched  = true;
                return listaNuvole[i];
              }
            }
          }
        }

        // No intersection found

      }
      return null;
    }

    public NuvolaGen IntersectPecorelle(MouseState tl)
    {
      for (int i = listaPecorelle.Count - 1; i >= 0; i--)
      {
        Rectangle rettPecora = new Rectangle((int)listaPecorelle[i].Position.X, (int)listaPecorelle[i].Position.Y, 194, 173);
        Rectangle rettTouch = new Rectangle((int)tl.X, (int)tl.Y, 1, 1);
        Color[] dataB = new Color[] { Color.Red };
        // Find the bounds of the rectangle intersection
        int top = Math.Max(rettPecora.Top, rettTouch.Top);
        int bottom = Math.Min(rettPecora.Bottom, rettTouch.Bottom);
        int left = Math.Max(rettPecora.Left, rettTouch.Left);
        int right = Math.Min(rettPecora.Right, rettTouch.Right);

        // Check every point within the intersection bounds
        for (int y = top; y < bottom; y++)
        {
          for (int x = left; x < right; x++)
          {
            // Get the color of both pixels at this point
            if (((x - rettPecora.Left) + (y - rettPecora.Top) * rettPecora.Width) < mappaNuvola.Length)
            {
              Color colorA = mappaNuvola[(x - rettPecora.Left) +
                                   (y - rettPecora.Top) * rettPecora.Width];
              Color colorB = dataB[(x - rettTouch.Left) +
                                   (y - rettTouch.Top) * rettTouch.Width];

              // If both pixels are not completely transparent,
              if (colorA.A != 0 && colorB.A != 0)
              {
                // then an intersection has been found
                //listaPecorelle[i].Touched = true;
                return listaPecorelle[i];
              }
            }
          }
        }

        // No intersection found

      }
      return null;
    }

    #endregion

    #region Updating calls for items

    public void killaPecora(GameTime t)
    {
      for (int i = 0; i < listaPecorelle.Count; i++)
        if (listaPecorelle[i].Touched)
        {
          se.Play();
          AddExplosion(listaPecorelle[i].Position, 7, 5, t);
          listaPecorelle.RemoveAt(i);
        }
    }

    public void UpdatePosNuvole()
    {
      foreach (NuvolaGen ng in listaNuvole)
      {
        ng.Update();
      }
    }

    public void RemoveUselessNuvole()
    {
      for (int i = listaNuvole.Count - 1; i >= 0; i--)
      {
        if (listaNuvole[i].Position.X < -200 || listaNuvole[i].Touched)
          listaNuvole.RemoveAt(i);
      }
    }

    public void UpdatePosPecore()
    {
      foreach (NuvolaGen ng in listaPecorelle)
      {
        ng.Update();
      }
    }

    public void RemoveUselessPecore()
    {
      for (int i = listaPecorelle.Count - 1; i >= 0; i--)
      {
        if (listaPecorelle[i].Position.X < -200)
          listaPecorelle.RemoveAt(i);
      }
    }

    public void UpdateLupoCattivo()
    {
      if (lc != null && (lc.Coord.X > GraphicsDevice.DisplayMode.Width || lc.Coord.Y > GraphicsDevice.DisplayMode.Height))
      {
        lc = null;
        attentiAlLupo = false;
      }
      else
        lc.UpdateLupo();
    }

    private void UpdateParticles(GameTime gameTime)
    {
      float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
      for (int i = listaBoom.Count - 1; i >= 0; i--)
      {
        Esplosione particle = listaBoom[i];
        // float timeAlive = now - particle.BirthTime;

        if (now > particle.AliveTime.TotalMilliseconds)
        {
          listaBoom.RemoveAt(i);
        }
        else
        {
          particle.Update();
          listaBoom[i] = particle;
        }
      }
    }

    #endregion

    #region Drawing methods

    public void DrawNuvole()
    {
      spriteBatch.Begin();
      foreach (NuvolaGen ng in listaNuvole)
      {
        //if (!ng.Touched)          
        spriteBatch.Draw(ng.Texture, ng.Position, Color.White);
      }
      spriteBatch.End();
    }

    public void DrawPecore()
    {
      spriteBatch.Begin();
      foreach (NuvolaGen ng in listaPecorelle)
      {
        if (ng.Touched)
          spriteBatch.Draw(ng.Texture, ng.Position, Color.Red);
        else
          spriteBatch.Draw(ng.Texture, ng.Position, Color.White);
      }
      spriteBatch.End();
    }

    public void DrawExplosione()
    {
      spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);

      for (int i = 0; i < listaBoom.Count; i++)
      {
        Esplosione particle = listaBoom[i];
        Vector2 coordEspl = particle.Coord;
        coordEspl.X += nuvola.Width / 2;
        coordEspl.Y += nuvola.Height / 2;
        spriteBatch.Draw(boom, coordEspl, null, Color.White, i, new Vector2(256, 256), 0.5f, SpriteEffects.None, 1);//particle.Scaling
      }
      spriteBatch.End();

    }

    public void DrawHearts()
    {
      int posYStart = graphics.GraphicsDevice.DisplayMode.Width - heart.Width * 3 - 9;
      spriteBatch.Begin();
      for (int i = 0; i < vite; i++)
      {
        spriteBatch.Draw(heart, new Vector2((float)posYStart, 4.0f), Color.Red);
        posYStart += heart.Width + 3;
      }
      spriteBatch.End();
    }

    public void DrawPoints()
    {
      spriteBatch.Begin();
      spriteBatch.DrawString(sf, "Score: " + punti.ToString(), Vector2.Zero, Color.Red);
      spriteBatch.End();
    }

    public void DrawLupoCattivo()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(lupo, lc.Coord, Color.White);
      spriteBatch.End();
    }

    public void DrawHaHaHaiPerso()
    {
      spriteBatch.Begin();
      spriteBatch.Draw(perdente, new Vector2((float)(GraphicsDevice.DisplayMode.Height / 2 - perdente.Width / 2), (float)(GraphicsDevice.DisplayMode.Width - perdente.Height) / 2), Color.White);
      spriteBatch.End();
    }

    #endregion

    #region others

    private void AddExplosion(Vector2 explosionPos, int numberOfParticles, float size, GameTime gameTime)
    {
      for (int i = 0; i < numberOfParticles; i++)
        AddExplosionParticle(explosionPos, size, gameTime);
    }

    private void AddExplosionParticle(Vector2 explosionPos, float explosionSize, GameTime gameTime)
    {
      Vector2 displacement = new Vector2((float)rand.NextDouble() * explosionSize, 0);
      displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(MathHelper.ToRadians(rand.Next(360))));

      Esplosione espl = new Esplosione(explosionPos, gameTime, displacement * 2.0f);
      listaBoom.Add(espl);
    }

    public void InizializzaGioco()
    {
      listaNuvole = new List<NuvolaGen>();
      listaPecorelle = new List<NuvolaGen>();
      listaBoom = new List<Esplosione>();
      attentiAlLupo = false;
      punti = 0;
      isSubmitting = false;
      scoreSubmitted = false;
      vite = 3;
    }

    #endregion
  }
}
