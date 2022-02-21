using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using crudelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace crudelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            List<Dish> AllDishes = _context.Dishes.OrderByDescending(d => d.DishId).ToList();
            ViewBag.AllDishes = AllDishes;
            return View();
        }
        [Route("new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Process(Dish newDish)
        {
            if(ModelState.IsValid)
            {
                _context.Add(newDish);
                _context.SaveChanges();
                return RedirectToAction("Index");
            } else {
                return View("new");
            }
        }

        [HttpGet("{dishid}")]
        public IActionResult Show(int dishid)
        {
            Dish dishToShow = _context.Dishes.FirstOrDefault(a => a.DishId == dishid);
            ViewBag.dishToShow = dishToShow;
            return View(dishToShow);
        }

        [HttpGet("edit/{dishid}")]
        public IActionResult Edit(int dishid)
        {
            Dish dishToEdit = _context.Dishes.FirstOrDefault(a => a.DishId == dishid);
            return View(dishToEdit);
        }

        [HttpPost("update/{dishid}")]
        public IActionResult Update(int dishid, Dish editedDish)
        {
            Dish old = _context.Dishes.FirstOrDefault(a => a.DishId == dishid);
            old.Name = editedDish.Name;
            old.Chef = editedDish.Chef;
            old.Calories = editedDish.Calories;
            old.Tastiness = editedDish.Tastiness;
            old.Description = editedDish.Description;
            old.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("delete/{dishid}")]
        public IActionResult deleteOne(int dishid)
        {
            Dish dishToDelete = _context.Dishes.SingleOrDefault(a => a.DishId == dishid);
            _context.Dishes.Remove(dishToDelete);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
