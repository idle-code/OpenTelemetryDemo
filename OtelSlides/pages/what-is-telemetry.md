# Czym jest telemetria?

<v-switch>
<template #1>

```csharp
public static void Main(string[] args)
{
    Console.WriteLine("Got args: {0}", string.Join(", ", args));
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

</template>


<template #2>


```csharp
public static void Main(string[] args)
{
    _logger.LogDebug("Got args: {Args}", string.Join(", ", args));
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

</template>
</v-switch>

---

<v-switch>
    <template #0><img src="./app1.svg" /></template>
    <template #1><img src="./app2.svg" /></template>
    <template #2><img src="./app3.svg" /></template>
    <template #3><img src="./app4.svg" /></template>
    <template #4><img src="./app5.svg" /></template>
</v-switch>
