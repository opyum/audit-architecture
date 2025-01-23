public class Program
{
    public static async Task Main(string[] args)
    {
        var service = new UnreliableService();
        var circuitBreaker = new CircuitBreaker(failureThreshold: 3, openDuration: TimeSpan.FromSeconds(30));
        bool isOk = false;
        int maxTries = 10;
        int currentCount = 0;
        while (isOk || currentCount <= maxTries) 
        {
            currentCount++;
            try
            {
                string result = await circuitBreaker.ExecuteAsync(() => service.GetDataAsync());
                Console.WriteLine($"Succès: {result}");
                isOk = true; 
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur détectée: {ex.Message}");
                await Task.Delay(1000);  // Attente entre les tentatives
            }

        }
    }
}
