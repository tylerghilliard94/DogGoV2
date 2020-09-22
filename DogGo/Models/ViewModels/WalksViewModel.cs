using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksViewModel
    {
        public List<Dog> Dog { get; set; }

        public List<Walker> Walker { get; set; }

        public Walks Walk { get; set; }

        public List<Walks> AllWalks { get; set;}

        public List<int> WalkIds { get; set; }
    }
}
