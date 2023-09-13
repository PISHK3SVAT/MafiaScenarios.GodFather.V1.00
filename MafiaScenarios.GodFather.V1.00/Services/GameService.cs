using System.Data;
using System.Linq;
using System.Xml.Linq;
using MafiaScenarios.GodFather.V1._00.Models.Data;

namespace MafiaScenarios.GodFather.V1._00.Services
{
    public class GameService
    {
        public List<Player> Players = new();
        private readonly string _path;
        public const int PLAYERCOUNT= 11;
        public GameService(string path)
        {
            _path = path;
            for (int i=0;i<PLAYERCOUNT; i++)
                Players.Add(new Player(string.Empty));
        }

        public GameServiceResultDto SetPlayerNames(List<string> playerNames)
        {
            playerNames=playerNames.Distinct().ToList();

            if (playerNames.Count() != PLAYERCOUNT)
                return new GameServiceResultDto(false, "تعداد بازیکنان باید 11 عدد باشد و همچنین نام تکراری مجاز نیست");

            Players.Clear();
            playerNames.ForEach(x => Players.Add(new Player(x)));
            return new GameServiceResultDto(true, "ثبت نام بازیکنان با موفقیت انجام شد");
        }
        
        public List<RoleCard> GetRoleCards()
        {
            var rolesXml = XElement.Load(_path);
            var roles = rolesXml.Elements("Role")
                .Select(r => new RoleCard
                {
                    Title = r.Element("title")!.Value,
                    Side = r.Attribute("side")!.Value,
                    Describtion = r.Element("describtion")!.Value,
                    PicPath = r.Element("picPath")!.Value,
                }).ToList();
            return roles;
        }

        public GameServiceResultDto<RoleCard> GetPlayerCard(string playerName)
        {
            var player = Players.FirstOrDefault(p=>p.Name==playerName);
            if (player == null)
                return new GameServiceResultDto<RoleCard>(false, "این بازیکن وجود ندارد", new RoleCard());
            if (player.Card == null)
                return new GameServiceResultDto<RoleCard>(false, "این بازیکن کارتی ندارد!", new RoleCard());
            return new GameServiceResultDto<RoleCard>(true, "", player!.Card!);
        }

        public void AssigneRandomRoleCardToPlayer()
        {
            var cards=GetRoleCards();
            var randomIndex = new Random();
            for (int i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                var roleIndex = randomIndex.Next(0, cards.Count);
                var role = cards[roleIndex];
                player.Card = role;
                cards.Remove(role);
            }
        }
    }
    public class GameServiceResultDto
    {
        public GameServiceResultDto(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class GameServiceResultDto<TData>
    {
        public GameServiceResultDto(bool isSuccess, string message,TData data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}
