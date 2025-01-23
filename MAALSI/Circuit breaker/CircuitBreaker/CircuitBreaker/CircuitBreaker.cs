public class CircuitBreaker
{
    private int _failureCount = 0;
    private readonly int _failureThreshold; // Nombre d'échecs avant d'ouvrir le circuit
    private readonly TimeSpan _openDuration; // Temps d'attente avant de tester à nouveau le service
    private DateTime _lastFailureTime;
    private CircuitBreakerState _state;

    public CircuitBreaker(int failureThreshold, TimeSpan openDuration)
    {
        _failureThreshold = failureThreshold;
        _openDuration = openDuration;
        _state = CircuitBreakerState.Closed;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        if (_state == CircuitBreakerState.Open)
        {
            if (DateTime.UtcNow - _lastFailureTime > _openDuration)
            {
                Console.WriteLine("Passage en mode Half-Open pour tester le service...");
                _state = CircuitBreakerState.HalfOpen;
            }
            else
            {
                throw new Exception("Circuit ouvert. Impossible d'appeler le service pour le moment.");
            }
        }

        try
        {
            T result = await action();

            // Succès, réinitialisation du compteur d'échec et retour en mode normal
            _failureCount = 0;
            _state = CircuitBreakerState.Closed;
            Console.WriteLine("Service réussi, circuit fermé.");

            return result;
        }
        catch (Exception)
        {
            _failureCount++;

            if (_failureCount >= _failureThreshold)
            {
                _state = CircuitBreakerState.Open;
                _lastFailureTime = DateTime.UtcNow;
                Console.WriteLine("Circuit ouvert après plusieurs échecs !");
            }
            else if (_state == CircuitBreakerState.HalfOpen)
            {
                _state = CircuitBreakerState.Open;
                Console.WriteLine("Test échoué, retour en circuit ouvert !");
            }

            throw;
        }
    }
}
