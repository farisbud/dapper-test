using Microsoft.AspNetCore.Mvc;
using pusdafi.Interface;
using pusdafi.Models;

namespace pusdafi.Controllers
{
    public class RaceController : Controller
    {
        //private readonly ApplicationDBContext _context;

        private readonly IRaceRepository _raceRepository;

        public RaceController(IRaceRepository raceRepository) {

            //    _context = context;
            _raceRepository = raceRepository;

        }
        public async Task<IActionResult> Index()
        {
            //list menampilkan semua data
            // List<Races>races = _context.Race.ToList();

            IEnumerable < Races > races = await _raceRepository.getAll();
            
            return View(races);
        }

        public async Task<IActionResult> Detail(int id) {
            //list tampil 1 data
            //Races Race = _context.Race.FirstOrDefault(a => a.Id == id);
            //Races Racess = _context.Race.Include(a => a.Address).FirstOrDefault(r => r.Id == id);
            Races races = await _raceRepository.getByIdAsync(id);
            return View(races); 
        
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Races races)
        {
            if (!ModelState.IsValid)
            {
                return View(races);
            }
            _raceRepository.Add(races);
            return RedirectToAction("Index");
        }

    }
}
