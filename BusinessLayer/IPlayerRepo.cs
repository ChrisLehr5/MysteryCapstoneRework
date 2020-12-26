using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysteryCapstoneRework.Models;

namespace MysteryCapstoneRework.BusinessLayer
{
    public interface IPlayerRespository
    {
        IEnumerable<Player> GetAll();
        Player GetById(int id);

    }
}
