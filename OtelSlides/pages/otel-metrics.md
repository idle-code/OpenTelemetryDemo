## Metrics
Liczniki reprezentujące aktualny stan systemu

<v-clicks>

- Liczba obsłużonych zapytań
- Poziom użycia pamięci
- Opóźnienia obsługiwanych zapytań

</v-clicks>

---

## .NET Metrics

<div>

- `WithMetrics()`: opentelemetry-dotnet SDK subskrybuje się do wskazanych `ActivitySource`s



</div>

<div>

- W aplikacji tworzone jest `ActivitySource`:

```csharp
private static readonly ActivitySource ActivitySource
        = new(typeof(LoggingPipelineBehavior<TRequest, TResponse>).FullName!);
```
</div>

<div>

- Jeśli ktoś nasłuchuje na dane `ActivitySource`, wszyscy subskrybenci są informowani o rozpoczęciu/zakończeniu nowego `Activity`:

```csharp
using var activity = ActivitySource.StartActivity($"Handling {requestTypeName}", ActivityKind.Internal);
```

</div>

<div>

- Span jest przetwarzany przez **procesory**
    - Tu następuje **grupowanie** (batching)
    - Na tym etapie span może zostać **odfiltrowany**

</div>

<div>

- Span jest jest wysyłany do **eksportera** (np: OTLP, Console)

</div>
