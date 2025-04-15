---
layout: two-cols
---

## Logs

Krótkie wiadomości tekstowe generowane w czasie uruchamiania kodu

<v-clicks>

- Informacje o stanie aplikacji
- Decyzja podjęta przez aplikację
- Ostrzeżenia (błędy walidacji)
- Błędy (wyjątki)

</v-clicks>

::right::

<br/>
<br/>

<v-click>

````md magic-move {lines: true}

```csharp {*|3-4,7,12,14}
public static void Main(string[] args)
{
    Console.WriteLine("Got args: {0}",
        string.Join(", ", args));
    if (args.Length < 2)
    {
        Console.WriteLine("Not enough arguments provided");
        return;
    }
    
    var a = int.Parse(args[0]);
    Console.WriteLine("A = {0}", a);
    var b = int.Parse(args[1]);
    Console.WriteLine("B = {0}", b);
    
    var sum = a + b;
    Console.WriteLine("The sum is: {0}", sum);
}
```

```csharp {3,6,11,13}
public static void Main(string[] args)
{
    _logger.LogDebug("Got args: {Args}", args);
    if (args.Length < 2)
    {
        _logger.LogError("Not enough arguments provided");
        return;
    }
    
    var a = int.Parse(args[0]);
    _logger.LogInformation("A = {A}", a);
    var b = int.Parse(args[1]);
    _logger.LogInformation("B = {B}", b);
    
    var sum = a + b;
    Console.WriteLine("The sum is: {0}", sum);
}
```
````

</v-click>

<!--
- Najstarszy format telemetrii
- OpenTelemetry musiało się do niego dostosować
-->

---

<<< ../../WebAPI/Handlers/IncrementNamedCounter.cs#handle-method {*|8,13|3|*}{lines:true}

<br/>

<v-click>

<<< ../../WebAPI/Telemetry/LoggerExtensions.cs#push-property-extension-method {lines:true}

</v-click>

<!--
- Nie rozwodzić się nad tym co jest logowane
- Warto używać poziomów logowania ze względu na koszty trzymania logów
-->

---
hideInToc: true
transition: fade
---

## .NET Logging

<v-clicks>

<div>

- `WithLogging()`: opentelemetry-dotnet SDK rejestruje `OpenTelemetryLoggerProvider`

</div>

<<< ../../WebAPI/Program.cs#opentelemetry-setup {*|5-6|*}{lines:true}

</v-clicks>

---
hideInToc: true
---

## .NET Logging

<div>

- `WithLogging()`: opentelemetry-dotnet SDK rejestruje `OpenTelemetryLoggerProvider`

</div>

<div>

- `ILoggerFactory` tworzy instancję `ILogger` która jest wstrzykiwana do klasy `IncrementNamedCounterHandler`

</div>

<v-clicks>

```csharp
_logger.LogInformation("Incrementing {CounterId} counter by {Delta}", request.CounterId, request.Delta);
```

```json
{
  "LogLevel": "Information",
  "Category": "WebAPI.Handlers.IncrementNamedCounterHandler",
  "TraceId": "d0194a116c84d1d36214436295d7fa55",
  "SpanId": "d251c5795cded1e9",
  "Attributes": { "CounterId": "rabarbar", "Delta": 1 }
}
```

<div>

- Log jest przetwarzany przez **procesory**
  - Tu następuje **grupowanie** (batching) rekordów
  - Na tym etapie log może zostać **odfiltrowany** (np. ze względu na swój poziom)

</div>

<div>

- Log jest jest wysyłany do **eksportera** (np. OTLP, Console)

</div>

</v-clicks>

<!--

- LoggerProvider nie tworzy loggerów - to jest sink na logi, LoggerFactory to robi
- Adaptacja OpenTelemetry do istniejących implementacji
- LoggerFactory konfiguruje ILoggery (aby pisały do LoggerProviderów)

-->

---
hideInToc: true
transition: fade
---

## LogRecord

<v-clicks>

|                                   |                                   |
|----------------------------------:|:----------------------------------|
|                          **Body** | Treść (message)                   |
|                    **Attributes** | Dodatkowe atrybuty (tagi)         |
| **SeverityText / SeverityNumber** | Log level                         |
|     Timestamp / ObservedTimestamp | Czas kiedy log został wyemitowany |
| TraceId<br/>SpanId<br/>TraceFlags | Trace context                     |
|              InstrumentationScope | Instrumentacja generująca log     |
|                          Resource | Serwis/środowisko generujący log  |

</v-clicks>

<!--
- Atrybuty strukturalne lądują w Attributes

- Co jeśli nie ma Activity? logi są nieskorelowane?
-->

---
hideInToc: true
---

## LogRecord

|                                   |                                                                          |
|----------------------------------:|:-------------------------------------------------------------------------|
|                          **Body** | `Incrementing {CounterId} counter by {Delta}`                            |
|                    **Attributes** | `CounterId: rabarbar` `Delta: 1`                                         |
| **SeverityText / SeverityNumber** | `Info`                                                                   |
|     Timestamp / ObservedTimestamp | `2025-04-12T14:57:26.6768257Z`                                           |
| TraceId<br/>SpanId<br/>TraceFlags | `d0194a116c84d1d36214436295d7fa55`<br/>`32434d330c6950e9`<br/>`Recorded` |
|              InstrumentationScope |                                                                          |
|                          Resource | `service.name: WebAPI`                                                   |
