---
layout: two-cols
---
## Traces
Operacje wykonywane przez aplikację

<v-clicks>

- Zapytanie HTTP
- Zdarzenie
- Interakcja użytkownika

</v-clicks>

---
hideInToc: true
transition: fade
---

## .NET Tracing

- `WithTracing()`: opentelemetry-dotnet SDK rejestruje `ActivityListener` do wskazanych `ActivitySource`s

<<< ../../WebAPI/Program.cs#opentelemetry-setup {*|7-15|13}{lines:true}

---
hideInToc: true
---

## .NET Tracing

<div>

- `WithTracing()`: opentelemetry-dotnet SDK rejestruje `ActivityListener` do wskazanych `ActivitySource`s

</div>

<div>

- W aplikacji tworzone jest `ActivitySource`:

```csharp
private static readonly ActivitySource ActivitySource
        = new(typeof(LoggingPipelineBehavior<TRequest, TResponse>).FullName!);
```
</div>

<v-clicks>

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

</v-clicks>

<!--
- sampling
- ActivitySource:
  - factory do tworzenia Activities
  - przestrzeń nazw
  - używa ActivityListenera do notyfikacji w przypadku gdy coś nasłuchuje na dane ActivitySource

- Activity:
  - unit of work
  - używa AsyncLocal<Activity> do przetrzymywania kontekstu pomiędzy wywołaniami asynchronicznymi

-->
---

```csharp
public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
```

<<< ../../WebAPI/Telemetry/LoggingPipelineBehavior.cs#logging-behavior {*|1-2|9-10|12-16|18|*}{lines:true}

<!--

- Czym jest mediatorowy pipeline behavior?
  - porównanie do middleware

-->

---
transition: fade
hideInToc: true
---

## Span
<v-clicks>

|                                   |                                   |
|----------------------------------:|:----------------------------------|
|                          **Name** | Nazwa operacji                    |
|                    **Attributes** | Dodatkowe atrybuty (tagi)         |
|            **Start and duration** | Okno czasowe wykonywania operacji |
|                       Span status | Status operacji                   |
|                         Span kind | Rodzaj (kategoria) operacji       |
|                    Parent span ID | ID rodzica                        |
| TraceId<br/>SpanId<br/>TraceFlags | Trace context                     |
|              InstrumentationScope | Activity source                   |
|                          Resource | Serwis/środowisko generujący span |

</v-clicks>


---
hideInToc: true
---

## Span

|                                   |                                                                                   |
|----------------------------------:|:----------------------------------------------------------------------------------|
|                          **Name** | `Handling IncrementNamedCounter`                                                  |
|                    **Attributes** | `request.CounterId: rabarbar` `request.Delta: 1`                                  |
|            **Start and duration** | `2025-04-12T16:08:16.4385066Z` `00:00:00.4275037`                                 |
|                       Span status | `Unset`                                                                           |
|                         Span kind | `Internal`                                                                        |
|                    Parent span ID | `0907e33699fbd0c7`                                                                |
| TraceId<br/>SpanId<br/>TraceFlags | `230909447214bb90523f50023553b274`<br/>`30dc7cfd9ccf4f0a`<br/>`Recorded`          |
|              InstrumentationScope | `WebAPI.Telemetry.LoggingPipelineBehavior<WebAPI.Handlers.IncrementNamedCounter>` |
|                          Resource | `telemetry.sdk.name: opentelemetry`                                               |
