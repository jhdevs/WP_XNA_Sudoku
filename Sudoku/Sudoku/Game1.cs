using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Phone.Shell;

namespace Sudoku
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public enum GameState
        {
            Load,
            Menu,
            Level,
            Rules,
            Game,
            Pause,
            Settings,
            Finish
        }

        GameState gamestate = GameState.Load;
        SpriteFont numfont;
        SpriteFont fieldfont;
        SpriteFont fieldfontB;

        Texture2D board;
        Texture2D middlescreen;
        Texture2D settings;
        Texture2D rules;
        Texture2D back;

        Rectangle boardBounds;
        Rectangle middleBounds;


        Song back_music;
        float musicVolume = 0.3f;
        SoundEffect click;
        SoundEffect pick;
        SoundEffect select;
        float sfxVolume = 1.0f;

        NumButton[] nums;
        int currentNum;
        int lastChange = 81;

        Field[] fields;

        SerializableData data;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;

            // Hook up lifecycle events
            PhoneApplicationService.Current.Launching += Game_Launching;
            PhoneApplicationService.Current.Activated += Game_Activated;
            PhoneApplicationService.Current.Closing += Game_Closing;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            TouchPanel.EnabledGestures = GestureType.Tap;
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
            currentNum = 1;
            fields = new Field[81];
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
            Viewport screen = GraphicsDevice.Viewport;

            back_music = Content.Load<Song>("some_back_music2");
            MediaPlayer.Play(back_music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = musicVolume;
            click = Content.Load<SoundEffect>("click");
            pick = Content.Load<SoundEffect>("pick3");
            select = Content.Load<SoundEffect>("select2");

            board = Content.Load<Texture2D>("board");
            //Vector2 boardPos = new Vector2(screen.Width / 2 - board.Width / 2, screen.Height / 2 - board.Height / 2 + 250);
            boardBounds = new Rectangle(screen.Width / 2 - board.Width / 2, screen.Height / 2 - board.Height / 2 - 50, board.Width, board.Height);
            float boardIter = board.Width / 9;

            for (int i = 0; i < 81; i++)
            {
                fields[i] = new Field(i,boardBounds, boardIter);
            }

            numfont = Content.Load<SpriteFont>("numfont");

            nums = new NumButton[10];
            int txtSpacers = 20;
            Vector2 position = new Vector2(txtSpacers, 570);
            float txtOffset = (screen.Width - 2 * txtSpacers) / 10;
            for (int i = 0; i < 10; i++)
            {
                nums[i] = new NumButton(i, numfont, position, txtOffset);
                position = new Vector2(position.X + txtOffset, position.Y);
            }

            fieldfont = Content.Load<SpriteFont>("fieldfont");
            fieldfontB = Content.Load<SpriteFont>("fieldfontB");
            /*
            fields[1].SetNum(1, fieldfontB, true);
            fields[3].SetNum(7, fieldfontB, true);
            fields[5].SetNum(8, fieldfontB, true);
            fields[6].SetNum(6, fieldfontB, true);
            fields[11].SetNum(9, fieldfontB, true);
            fields[13].SetNum(3, fieldfontB, true);
            fields[14].SetNum(6, fieldfontB, true);
            fields[15].SetNum(4, fieldfontB, true);
            fields[17].SetNum(5, fieldfontB, true);
            fields[18].SetNum(7, fieldfontB, true);
            fields[19].SetNum(6, fieldfontB, true);
            fields[25].SetNum(1, fieldfontB, true);
            fields[27].SetNum(4, fieldfontB, true);
            fields[28].SetNum(3, fieldfontB, true);
            fields[30].SetNum(8, fieldfontB, true);
            fields[32].SetNum(1, fieldfontB, true);
            fields[35].SetNum(9, fieldfontB, true);
            fields[37].SetNum(9, fieldfontB, true);
            fields[40].SetNum(5, fieldfontB, true);
            fields[43].SetNum(8, fieldfontB, true);
            fields[45].SetNum(5, fieldfontB, true);
            fields[48].SetNum(3, fieldfontB, true);
            fields[50].SetNum(2, fieldfontB, true);
            fields[52].SetNum(6, fieldfontB, true);
            fields[53].SetNum(4, fieldfontB, true);
            fields[55].SetNum(8, fieldfontB, true);
            fields[61].SetNum(4, fieldfontB, true);
            fields[62].SetNum(6, fieldfontB, true);
            fields[63].SetNum(9, fieldfontB, true);
            fields[65].SetNum(3, fieldfontB, true);
            fields[66].SetNum(6, fieldfontB, true);
            fields[67].SetNum(1, fieldfontB, true);
            fields[69].SetNum(8, fieldfontB, true);
            fields[74].SetNum(7, fieldfontB, true);
            fields[75].SetNum(2, fieldfontB, true);
            fields[77].SetNum(4, fieldfontB, true);
            fields[79].SetNum(3, fieldfontB, true);
            */
            gamestate = GameState.Menu;
            // TODO: use this.Content to load your game content here
            
            Debug.WriteLine("asd");
            IsolatedStorageFileStream isfStream = null;
            XDocument doc = null;
            var storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!storage.DirectoryExists("levels")) storage.CreateDirectory("levels");
            //if (storage.FileExists("levels\\Level2.xml")) storage.DeleteFile("levels\\Level2.xml");
            if (!storage.FileExists("levels\\Level2.xml"))
            {
                doc = XDocument.Load("levels/level2.xml");
                isfStream = new IsolatedStorageFileStream("levels\\Level2.xml", FileMode.CreateNew, storage);
                doc.Save(isfStream);
                isfStream.Close();
            }
            else
            {
                isfStream = new IsolatedStorageFileStream("levels\\Level2.xml", FileMode.Open, storage);
                doc = XDocument.Load(isfStream);
                isfStream.Close();
            }
            if (doc != null)
            {
                foreach (XElement el in doc.Root.Elements("Field")) 
                {
                    int i = int.Parse(el.Element("position").Value);
                    bool preset = (el.Element("preset").Value == "true");
                    int num = int.Parse(el.Element("num").Value);
                    if (i>=0 && i<81 && num>0 && num<10)
                    {
                        fields[i].SetNum(num, preset?fieldfontB:fieldfont, preset);
                    }
                }
            }
            data = new SerializableData();
            data.txt = "Sudoku Game";
            middlescreen = Content.Load<Texture2D>("middlescreen");
            settings = Content.Load<Texture2D>("settings2");
            back = Content.Load<Texture2D>("back");
            rules = Content.Load<Texture2D>("rules");
            middleBounds = new Rectangle((int)(screen.Width - middlescreen.Width) / 2, (int)(screen.Height - middlescreen.Height) / 2,middlescreen.Width,middlescreen.Height);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            XElement root = new XElement("XnaContent");
            for (int i = 0; i < 81; i++)
            {
                if (fields[i].i>0) root.Add(new XElement("Field",
                        new XElement("position", i.ToString()),
                        new XElement("preset", fields[i].preset?"true":"false"),
                        new XElement("num", fields[i].txt)
                    ));
            }

            XDocument doc = new XDocument(root);
            doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
            var storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists("levels\\Level2.xml")) storage.DeleteFile("levels\\Level2.xml");
            IsolatedStorageFileStream isfStream = new IsolatedStorageFileStream("levels\\Level2.xml", FileMode.CreateNew, storage);
            doc.Save(isfStream);
            isfStream.Close();
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
                switch (gamestate)
                {
                    case GameState.Menu:
                        select.Play(sfxVolume, 0.0f, 0.0f);
                        this.Exit();
                        break;
                    case GameState.Game:
                        select.Play(sfxVolume, 0.0f, 0.0f);
                        gamestate = GameState.Pause;
                        break;
                    case GameState.Pause:
                        select.Play(sfxVolume, 0.0f, 0.0f);
                        gamestate = GameState.Menu;
                        break;
                    case GameState.Finish:
                        select.Play(sfxVolume, 0.0f, 0.0f);
                        gamestate = GameState.Menu;
                        break;
                }
                return;
            }

            Vector2 touchPosition = Vector2.Zero;
            if (TouchPanel.IsGestureAvailable)
               {
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                {
                    touchPosition = new Vector2(gesture.Position.X, gesture.Position.Y);
                }
            }
            /*TouchCollection touchInput = TouchPanel.GetState();
            //look at all touch points (usually 1)
            Vector2 touchPosition = Vector2.Zero;
            foreach(TouchLocation touch in touchInput)
            {
                touchPosition = touch.Position;
            }*/

            if (touchPosition != Vector2.Zero)
            {
                Rectangle touchRect = new Rectangle((int)touchPosition.X - 2, (int)touchPosition.Y - 2, 4, 4);
                switch (gamestate)
                {
                    case GameState.Game:
                        for (int i = 0; i < 10; i++)
                        {
                            if (nums[i].bounds.Intersects(touchRect)) {
                                currentNum = i;
                                if (lastChange!=-i-1) 
                                {
                                    lastChange = -i - 1;
                                    click.Play(sfxVolume, 0.0f, 0.0f);
                                }
                            }
                        }
                        if (touchRect.Intersects(new Rectangle(140, 725, 50, 50)))
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                            gamestate = GameState.Menu;
                            return;
                        }
                        else if (touchRect.Intersects(new Rectangle(215, 725, 50, 50)))
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                            gamestate = GameState.Rules;
                            return;
                        }
                        else if (touchRect.Intersects(new Rectangle(295, 725, 50, 50)))
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                            gamestate = GameState.Settings;
                            return;
                        }

                        if (boardBounds.Intersects(touchRect))
                        {
                            /*int x = (int)((touchPosition.X - boardBounds.Left) / boardIter);
                            int y = (int)((touchPosition.Y - boardBounds.Top) / boardIter);
                            if (x < 9 && y < 9)
                            {
                                int i = x + 9 * y;
                                fields[i].SetNum(currentNum, fieldfont, boardBounds, boardIter);
                            }
                             */
                            bool all = false;
                            for (int i = 0; i < 81; i++)
                            {
                                if (!fields[i].preset && fields[i].bounds.Intersects(touchRect)) {
                                    if (lastChange != i && fields[i].i != currentNum)
                                    {
                                        all = true;
                                        lastChange = i;
                                        fields[i].SetNum(currentNum, fieldfont);
                                        pick.Play(sfxVolume, 0.0f, 0.0f);
                                    }
                                    break;
                                }
                            }
                            if (all)
                            {
                                for (int i = 0; i < 81; i++)
                                {
                                    all = all & (fields[i].i > 0);
                                }
                            }
                            if (all)
                            {
                                bool ok = true;
                                for (int i = 1; i < 10; i++)
                                { //цифра
                                    for (int j = 0; j < 9; j++)
                                    {//строка, столбец или квадрат
                                        ok = ok & ((fields[j * 9].i == i) || (fields[j * 9 + 1].i == i) || (fields[j * 9 + 2].i == i) || (fields[j * 9 + 3].i == i) || (fields[j * 9 + 4].i == i) ||
                                            (fields[j * 9 + 5].i == i) || (fields[j * 9 + 6].i == i) || (fields[j * 9 + 7].i == i) || (fields[j * 9 + 8].i == i));
                                        ok = ok & ((fields[j].i == i) || (fields[j + 9 * 1].i == i) || (fields[j + 9 * 2].i == i) || (fields[j + 9 * 3].i == i) || (fields[j + 9 * 4].i == i) ||
                                            (fields[j + 9 * 5].i == i) || (fields[j + 9 * 6].i == i) || (fields[j + 9 * 7].i == i) || (fields[j + 9 * 8].i == i));
                                        int x = j % 3;
                                        int y = j / 3;
                                        ok = ok & ((fields[x * 3 + y * 3 * 9].i == i) || (fields[x * 3 + y * 3 * 9 + 1].i == i) || (fields[x * 3 + y * 3 * 9 + 2].i == i) || (fields[x * 3 + y * 3 * 9 + 9].i == i) || (fields[x * 3 + y * 3 * 9 + 9 + 1].i == i) ||
                                            (fields[x * 3 + y * 3 * 9 + 9 + 2].i == i) || (fields[x * 3 + y * 3 * 9 + 18].i == i) || (fields[x * 3 + y * 3 * 9 + 18 + 1].i == i) || (fields[x * 3 + y * 3 * 9 + 18 + 2].i == i));
                                    }
                                }
                                if (ok)
                                {
                                    gamestate = GameState.Finish;
                                }
                            }
                        }
                        break;
                    case GameState.Menu:
                        if (touchPosition.Y >= 300 && touchPosition.Y < 350)
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                            gamestate = GameState.Game;
                        }
                        else if (touchPosition.Y >= 350 && touchPosition.Y < 400)
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                        }
                        else if (touchPosition.Y >= 400 && touchPosition.Y < 450)
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                        }
                        else if (touchPosition.Y >= 450 && touchPosition.Y < 500)
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                            this.Exit();
                        }
                        break;
                    case GameState.Pause:
                        if (middleBounds.Intersects(touchRect))
                        {
                            select.Play(sfxVolume, 0.0f, 0.0f);
                            gamestate = GameState.Game;
                            return;
                        }
                        break;
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
            GraphicsDevice.Clear(Color.LightGray);
            Viewport screen = GraphicsDevice.Viewport;
            
            spriteBatch.Begin();

            switch (gamestate)
            {
                case GameState.Menu:
                    spriteBatch.DrawString(numfont, data.txt, new Vector2(20, 20), Color.Black, 0.0f, new Vector2(0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(numfont, "PLAY", new Vector2(50, 300), Color.Black);
                    spriteBatch.DrawString(numfont, "RULES", new Vector2(50, 350), Color.Black);
                    spriteBatch.DrawString(numfont, "SETTINGS", new Vector2(50, 400), Color.Black);
                    spriteBatch.DrawString(numfont, "QUIT", new Vector2(50, 450), Color.Black);
                    break;
                case GameState.Finish:
                    String txt = "FINISH";
                    Vector2 size = numfont.MeasureString(txt);
                    spriteBatch.DrawString(numfont, txt, new Vector2((screen.Width-size.X)/2,(screen.Height-size.Y)/2), Color.Black);
                    break;
                case GameState.Game:
                    spriteBatch.DrawString(numfont, data.txt, new Vector2(20, 25), Color.Black, 0.0f, new Vector2(0), 0.7f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(back, new Rectangle(140, 730, 40, 40), Color.White);
                    spriteBatch.Draw(rules, new Rectangle(215, 725, 50, 50), Color.White);
                    spriteBatch.Draw(settings, new Rectangle(295, 725, 50, 50), Color.White);
                    spriteBatch.Draw(board, boardBounds, Color.White);

                    for (int i = 0; i < 10; i++)
                    {
                        spriteBatch.DrawString(numfont, nums[i].i, nums[i].position, currentNum == i ? Color.Black : Color.White);
                    }
                    for (int i = 0; i < 81; i++)
                    {
                        if (fields[i].i > 0)
                        {
                            spriteBatch.DrawString(fields[i].preset ? fieldfontB : fieldfont, fields[i].txt, fields[i].position, Color.Black);
                        }
                    }
                    break;
                case GameState.Pause:
                    spriteBatch.DrawString(numfont, data.txt, new Vector2(20, 25), Color.Black, 0.0f, new Vector2(0), 0.7f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(board, boardBounds, Color.White);
                    for (int i = 0; i < 81; i++)
                    {
                        if (fields[i].i > 0)
                        {
                            if (fields[i].preset) spriteBatch.DrawString(fields[i].preset ? fieldfontB : fieldfont, fields[i].txt, fields[i].position, Color.Black);
                        }
                    }
                    Vector2 pos = new Vector2((screen.Width - middlescreen.Width) / 2, (screen.Height - middlescreen.Height) / 2);
                    spriteBatch.Draw(middlescreen, middleBounds, Color.White);
                    spriteBatch.DrawString(numfont, "PAUSED", new Vector2(middleBounds.X+80,middleBounds.Y+20), Color.Black, 0.0f, new Vector2(0), 0.7f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(numfont, "tap to continue...", new Vector2(middleBounds.X + 90, middleBounds.Y + 90), Color.Black, 0.0f, new Vector2(0), 0.5f, SpriteEffects.None, 0.0f);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Game_Closing(object sender, ClosingEventArgs e)
        {
            UnloadContent();
            //PhoneApplicationService.Current.State["fields"] = fields;
            PhoneApplicationService.Current.State["data"] = data;
        }

        private void Game_Activated(object sender, ActivatedEventArgs e)
        {
            //ReloadRequired = !e.IsApplicationInstancePreserved;

            if (e.IsApplicationInstancePreserved)
            {
                // The instance was preserved, so use the local member which was not serialized
                PhoneApplicationService.Current.State["data"] = data;
            }
        }

        private void Game_Launching(object sender, LaunchingEventArgs e)
        {
            //ReloadRequired = true;

            PhoneApplicationService.Current.State["text"] = false;
            

            // Display the main screen
        }
    }
}
