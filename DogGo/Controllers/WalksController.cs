using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGo.Repositories;
using DogGo.Models;
using DogGo.Models.ViewModels;


namespace DogGo.Controllers
{
    
    public class WalksController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly IDogRepository _dogRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalksController(IWalkerRepository walkerRepository, IWalkRepository walkRepository, IDogRepository dogRepository)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;
            _dogRepo = dogRepository;

        }
        // GET: WalksController
        public ActionResult Index()
        {
            List<Walks> walks = _walkRepo.GetAllWalks();
            return View(walks);
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
           
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create()
        {
            List<Dog> Dog = _dogRepo.GetAllDogs();
            List<Walker> Walker = _walkerRepo.GetAllWalkers();

            WalksViewModel vm = new WalksViewModel
            {
                Dog = Dog,
                Walker = Walker,
                Walk = new Walks()
            };
            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walks Walk)
        {
            try { 
            int counter = 0;
            foreach (int dog in Walk.Dogs)
            {
                _walkRepo.AddWalks(Walk, counter);
                counter++;
            }
            return RedirectToAction("Index");
        }
            catch
            {
                List<Dog> Dog = _dogRepo.GetAllDogs();
                List<Walker> Walker = _walkerRepo.GetAllWalkers();

                WalksViewModel vm = new WalksViewModel
                {
                    Dog = Dog,
                    Walker = Walker,
                    Walk = new Walks()
                };
                return View(vm);
            }


        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
