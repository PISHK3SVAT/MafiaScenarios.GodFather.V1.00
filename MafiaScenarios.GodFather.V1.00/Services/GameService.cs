using System.Linq;
using System.Xml.Linq;
using MafiaScenarios.GodFather.V1._00.Models.Data;

namespace MafiaScenarios.GodFather.V1._00.Services
{
    public class GameService
    {
        public List<string> PlayerNames = new();
        private readonly string _path;
        public const int PLAYERCOUNT= 11;
        public GameService(string path)
        {
            _path = path;
            for (int i=0;i<PLAYERCOUNT; i++)
                PlayerNames.Add(string.Empty);
        }

        public GameServiceResultDto SetPlayerNames(List<string> playerNames)
        {
            playerNames=playerNames.Distinct().ToList();

            if (playerNames.Count() != PLAYERCOUNT)
                return new GameServiceResultDto(false, "تعداد بازیکنان باید 11 عدد باشد و همچنین نام تکراری مجاز نیست");

            PlayerNames.Clear();
            playerNames.ForEach(x => PlayerNames.Add(x));
            return new GameServiceResultDto(true, "ثبت نام بازیکنان با موفقیت انجام شد");
        }
        
        public List<Role> GetRoles()
        {
            var rolesXml = XElement.Load(_path);
            var roles = rolesXml.Elements("Role")
                .Select(r => new Role
                {
                    Title = r.Element("title")!.Value,
                    Side = r.Attribute("side")!.Value,
                    Describtion = r.Element("describtion")!.Value,
                    PicPath = r.Element("picPath")!.Value,
                }).ToList();
            return roles;
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
}
