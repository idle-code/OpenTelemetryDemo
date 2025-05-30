---
layout: two-cols
---

## Traces
Operacje wykonywane przez aplikację

<v-clicks>

- Zapytanie HTTP
- Obsługa zdarzenia
- Interakcja użytkownika

</v-clicks>

<v-click>

<h2>Spans</h2>
Operacje wykonywane przez aplikację

</v-click>

<v-clicks>

- Zapytanie HTTP
- Obsługa zdarzenia
- Interakcja użytkownika
- Zapytanie do bazy danych
- Wywołanie serwisu wewnętrznego
- MediatR handler

</v-clicks>

<!--
- Unit Of Work
- Interakcje
-
-->

---
hideInToc: true
transition: fade
---

## .NET Tracing

- `WithTracing()`: opentelemetry-dotnet SDK rejestruje `ActivityListener` do wskazanych `ActivitySource`s

<<< ../../WebAPI/Program.cs#opentelemetry-setup {*|7-16|13|*}{lines:true}

<!--
- AddSource jest ważne w przypadku tworzenia własnych Activity(Source)
-->

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

<<< ../../WebAPI/Telemetry/LoggingPipelineBehavior.cs#logging-behavior {*|1-2|5,9-10|12-16|18|*}{lines:true}

<!--
- Czym jest mediatorowy pipeline behavior?
  - porównanie do middleware
  - router requestów
- Gdzie/kiedy tworzyć ActivitySource?
- Best practice: static, per-(sub)module
-->

---

<img src="./ai_handling_span.png">

<!--
1 - Wizualizacja w drzewie
2 - doklejone tagi
3 - nazwa
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

<!--
- Trace = drzewo spanów
-->

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

<!--
- Span status - Unset
- Parent span ID - empty jeśli root
  - ApplicationsInsight pokazuje takie samo Operation ID
-->
