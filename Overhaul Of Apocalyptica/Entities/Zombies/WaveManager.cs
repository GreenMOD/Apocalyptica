using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica
{
    class WaveManager : IEntity
    {
        public List<List<Zombie>> _Waves = new List<List<Zombie>>();
        Random rng = new Random();
        public int _currentWave = 0;
        public List<Zombie> zombiesSpawned = new List<Zombie>(); //contains the zombies that are already in play
        public List<Zombie> zombiesLeft = new List<Zombie>(); // all zombies that havenet been spawned yet but are to be
        public List<Zombie> zombiesToAdd = new List<Zombie>(); // all zombies are to be spawned
        public int _numZombiesSpawned;
        double _lastSpawned = 0; // tracks the time between spawning
        float _spawnBuffer = 5f;// time inbertween spawns

        Texture2D _zombieSpriteSheet;
        Texture2D _projectilesSpriteSheet;

        Sprite _sprite;
        List<Rectangle> frames = new List<Rectangle>() {new Rectangle(2,1,5,31), new Rectangle(8,1,14,31), new Rectangle(23, 1, 18, 32), new Rectangle(42, 1, 24, 31), new Rectangle(68, 1, 19, 31)}; //TODO THIS ONLY GOES UP TO 5

        EntityManager _entityManager;

        public bool isRunning { get; set; }
        public WaveManager(Texture2D ZombieSheet, Player player,EntityManager entityManager,Texture2D waveCounterSprite,Texture2D projectiles)
        {
            _entityManager = entityManager;
            _sprite = new Sprite(waveCounterSprite, frames, new Vector2(400, 100));
            _zombieSpriteSheet = ZombieSheet;
            _projectilesSpriteSheet = projectiles;
            isRunning = false; 
        }
        public void NextWave()
        {


            _currentWave++;
            zombiesLeft = _Waves[_currentWave];
            SpawnZombie(5);

        }
       
        public void Update(GameTime gameTime)
        {
            
            //TODO Zombies speed up when new ones are spawned this is because the game starts to run slowly
            if (zombiesToAdd.Count != 0) 
            {
                foreach (Zombie z in zombiesToAdd)
                {
                    zombiesSpawned.Add(z);
                }
                zombiesToAdd.Clear();
            }
            //needs to be outside as while iside a foreach the list cannot be edited
            int index = zombiesSpawned.Count;
            //foreach (Zombie z in zombiesSpawned)
            //{
            //    List<Zombie> zombiesCheckCollsion = zombiesSpawned;
            //    zombiesCheckCollsion.Remove(z);
            //    for (int i = 0; i < index - 1; i++)
            //    {
            //        if (zombiesCheckCollsion[i].CollisionBox.Intersects(z.CollisionBox))
            //        {
            //            z.Separate(zombiesCheckCollsion[i].Position); 
            //        }
            //    }
            //}
            
            //if (zombiesLeft.Count != 0)//& (_numZombiesSpawned != 0) )
            //{
            //    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    if ((gameTime.TotalGameTime.TotalSeconds - lastSpawned) >= spawnBuffer)
            //    {


            //        SpawnZombie(1);
            //        lastSpawned = gameTime.TotalGameTime.TotalSeconds;
            //    }
            //}
            //Separate(zombiesSpawned);

        }
        /// <summary>
        /// Compiles all zombies that have been spawned in and draws them
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            
            _sprite.Frame = _currentWave;
            _sprite.Update(gameTime, new Vector2(400f, 50f));
            _sprite.Draw(spriteBatch, gameTime,2.5f);
            
        }
        /// <summary>
        /// A set amount of zombies are added to a list that will be spawned when the next Update is called
        /// </summary>
        /// <param name="numZombies"></param>
        public void SpawnZombie(int numZombies)
        {
            for (int i = 0; i < numZombies; i++)
            {
                zombiesToAdd.Add(zombiesLeft[i]);
                _entityManager.AddEntity(zombiesLeft[i]);

            }
            _numZombiesSpawned = zombiesSpawned.Count + zombiesToAdd.Count;
          
            zombiesLeft.RemoveRange(0, numZombies - 1);

        }
        /// <summary>
        /// Moves each zombie in the current wave apart to avoid entity cramming
        /// </summary>
        /// <param name="hoarde"></param>
        public void Separate(List<Zombie> hoarde)
        {
            //List<Zombie> zombiesLeftToSeparate = hoarde; // at the begining before the second call it contains all zombies and only when the zombie's collision box doesn't intersect deoes it remove the zombie from the list.
            //Vector2 sum = new Vector2();

            //foreach (Zombie z in hoarde)
            //{
            //    float separationDistance = 5f;
            //    foreach (Zombie other in hoarde)
            //    {
            //        int count = 0;
            //        float distance = Vector2.Distance(z.Position, other.Position);

            //        if ((distance > 0) && (distance < separationDistance))
            //        {
            //            count++;
            //            Vector2 difference = Vector2.Subtract(z.Position, other.Position);
            //            difference.Normalize();
            //            sum = Vector2.Add(sum, difference); // this way all the movement that the zombie needs to make in order to move away from the other zombies is place into one vector

            //        }
            //        if (count > 0) // this will only change the position of the zombies if z is actually close to anyother zombie
            //        {
            //            z.ApplyForce(sum);
            //        }

            //        //if (z.GetCollision.Intersects(other.GetCollision))
            //        //{
            //        //    Separate(hoarde);   
            //        //} 
            //    }
            //    //zombiesLeftToSeparate.Remove(z);  
            //}

        }/// <summary>
        /// Randomly generates number of zombies in waves using set parameters for each wave. isRunning becomes true
        /// </summary>
        public void Intialise()
        {
            isRunning = true;
            //Wave1 Load
            List<Zombie> Wave1 = new List<Zombie>();
            for (int i = 0; i < 10; i++)
            {
                Zombie b = new Walker(_zombieSpriteSheet, _entityManager, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), zombiesSpawned); //TODO make it so it only updates zombies that are in there
                Wave1.Add(b);
            }
            Zombie zombie = new Captain(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager, _projectilesSpriteSheet);
            Wave1.Add(zombie);
            //Wave2
            List<Zombie> Wave2 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
                Zombie b = new Captain(_zombieSpriteSheet, new Vector2(150, 200), _entityManager, _projectilesSpriteSheet);
                Wave2.Add(b);
            }
            //for (int i = 0; i < 10; i++)
            //{
            //    Zombie z = new Walker(ZombieSheet, ninja, new Vector2(rng.Next(0, 800), rng.Next(480, 700)));
            //    Wave2.Add(z);
            //}
            //Wave3
            List<Zombie> Wave3 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
                Zombie b = new Screamer(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager);
                Wave3.Add(b);
            }
            Zombie z = new Captain(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager, _projectilesSpriteSheet);
            Wave3.Add(z);
            //etc.

            //Gathering all waves




            _Waves.Add(Wave3);
            _Waves.Add(Wave2);
            zombiesLeft = _Waves[_currentWave];
            SpawnZombie(3);
        }
       




    }
}



