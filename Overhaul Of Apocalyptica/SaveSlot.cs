using System.IO;


namespace Overhaul_Of_Apocalyptica
{
    public class SaveSlot
    {
        private string _filePath;
        private string _status;
        private string _slotName;
        private string _playerName;
        private string _playerClass;
        private int _currentWave;

        public string Status { get => _status; set => _status = value; }
        public string SlotName { get => _slotName; set => _slotName = value; }
        public string PlayerName { get => _playerName; set => _playerName = value; }
        public string PlayerClass { get => _playerClass; set => _playerClass = value; }
        public int CurrentWave { get => _currentWave; set => _currentWave = value; }

        



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
                    SlotName = sr.ReadLine();
                    Status = sr.ReadLine().Substring(8);

                    if (Status == "Used")
                    {
                        PlayerName = sr.ReadLine().Substring(6);
                        PlayerClass = sr.ReadLine().Substring(7);
                        CurrentWave = int.Parse(sr.ReadLine().Substring(6)) - 1;
                    }
                    else
                    {
                        PlayerName = "";
                        PlayerClass = "";
                        CurrentWave = -1;
                    }
                    sr.Close();
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
