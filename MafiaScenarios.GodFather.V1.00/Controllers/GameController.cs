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
            var names = _game.Players
                .Select(p => p.Name).ToList();
            return View(names);
        }

        public IActionResult ShowCards()
        {
            var cards = _game.GetCards()
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

        public IActionResult AssigneCards()
        {
            _game.AssigneRandomCardToPlayer();
            ViewData["Message"] = "نقش های توضیع شدند";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult PlayerCard(string playerName)
        {
            var result=_game.GetPlayerCard(playerName);
            if (result.IsSuccess)
                return Json(new
                {
                    IsSucces = true,
                    Data = new
                    {
                        Title = result.Data!.Title,
                        Side = result.Data!.Side,
                        Describtion = result.Data!.Describtion,
                        PicPath=result.Data!.PicPath,
                    }
                });
            return Json(new
            {
                IsSuccess = false,
                Message = result.Message
            });
        }

        public IActionResult InputPlayerNames()
        {
            var vm= _game.Players.Select(n=>new GetPlayerNamesVM
            {
                Name=n.Name
            }).ToList();
            return View(vm);
        }

        [HttpPost]
        public IActionResult InputPlayerNames(List<GetPlayerNamesVM> vm) 
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
