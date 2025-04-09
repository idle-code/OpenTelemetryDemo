---
layout: two-cols
---
## Logs

Krótkie wiadomości tekstowe generowane w czasie uruchamiania kodu

- Decyzja podjęta przez aplikację
- Ostrzeżenie (błędy walidacji)
- Błędy działania aplikacji (wyjątki)

<br/>

<v-click at="4">

- Timestamp
- Level
- Message
- Attributes/Parameters

</v-click>


::right::

<br/>
<br/>

<v-click>

````md magic-move {lines: true}

```csharp {*|3,4,6,11,13,16}
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

```csharp {3,6,11,13,16}
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
    _logger.LogInformation("The sum is: {Sum}", sum);
}
```
````

</v-click>

<!--
- Najstarszy format telemetrii
- OpenTelemetry musiało się do niego dostosować
-->

---

<<< ../../WebAPI/Handlers/IncrementNamedCounter.cs#handle-method {*|6,11|1}{lines:true}

<br/>

<v-click>

<<< ../../WebAPI/LoggerExtensions.cs#push-property-extension-method {lines:true}

</v-click>
