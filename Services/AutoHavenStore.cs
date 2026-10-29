using System.Security.Cryptography;
using System.Text.Json;
using AutoHaven.Models;

namespace AutoHaven.Services;

public class AutoHavenStore
{
    private readonly string _folder;
    private readonly string _carsFile;
    private readonly string _usersFile;
    private readonly string _inquiriesFile;
    private readonly object _gate = new();
    private readonly JsonSerializerOptions _json = new() { WriteIndented = true };

    public AutoHavenStore(IWebHostEnvironment env)
    {
        _folder = Path.Combine(env.ContentRootPath, "AutoHaven_Data");
        Directory.CreateDirectory(_folder);
        _carsFile = Path.Combine(_folder, "cars.json");
        _usersFile = Path.Combine(_folder, "users.json");
        _inquiriesFile = Path.Combine(_folder, "inquiries.json");
        if (!File.Exists(_carsFile)) Save(_carsFile, SeedCars());
        if (!File.Exists(_usersFile)) Save(_usersFile, new List<CustomerAccount>());
        if (!File.Exists(_inquiriesFile)) Save(_inquiriesFile, new List<ShowroomInquiry>());
    }

    public List<Car> GetCars() { lock (_gate) return Read<List<Car>>(_carsFile) ?? new(); }
    public Car? GetCar(int id) => GetCars().FirstOrDefault(c => c.Id == id);
    public List<ShowroomInquiry> GetInquiries() { lock (_gate) return Read<List<ShowroomInquiry>>(_inquiriesFile) ?? new(); }

    public bool Register(string name, string email, string password)
    {
        lock (_gate)
        {
            var users = Read<List<CustomerAccount>>(_usersFile) ?? new();
            if (users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))) return false;
            users.Add(new CustomerAccount { Name = name.Trim(), Email = email.Trim(), PasswordHash = HashPassword(password) });
            Save(_usersFile, users); return true;
        }
    }

    public CustomerAccount? Login(string email, string password)
    {
        lock (_gate)
        {
            var user = (Read<List<CustomerAccount>>(_usersFile) ?? new()).FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return user != null && VerifyPassword(password, user.PasswordHash) ? user : null;
        }
    }

    public ShowroomInquiry AddInquiry(ShowroomInquiry inquiry)
    {
        lock (_gate)
        {
            var list = Read<List<ShowroomInquiry>>(_inquiriesFile) ?? new();
            inquiry.Id = list.Count == 0 ? 1 : list.Max(i => i.Id) + 1;
            inquiry.CreatedAt = DateTime.Now;
            list.Add(inquiry); Save(_inquiriesFile, list); return inquiry;
        }
    }

    private T? Read<T>(string path) => File.Exists(path) ? JsonSerializer.Deserialize<T>(File.ReadAllText(path)) : default;
    private void Save<T>(string path, T data) => File.WriteAllText(path, JsonSerializer.Serialize(data, _json));
    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 120000, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }
    private static bool VerifyPassword(string password, string stored)
    {
        try { var parts = stored.Split('.'); var salt = Convert.FromBase64String(parts[0]); var expected = Convert.FromBase64String(parts[1]); var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, 120000, HashAlgorithmName.SHA256, 32); return CryptographicOperations.FixedTimeEquals(actual, expected); }
        catch { return false; }
    }

    private static List<Car> SeedCars() => new()
    {
        new() { Id=1, Name="911 Carrera GTS", Brand="Porsche", Category="Sports", Year=2025, Price=23000000, Fuel="Petrol", Transmission="Automatic", Seats=4, Mileage=10, Badge="Performance", ImageUrl="https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=1200&q=85", Description="A driver-focused sports car with unmistakable silhouette, precise handling and everyday usability." },
        new() { Id=2, Name="Huracán EVO", Brand="Lamborghini", Category="Supercar", Year=2024, Price=39000000, Fuel="Petrol", Transmission="Automatic", Seats=2, Mileage=7, Badge="Rare find", ImageUrl="https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=1200&q=85", Description="A dramatic supercar experience shaped by sculpted lines, an immersive cockpit and thrilling performance." },
        new() { Id=3, Name="Range Rover Sport", Brand="Land Rover", Category="SUV", Year=2025, Price=14500000, Fuel="Diesel", Transmission="Automatic", Seats=5, Mileage=11, Badge="Luxury SUV", ImageUrl="https://images.unsplash.com/photo-1519641471654-76ce0107ad1b?auto=format&fit=crop&w=1200&q=85", Description="Refined luxury, confident road presence and a beautifully considered cabin for every journey." },
        new() { Id=4, Name="M4 Competition", Brand="BMW", Category="Coupe", Year=2024, Price=15500000, Fuel="Petrol", Transmission="Automatic", Seats=4, Mileage=9, Badge="Driver's pick", ImageUrl="https://images.unsplash.com/photo-1555215695-3004980ad54e?auto=format&fit=crop&w=1200&q=85", Description="A performance coupe with bold styling, responsive power and a cockpit built around the driver." },
        new() { Id=5, Name="C-Class AMG Line", Brand="Mercedes-Benz", Category="Sedan", Year=2025, Price=7200000, Fuel="Petrol", Transmission="Automatic", Seats=5, Mileage=14, Badge="Executive", ImageUrl="https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?auto=format&fit=crop&w=1200&q=85", Description="An elegant executive sedan combining modern comfort, premium materials and contemporary technology." },
        new() { Id=6, Name="Mustang GT", Brand="Ford", Category="Muscle", Year=2024, Price=8500000, Fuel="Petrol", Transmission="Manual", Seats=4, Mileage=8, Badge="Iconic", ImageUrl="https://images.unsplash.com/photo-1584345604476-8ec5e12e42dd?auto=format&fit=crop&w=1200&q=85", Description="A modern muscle-car icon with a long-hood profile and unmistakable performance character." },
        new() { Id=7, Name="Model Y Long Range", Brand="Tesla", Category="Electric", Year=2025, Price=6800000, Fuel="Electric", Transmission="Automatic", Seats=5, Mileage=0, Badge="Electric", ImageUrl="https://images.unsplash.com/photo-1560958089-b8a1929cea89?auto=format&fit=crop&w=1200&q=85", Description="A practical all-electric SUV with minimalist design, generous cargo space and quiet acceleration." },
        new() { Id=8, Name="Continental GT", Brand="Bentley", Category="Grand Tourer", Year=2024, Price=42000000, Fuel="Petrol", Transmission="Automatic", Seats=4, Mileage=6, Badge="Bespoke luxury", ImageUrl="https://images.unsplash.com/photo-1563720223185-11003d516935?auto=format&fit=crop&w=1200&q=85", Description="Hand-finished grand touring luxury designed for effortless long-distance travel." }
    };
}
