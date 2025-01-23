using System;
using System.Threading.Tasks;

public class UnreliableService
{
    private static Random _random = new Random();

    public async Task<string> GetDataAsync()
    {
        await Task.Delay(500); // Simule un délai réseau

        if (_random.Next(1, 50) > 2)  // Simule une erreur aléatoire
        {
            throw new Exception("Échec du service !");
        }

        return "Réponse du service";
    }
}
