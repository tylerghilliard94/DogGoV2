using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walks> GetAllWalksById(int id);
    }
}