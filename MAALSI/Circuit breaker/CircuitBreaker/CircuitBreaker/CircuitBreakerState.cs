public enum CircuitBreakerState
{
    Closed,     // Le circuit est fermé (normal)
    Open,       // Le circuit est ouvert (les appels sont bloqués)
    HalfOpen    // Le circuit est en test
}
