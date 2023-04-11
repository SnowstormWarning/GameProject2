using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject2.StateManagement;
using GameProject2.Sprites;

namespace GameProject2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static SpriteFont _font;
        private StateManager _stateManager;
        public static Vector2 GlobalScalingFactor;
        public static BlockClearParticleSystem BlockClearParticleSystem;
        public static PlaceParticleSystem PlaceParticleSystem;
        public static SpriteFont Font => _font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.Title = "Downfall";
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            BlockClearParticleSystem = new BlockClearParticleSystem(this, 100);
            Components.Add(BlockClearParticleSystem);
            PlaceParticleSystem = new PlaceParticleSystem(this, 25);
            Components.Add(PlaceParticleSystem);
            //_graphics.IsFullScreen = true; DOES NOT WORK IN DEBUG MODE
            _graphics.ApplyChanges();
            ChangeGlobalScaling();
            _stateManager = new StateManager(GraphicsDevice);
            Vector2 test = GlobalScalingFactor;
            _stateManager.AddUnloadedState(new MainMenu());
            _stateManager.CurrentState.Initialize();
            base.Initialize();
        }

        public void ChangeGlobalScaling()
        {
            GlobalScalingFactor = new Vector2(((float)GraphicsDevice.DisplayMode.Width) / ((float)2560), ((float)GraphicsDevice.DisplayMode.Height) / ((float)1440));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("arial");
            _stateManager.CurrentState.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || StateManager.CloseGame)
                Exit();

            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();
            _stateManager.CurrentState.Update(gameTime,mouse,keyboard);
            switch (_stateManager.CurrentState.GetStateCommand())
            {
                case StateCommands.None:
                    break;
                case StateCommands.AddTarget:
                    _stateManager.AddState(_stateManager.CurrentState.GetTargetState(), Content);
                    break;
                case StateCommands.RemoveSource:
                    _stateManager.RemoveCurrentState();
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _stateManager.CurrentState.Draw(gameTime,_spriteBatch,Font);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}