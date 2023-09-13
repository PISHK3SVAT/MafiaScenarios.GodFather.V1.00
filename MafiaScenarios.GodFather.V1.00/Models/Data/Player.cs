namespace MafiaScenarios.GodFather.V1._00.Models.Data
{
    public class Player
    {
        public string Name { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public Card? Card { get; set; }
    }
}
