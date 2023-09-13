using MafiaScenarios.GodFather.V1._00.Models;
using MafiaScenarios.GodFather.V1._00.Services;

using Microsoft.AspNetCore.Mvc;

namespace MafiaScenarios.GodFather.V1._00.Controllers
{
    public class GameController : Controller
    {
        private GameService _game;

        public GameController(GameService game)
        {
            _game = game;
        }

        public IActionResult Index()
        {
            var m = _game.GetRoles();
            return View(m);
        }

        public IActionResult ShowCards()
        {
            var cards = _game.GetRoles()
                .DistinctBy(r=>r.Title)
                .Select(r => new ShowCardVM
                {
                    Title = r.Title,
                    Describtion = r.Describtion,
                    PicPath = r.PicPath,
                    Side = r.Side
                }).ToList();
            return View(cards);
        }

        public IActionResult GetPlayerNames()
        {
            var vm= _game.PlayerNames.Select(n=>new GetPlayerNamesVM
            {
                Name=n
            }).ToList();
            return View(vm);
        }

        [HttpPost]
        public IActionResult GetPlayerNames(List<GetPlayerNamesVM> vm) 
        {
            var names = vm.Select(x => x.Name).ToList();
            var result=_game.SetPlayerNames(names);
            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", result.Message);
            return View(vm);
        }
    }
}
