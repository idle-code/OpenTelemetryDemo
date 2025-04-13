## Metrics
Liczniki reprezentujące aktualny stan systemu

<v-clicks>

- Liczba obsłużonych zapytań
- Poziom użycia pamięci
- Opóźnienia obsługiwanych zapytań

</v-clicks>

---

## .NET Metrics

<v-clicks>
<div>

- `WithMetrics()`: opentelemetry-dotnet SDK subskrybuje się do wskazanych mierników via `.AddMeter()`

</div>

<div>

- Przy starcie tworzone są instrumenty takie jak `Counter`:

```csharp
var meter = meterFactory.Create("WebAPI.counter");
_counterIncrements = meter.CreateCounter<int>("WebAPI.counter.increments");
```

</div>

<div>

- Podczas uruchomienia, kod rejestruje zmiany w mertykach:

```csharp
_counterMetrics.CounterIncrement(counter.Id, request.Delta);
```

```csharp
public void CounterIncrement(string counterName, int delta)
{
    _counterIncrements.Add(delta, new[] { new KeyValuePair<string, object?>("counter_name", counterName) });
}
```

</div>

<div>

- **Procesory** są odpowiedzialne za agregację i filtrowanie metryk

</div>

<div>

- Domyślnie co 60 sekund metryki są **eksportowane**
  - PrometheusExporter używa metody pull - via `/metrics` endpoint

</div>

</v-clicks>

<!--
- Metryki są zapisywane w pamięci
-->
