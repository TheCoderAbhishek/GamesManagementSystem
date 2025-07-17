namespace GamesManagementSystem.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DevelopedBy { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string? BannerImagePath { get; set; }
        public ICollection<GameDevice> GameDevices { get; set; } = [];
    }
}
