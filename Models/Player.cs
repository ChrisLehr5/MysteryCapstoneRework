using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MysteryCapstoneRework.Models
{
    public class Player : Character
    {
        #region FIELDS        

        private List<Location> _locationsVisited;
        private List<Npc> _npcsEngaged;
        private PerceiveModeName _perceiveMode;
        private ObservableCollection<GameItemQuantity> _inventory;
        private ObservableCollection<GameItemQuantity> _potions;
        private ObservableCollection<GameItemQuantity> _mundaneItem;
        private ObservableCollection<GameItemQuantity> _keys;
        private ObservableCollection<Journal> _journals;

        #endregion

        #region PROPERTIES

        public ObservableCollection<Journal> Journals
        {
            get { return _journals; }
            set { _journals = value; }
        }

        public PerceiveModeName PerceiveMode
        {
            get { return _perceiveMode; }
            set { _perceiveMode = value; }
        }

        public List<Location> LocationsVisited
        {
            get { return _locationsVisited; }
            set { _locationsVisited = value; }
        }

        public List<Npc> NpcsEngaged
        {
            get { return _npcsEngaged; }
            set { _npcsEngaged = value; }
        }

        public ObservableCollection<GameItemQuantity> Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public ObservableCollection<GameItemQuantity> Potions
        {
            get { return _potions; }
            set { _potions = value; }
        }

        public ObservableCollection<GameItemQuantity> MundaneItem
        {
            get { return _mundaneItem; }
            set { _mundaneItem = value; }
        }

        public ObservableCollection<GameItemQuantity> Keys
        {
            get { return _keys; }
            set { _keys = value; }
        }


        #endregion

        #region CONSTRUCTORS

        public Player()
        {
            _locationsVisited = new List<Location>();
            _npcsEngaged = new List<Npc>();
            _mundaneItem = new ObservableCollection<GameItemQuantity>();
            _potions = new ObservableCollection<GameItemQuantity>();
            _keys = new ObservableCollection<GameItemQuantity>();
            _journals = new ObservableCollection<Journal>();
        }

        #endregion

        #region METHODS
        public void UpdateJournalStatus()
        {
            foreach (Journal journal in _journals.Where(m => m.Status == Journal.JournalStatus.Incomplete))
            {
                if (journal is JournalTravel)
                {
                    if (((JournalTravel)journal).LocationsNotCompleted(_locationsVisited).Count == 0)
                    {
                        journal.Status = Journal.JournalStatus.Complete;
                    }
                }
                else
                {
                    throw new Exception("Unknown journal.");
                }
            }
        }

        //update the game item list

        public void UpdateInventoryCategories()
        {
            Potions.Clear();
            MundaneItem.Clear();
            Keys.Clear();

            foreach (var gameItemQuantity in _inventory)
            {
                if (gameItemQuantity.GameItem is Potion) Potions.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is MundaneItem) MundaneItem.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is Key) Keys.Add(gameItemQuantity);
            }
        }

        /// <summary>
        /// add selected item to inventory or update quantity if already in inventory
        /// </summary>
        /// <param name="selectedGameItemQuantity">selected item</param>
        public void AddGameItemQuantityToInventory(GameItemQuantity selectedGameItemQuantity)
        {
            //
            // locate selected item in inventory
            //
            GameItemQuantity gameItemQuantity = _inventory.FirstOrDefault(i => i.GameItem.Id == selectedGameItemQuantity.GameItem.Id);

            if (gameItemQuantity == null)
            {
                GameItemQuantity newGameItemQuantity = new GameItemQuantity();
                newGameItemQuantity.GameItem = selectedGameItemQuantity.GameItem;
                newGameItemQuantity.Quantity = 1;

                _inventory.Add(newGameItemQuantity);
            }
            else
            {
                gameItemQuantity.Quantity++;
            }

            UpdateInventoryCategories();
        }

        /// <param name="selectedGameItemQuantity">selected item</param>
        public void RemoveGameItemQuantityFromInventory(GameItemQuantity selectedGameItemQuantity)
        {
            //
            // locate selected item in inventory
            //
            GameItemQuantity gameItemQuantity = _inventory.FirstOrDefault(i => i.GameItem.Id == selectedGameItemQuantity.GameItem.Id);

            if (gameItemQuantity != null)
            {
                if (selectedGameItemQuantity.Quantity == 1)
                {
                    _inventory.Remove(gameItemQuantity);
                }
                else
                {
                    gameItemQuantity.Quantity--;
                }
            }

            UpdateInventoryCategories();
        }


        //check to see if visited
        public bool HasVisited(Location location)
        {
            return _locationsVisited.Contains(location);
        }

        #endregion

    }
}