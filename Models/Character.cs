using MysteryCapstoneRework.UtilityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryCapstoneRework.Models
{
    public class Character : ObservableObject
    {
        #region ENUMERABLES

        #endregion

        #region FIELDS

        private int _id;
        private string _name;
        private int _level;


        #endregion

        #region PROPERTIES    


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Age
        {
            get { return _level; }
            set { _level = value; }
        }


        #endregion

        #region CONSTRUCTORS

        public Character()
        {

        }

        public Character(int id, string name)
        {
            _name = name;
            // _locationId = locationId;
        }

        protected Random random = new Random();

        #endregion

        #region METHODS

        public virtual string DefaultGreeting()
        {
            return $"Hello, my name is {_name}.";
        }

        #endregion
    }
}
