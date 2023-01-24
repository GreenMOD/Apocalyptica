using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Controls;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Overhaul_Of_Apocalyptica
{
    public class SaveSlot
    {
        public string Status { get; set; }
        public string SlotName { get; set; }
        public string PlayerName { get; set; }
        public string PlayerClass { get; set; }
        public int CurrentWave { get; set; }

        private string _filePath;
        /// <summary>
        /// Using a file path a saveslot is instaniated and loads all relavant data into that instance
        /// </summary>
        /// <param name="filePath"></param>
        public SaveSlot(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    SlotName =sr.ReadLine();
                    Status = sr.ReadLine().Substring(8);

                    if (Status == "Used")
                    { 
                        PlayerName= sr.ReadLine().Substring(6);
                        PlayerClass = sr.ReadLine().Substring(7);
                        CurrentWave = int.Parse(sr.ReadLine().Substring(6))-1;

                        //    GameTime gameTime = new GameTime();

                        //    Player player1;
                        //    switch (Class)
                        //    {
                        //        case "Soldier":
                        //            Player soldier = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierSpriteSheet2, gameTime);
                        //            player1 = soldier;
                        //            break;
                        //        case "Ninja":
                        //            Player ninja = new Ninja(_ninjaSpriteSheet, _heartSpriteSheet);
                        //            player1 = ninja;
                        //            break;
                        //        default:
                        //            player1 = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierSpriteSheet2, gameTime);
                        //            break;
                    }
                    else
                    {
                        SlotName = sr.ReadLine();
                        PlayerName = "";
                        PlayerClass = "";
                        CurrentWave = -1;

                    }
                    sr.Close();
                    //    _entityManager.AddEntity(player1);
                    //    _collisionManager.AddCollidable(player1);
                    //    player1.Activate();





                }
                _filePath = filePath;
            }

        }
        /// <summary>
        /// Overrides the file that stores the values and corrects the file.
        /// </summary>
        public void OverrideSave()
        {
            using (StreamWriter sw = new StreamWriter(_filePath)) 
            {
                sw.WriteLine(SlotName);
                sw.WriteLine("Status: " + Status);
                sw.WriteLine("Name: " + PlayerName);
                sw.WriteLine("Class: " + PlayerClass);
                sw.WriteLine("Wave: " + CurrentWave);
            }
        }
    }
}
