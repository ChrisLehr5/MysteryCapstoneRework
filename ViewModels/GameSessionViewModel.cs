using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysteryCapstoneRework.UtilityClass;
using MysteryCapstoneRework.Models;
using MysteryCapstoneRework.BusinessLayer;

namespace MysteryCapstoneRework.ViewModels
{
    public class GameSessionViewModel : ObservableObject
    {

        #region CONSTANTANTS

        const string TAB = "\t";
        const string NEW_LINE = "\n";

        #endregion

        private Player _player;
        private Player _selectedPlayer;
        private string _searchText;
        private PlayerBusiness _playerBusiness;
        private Map _gameMap;
        private Location _currentLocation;
        private Location _northLocation, _eastLocation, _southLocation, _westLocation;
        private string _currentLocationInformation;
        private GameItemQuantity _currentGameItem;
        private Npc _currentNpc;
        private Random random = new Random();
        private string _errorMessage;


        public Player UserPlayer { get; set; }

        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));

            }
        }


        public Player Player
        {
            get { return _player; }
            set
            {
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string MessageDisplay
        {
            get { return _currentLocation.Message; }
        }

        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                _currentLocationInformation = _currentLocation.Description;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }

        //
        // expose information about travel points from current location
        //
        public Location NorthLocation
        {
            get { return _northLocation; }
            set
            {
                _northLocation = value;
                OnPropertyChanged(nameof(NorthLocation));
                OnPropertyChanged(nameof(HasNorthLocation));
            }
        }

        public Location EastLocation
        {
            get { return _eastLocation; }
            set
            {
                _eastLocation = value;
                OnPropertyChanged(nameof(EastLocation));
                OnPropertyChanged(nameof(HasEastLocation));
            }
        }

        public Location SouthLocation
        {
            get { return _southLocation; }
            set
            {
                _southLocation = value;
                OnPropertyChanged(nameof(SouthLocation));
                OnPropertyChanged(nameof(HasSouthLocation));
            }
        }

        public Location WestLocation
        {
            get { return _westLocation; }
            set
            {
                _westLocation = value;
                OnPropertyChanged(nameof(WestLocation));
                OnPropertyChanged(nameof(HasWestLocation));
            }
        }


        public string CurrentLocationInformation
        {
            get { return _currentLocationInformation; }
            set
            {
                _currentLocationInformation = value;
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }

        public bool HasNorthLocation
        {
            get
            {
                if (NorthLocation != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //
        // shortened code with same functionality as above
        //
        public bool HasEastLocation { get { return EastLocation != null; } }
        public bool HasSouthLocation { get { return SouthLocation != null; } }
        public bool HasWestLocation { get { return WestLocation != null; } }

        public void HelpWindow()
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }
        public GameItemQuantity CurrentGameItem
        {
            get { return _currentGameItem; }
            set
            {
                _currentGameItem = value;
                OnPropertyChanged(nameof(CurrentGameItem));
            }
        }

        public Npc CurrentNpc
        {
            get { return _currentNpc; }
            set
            {
                _currentNpc = value;
                OnPropertyChanged(nameof(CurrentNpc));
            }
        }

        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(
            Player player,
            Map gameMap,
            GameMapCoordinates currentLocationCoordinates)
        {
            _player = player;
            _gameMap = gameMap;
            _gameMap.CurrentLocationCoordinates = currentLocationCoordinates;
            _currentLocation = _gameMap.CurrentLocation;
            InitializeView();
        }

        /// <summary>
        /// initial setup of the game session view
        /// </summary>
        private void InitializeView()
        {
            UpdateAvailableTravelPoints();
            _currentLocationInformation = CurrentLocation.Description;
            _player.UpdateInventoryCategories();

        }

        /// <summary>
        /// calculate the available travel points from current location
        /// game slipstreams are a mapping against the 2D array where 
        /// </summary>
        private void UpdateAvailableTravelPoints()
        {
            //
            // reset travel location information
            //
            NorthLocation = null;
            EastLocation = null;
            SouthLocation = null;
            WestLocation = null;

            //
            // north location exists
            //
            if (_gameMap.NorthLocation() != null)
            {
                Location nextNorthLocation = _gameMap.NorthLocation();

                //
                // location generally accessible or player has required conditions
                //
                if (nextNorthLocation.Accessible == true) //|| PlayerCanAccessLocation(nextNorthLocation))
                {
                    NorthLocation = nextNorthLocation;
                }
            }

            //
            // east location exists
            //
            if (_gameMap.EastLocation() != null)
            {
                Location nextEastLocation = _gameMap.EastLocation();

                //
                // location generally accessible or player has required conditions
                //
                if (nextEastLocation.Accessible == true) //|| PlayerCanAccessLocation(nextEastLocation))
                {
                    EastLocation = nextEastLocation;
                }
            }

            //
            // south location exists
            //
            if (_gameMap.SouthLocation() != null)
            {
                Location nextSouthLocation = _gameMap.SouthLocation();

                //
                // location generally accessible or player has required conditions
                //
                if (nextSouthLocation.Accessible == true)// || PlayerCanAccessLocation(nextSouthLocation))
                {
                    SouthLocation = nextSouthLocation;
                }
            }

            //
            // west location exists
            //
            if (_gameMap.WestLocation() != null)
            {
                Location nextWestLocation = _gameMap.WestLocation();

                //
                // location generally accessible or player has required conditions
                //
                if (nextWestLocation.Accessible == true)// || PlayerCanAccessLocation(nextWestLocation))
                {
                    WestLocation = nextWestLocation;
                }
            }
        }


        /// <summary>
        /// player move event handler
        /// </summary>
        private void OnPlayerMove()
        {
            //
            // update player stats when in new location
            //
            if (!_player.HasVisited(_currentLocation))
            {
                //
                // add location to list of visited locations
                //
                _player.LocationsVisited.Add(_currentLocation);

                //
                // display a new message if available
                //
                OnPropertyChanged(nameof(MessageDisplay));

            }
        }

        /// <summary>
        /// travel north
        /// </summary>
        public void MoveNorth()
        {
            if (HasNorthLocation)
            {
                _gameMap.MoveNorth();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                //_player.UpdateJournalStatus();
            }
        }

        /// <summary>
        /// travel east
        /// </summary>
        public void MoveEast()
        {
            if (HasEastLocation)
            {
                _gameMap.MoveEast();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                // _player.UpdateJournalStatus();
            }
        }

        /// <summary>
        /// travel south
        /// </summary>
        public void MoveSouth()
        {
            if (HasSouthLocation)
            {
                _gameMap.MoveSouth();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                // _player.UpdateJournalStatus();
            }
        }

        /// <summary>
        /// travel west
        /// </summary>
        public void MoveWest()
        {
            if (HasWestLocation)
            {
                _gameMap.MoveWest();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                // _player.UpdateJournalStatus();

            }
        }

        /// <summary>
        /// add a new item to the players inventory
        /// </summary>
        /// <param name="selectedItem"></param>
        public void AddItemToInventory()
        {
            //
            // confirm a game item selected and is in current location
            // subtract from location and add to inventory
            //
            if (_currentGameItem != null && _currentLocation.GameItems.Contains(_currentGameItem))
            {
                //
                // cast selected game item 
                //
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.RemoveGameItemQuantityFromLocation(selectedGameItemQuantity);
                _player.AddGameItemQuantityToInventory(selectedGameItemQuantity);

                OnPlayerPickUp(selectedGameItemQuantity);
            }
        }

        /// <summary>
        /// process events when a player picks up a new game item
        /// </summary>
        /// <param name="gameItemQuantity">new game item</param>
        private void OnPlayerPickUp(GameItemQuantity gameItemQuantity)
        {

        }

        /// <summary>
        /// process events when a player puts down a new game item
        /// </summary>
        /// <param name="gameItemQuantity">new game item</param>
        private void OnPlayerPutDown(GameItemQuantity gameItemQuantity)
        {

        }

        /// <summary>
        /// remove item from the players inventory
        /// </summary>
        /// <param name="selectedItem"></param>
        public void RemoveItemFromInventory()
        {
            //
            // confirm a game item selected and is in inventory
            // subtract from inventory and add to location
            //
            if (_currentGameItem != null)
            {
                //
                // cast selected game item 
                //
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.AddGameItemQuantityToLocation(selectedGameItemQuantity);
                _player.RemoveGameItemQuantityFromInventory(selectedGameItemQuantity);

                OnPlayerPutDown(selectedGameItemQuantity);
            }
        }

        /// <summary>
        /// process using an item in the player's inventory
        /// </summary>
        public void OnUseGameItem()
        {
            switch (_currentGameItem.GameItem)
            {
                //case Potion potion:
                //   ProcessPotionUse(potion);
                //   break;
                case Key key:
                    ProcessKeyUse(key);
                    break;
                case MundaneItem mundaneItem:
                    ProcessMundaneItemUse(mundaneItem);
                    break;
                default:
                    break;
            }
        }

        public void InspectTheItem()
        {
            string message;

            message = CurrentGameItem.GameItem.Inspect;
            CurrentLocationInformation = message;
        }

        /// <summary>
        /// process the effects of using the key
        /// </summary>
        /// <param name="key">key</param>
        private void ProcessKeyUse(Key key)
        {
            string message;

            switch (key.UseAction)
            {
                case Key.UseActionType.OPENLOCATION:
                    message = _gameMap.OpenLocationsByKey(key.Id);
                    CurrentLocationInformation = key.UseMessage;
                    break;
                case Key.UseActionType.KILLPLAYER:
                    OnPlayerDies(key.UseMessage);
                    break;
                case Key.UseActionType.PLAYERWIN:
                    OnPlayerWins(key.UseMessage);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// process the effects of using the treasure
        /// </summary>
        /// <param name="treasure">treasure</param>
        private void ProcessMundaneItemUse(MundaneItem mundaneItem)
        {
            string message;

            switch (mundaneItem.UseAction)
            {
                case MundaneItem.UseActionType.OPENLOCATION:
                    message = _gameMap.OpenLocationsByMundaneItem(mundaneItem.Id);
                    CurrentLocationInformation = mundaneItem.UseMessage;
                    break;
                case MundaneItem.UseActionType.KILLPLAYER:
                    OnPlayerDies(mundaneItem.UseMessage);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// process player dies with option to reset and play again
        /// </summary>
        /// <param name="message">message regarding player death</param>
        private void OnPlayerDies(string message)
        {
            string messagetext = message +
                "Better luck next time!";

            string titleText = "Death";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(messagetext, titleText, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetPlayer();
                    break;
                case MessageBoxResult.No:
                    QuiteApplication();
                    break;
            }
        }

        // <summary>
        /// process player dies with option to reset and play again
        /// </summary>
        /// <param name="message">message regarding player death</param>
        private void OnPlayerWins(string message)
        {
            string messagetext = message +
                "Congrats! Hope you play again soon!";

            string titleText = "Winner";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(messagetext, titleText, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetPlayer();
                    break;
                case MessageBoxResult.No:
                    QuiteApplication();
                    break;
            }
        }

        public void OnPlayerTalkTo()
        {
            if (CurrentNpc != null && CurrentNpc is ISpeak)
            {
                ISpeak speakingNpc = CurrentNpc as ISpeak;
                CurrentLocationInformation = speakingNpc.Speak();
                _player.NpcsEngaged.Add(_currentNpc);

            }
        }

        public void OnPlayerPercieve()
        {
            if (CurrentNpc != null && CurrentNpc is IPerception)
            {
                IPerception perceivingNpc = CurrentNpc as IPerception;
                CurrentLocationInformation = perceivingNpc.Perceive();
                _player.NpcsEngaged.Add(_currentNpc);

            }
        }

        /// <summary>
        /// player chooses to exit game
        /// </summary>
        public void QuiteApplication()
        {
            Environment.Exit(0);
        }


        /// <summary>
        /// player chooses to reset game
        /// </summary>
        private void ResetPlayer()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// open the Journal window
        /// </summary>
        public void OpenJournalStatusView()
        {
            JournalStatusView journalStatusView = new JournalStatusView(InitializeJournalStatusViewModel());

            journalStatusView.Show();
        }

        /// <summary>
        /// initialize all property values for the mission status view model
        /// </summary>
        /// <returns>mission status view model</returns>
        private JournalStatusViewModel InitializeJournalStatusViewModel()
        {
            JournalStatusViewModel journalStatusViewModel = new JournalStatusViewModel();

            journalStatusViewModel.JournalInformation = GenerateJournalStatusInformation();

            journalStatusViewModel.Journals = new List<Journal>(_player.Journals);
            foreach (Journal journal in journalStatusViewModel.Journals)
            {
                if (journal is JournalTravel)
                    journal.StatusDetail = GenerateJournalTravelDetail((JournalTravel)journal);

                //if (mission is MissionEngage)
                //  mission.StatusDetail = GenerateMissionEngageDetail((MissionEngage)mission);

                // if (mission is MissionGather)
                //  mission.StatusDetail = GenerateMissionGatherDetail((MissionGather)mission);
            }

            return journalStatusViewModel;
        }

        /// <summary>
        /// generate the mission status information text based on percentage of missions completed
        /// </summary>
        /// <returns>mission status information text</returns>
        private string GenerateJournalStatusInformation()
        {
            string journalStatusInformation;

            double totalJournals = _player.Journals.Count();
            double journalsCompleted = _player.Journals.Where(m => m.Status == Journal.JournalStatus.Complete).Count();

            int percentJournalsCompleted = (int)((journalsCompleted / totalJournals) * 100);
            journalStatusInformation = $"Journal Entries Discovered: {percentJournalsCompleted}%" + NEW_LINE;

            if (percentJournalsCompleted == 0)
            {
                journalStatusInformation += "No quests complete at this point.";
            }
            else if (percentJournalsCompleted == 50)
            {
                journalStatusInformation += "Half way there!";
            }
            else if (percentJournalsCompleted == 66)
            {
                journalStatusInformation += "So close! You can do it!";
            }
            else if (percentJournalsCompleted == 100)
            {
                journalStatusInformation += "Congratulations, you have completed all quests.";
            }

            return journalStatusInformation;
        }


        /// <summary>
        /// generate the text for a travel mission detail
        /// </summary>
        /// <param name="mission">the mission</param>
        /// <returns>mission detail text</returns>
        private string GenerateJournalTravelDetail(JournalTravel journal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("All Required");
            foreach (var location in journal.RequiredLocations)
            {
                sb.AppendLine(TAB + location.Name);
            }

            if (journal.Status == Journal.JournalStatus.Incomplete)
            {
                sb.AppendLine("No new entries");
                foreach (var location in journal.LocationsNotCompleted(_player.LocationsVisited))
                {
                    sb.AppendLine(TAB + location.Name);
                }
            }

            sb.Remove(sb.Length - 2, 2); // remove the last two characters that generate a blank line

            return sb.ToString(); ;
        }

        /// <summary>
        /// handle the perception to event in the view
        /// </summary>
        public void OnPlayerPerceive()
        {
            if (CurrentNpc != null && CurrentNpc is IPerception)
            {
                IPerception perceiveNpc = CurrentNpc as IPerception;
                CurrentLocationInformation = perceiveNpc.Perceive();
                _player.NpcsEngaged.Add(_currentNpc);
                _player.UpdateJournalStatus();
            }
        }

    }
}
