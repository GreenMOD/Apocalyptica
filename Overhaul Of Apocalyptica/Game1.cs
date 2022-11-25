using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private EntityManager entityManager;
        private Texture2D NinjaSpriteSheet;
        private Texture2D ZombieSheet;
        private Texture2D DessertMap;
        private Texture2D NodePathfinding;
        private Texture2D Target;
        private Texture2D WaveCounterSpriteSheet;
        private Texture2D HeartSpriteSheet;
        private Texture2D projectileSpriteSheet;
        private WaveManager waveManager;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            entityManager = new EntityManager();
            
        }

        protected override void Initialize()
        {

           
           
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            NinjaSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/NinjaSpriteSheet");
            ZombieSheet = Content.Load<Texture2D>(@"SpriteSheets/ApocZombieSpriteSheet");
            DessertMap = Content.Load<Texture2D>(@"SpriteSheets/Dessert1");
            NodePathfinding = Content.Load<Texture2D>(@"Debugging/Pathfinder tracker");
            Target = Content.Load<Texture2D>(@"Debugging/target");
            WaveCounterSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/WaveCounterSprite");
            HeartSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/Heart");
            projectileSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/captainProjectile");
            
            
            Ninja player1 = new Ninja(NinjaSpriteSheet,HeartSpriteSheet);
            entityManager.AddEntity(player1);
            

            waveManager = new WaveManager(ZombieSheet,player1,entityManager, WaveCounterSpriteSheet, projectileSpriteSheet);
            

            player1.Activate();
            
            foreach (Zombie z in waveManager.zombiesSpawned)
            {
                entityManager.AddEntity(z);
            }
           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
            waveManager.Update(gameTime);
            if (waveManager.zombiesToAdd.Count != 0)
            {
                foreach (Zombie z in waveManager.zombiesToAdd)
                {
                    entityManager.AddEntity(z);
                }
            }
            base.Update(gameTime);

            entityManager.Update(gameTime);
            if (waveManager.isRunning == false) //not the most elegant of solutions however will be the best for the time being and in the long term for when game states are added
            {
                waveManager.Intialise();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(DessertMap, new Rectangle(0,0,Window.ClientBounds.Width,Window.ClientBounds.Height), Color.Yellow);
            
            entityManager.Draw(_spriteBatch, gameTime);

            waveManager.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
