using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysteryCapstoneRework.Models;

namespace MysteryCapstoneRework.BusinessLayer
{
    public class PlayerBusiness
    {
        #region Properties

        public FileIoMessage fileIOStatus { get; set; }

        #endregion

        #region Constructor
        public PlayerBusiness()
        {
            //SqlUtilities.WriteSeedDataToDatabase();
        }

        #endregion

        #region Methods

        /// <summary>
        /// gets pokemon by ID
        /// </summary>
        private Player GetPlayer(int id)
        {
            Player player = null;
            fileIOStatus = FileIoMessage.None;
            try
            {
                using (PlayerRepository playerrepository = new PlayerRepository())
                {
                    player = playerrepository.GetById(id);
                }

                if (player != null)
                {
                    fileIOStatus = FileIoMessage.Complete;
                }
                else
                {
                    fileIOStatus = FileIoMessage.NoRecordsFound;
                }
            }
            catch (Exception)
            {

                fileIOStatus = FileIoMessage.FileAccessError;
            }

            return player;
        }

        /// <summary>
        /// gets all the pokemon
        /// </summary>
        private List<Player> GetAllPlayer()
        {
            List<Player> player = null;
            fileIOStatus = FileIoMessage.None;

            try
            {
                using (PlayerRepository playerRepository = new PlayerRepository())
                {
                    player = playerRepository.GetAll() as List<Player>;
                }

                if (player != null)
                {
                    fileIOStatus = FileIoMessage.Complete;
                }
                else
                {
                    fileIOStatus = FileIoMessage.NoRecordsFound;
                }
            }
            catch (Exception e)
            {
                var errorMessage = e.Message;
                fileIOStatus = FileIoMessage.FileAccessError;
            }

            return player;
        }

        /// <summary>
        /// retrieves pokemon from seed data or data path
        /// </summary>
        public List<Player> AllPlayer()
        {
            //return GetAllPlayer() as List<Player>;           

            return GetAllPlayer();
        }

        /// <summary>
        /// retrieve a pokemon by id
        /// </summary>
        public Player PlayerByID(int id)
        {
            return GetPlayer(id);
        }

        /// <summary>
        /// add a pokemon
        /// </summary>
        public void AddPlayer(Player player)
        {
            try
            {
                if (player != null)
                {
                    using (PlayerRepository playerRepository = new PlayerRepository())
                    {
                        playerRepository.Add(player);
                    }
                    fileIOStatus = FileIoMessage.Complete;
                }
            }
            catch (Exception)
            {

                fileIOStatus = FileIoMessage.FileAccessError;
            }
        }


        #endregion
    }
}
