namespace GamesManagementSystem.Domain.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GameDevice> GameDevices { get; set; } = [];
    }
}
