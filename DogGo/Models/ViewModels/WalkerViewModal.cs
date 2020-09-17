using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerViewModal
    {
        public Walker Walker { get; set; }

        public List<Walks> Walks { get; set; }
       

        
        private string getTotalDuration()
        {
            double Total = 0;
            double TotalHours = 0;
            double TotalMinutes = 0;
            foreach(Walks walk in Walks)
            {
                Total += walk.DurationMin;
                TotalHours = Math.Floor(Total / 60);
                TotalMinutes = Total % 60;

            }

            string TotalWalkDuration = $"{TotalHours}hrs {TotalMinutes}mins";

            return TotalWalkDuration;
        }
        public string TotalDuration { get{ return getTotalDuration(); } }
            

        

    }
}
