using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walks> GetAllWalks();
        List<Walks> GetAllWalksById(int id);

        void AddWalks(Walks walk, int id);

        void DeleteWalks(int id);
    }
}