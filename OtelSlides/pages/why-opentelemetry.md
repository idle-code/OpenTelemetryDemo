---
transition: fade
---

# Dlaczego OpenTelemetry?

<v-switch>

<template #1>
<div align="center">

```mermaid
graph TD;
    app["Application"]
    backend["Storage backend"]
    fronted["Visualization frontend"]
    
    app --> backend --> fronted
```

</div>
</template>

<template #2>
<div align="center">


```mermaid
graph TD;
    app["Application"]
    prometheus["Prometheus"]
    loki["Loki"]
    grafana["Grafana"]
    
    app --> prometheus
    app --> loki
    prometheus --> grafana
    loki --> grafana
```


</div>
</template>

<template #3>
<div align="center">


```mermaid
graph TD;
    subgraph app["Application"]
        prometheus_int["prometheus-net"]
        loki_sink["Serilog.Sinks.Grafana.Loki"]
    end
    prometheus["Prometheus"]
    loki["Loki"]
    grafana["Grafana"]
    
    prometheus_int --> prometheus
    loki_sink --> loki
    prometheus --> grafana
    loki --> grafana
```


</div>
</template>

<template #4>
<div align="center">


```mermaid
graph TD;
    app["Application"]
    subgraph apm["APM"]
        backend["Storage backend"]
        fronted["Visualization frontend"]
    end
    
    app --> apm
```


</div>
</template>

<template #5>
<div align="center">


```mermaid
graph TD;
    app["Application"]
    ai["Application Insights"]
    
    app --> ai
```


</div>
</template>

<template #6>
<div align="center">

```mermaid
graph TD;
    app["Application"]
    ai["Application Insights"]
    datadog["Datadog"]
    
    app --> ai
    app -.-> datadog
```

</div>
</template>

<template #7>
<div align="center">

```mermaid
graph TD;
    app1["Application 1"]
    app2["Application 2"]
    app3["Application 3"]
    ai["Application Insights"]
    datadog["Datadog"]
    
    app1 --> ai
    app2 --> ai
    app3 --> ai
    app3 -.-> datadog
```

</div>
</template>


<template #8>
<div align="center">

```mermaid
graph TD;
    app1["C#"]
    app2["Python"]
    app3["Rust"]
    ai["Application Insights"]
    datadog["Datadog"]
    
    app1 --> datadog
    app2 --> datadog
    app3 --> ai
    app3 -.-> datadog
```

</div>
</template>


<template #9>
<div align="center">

```mermaid
graph TD;
    app1["C#"]
    app2["Python"]
    app3["Rust"]
    ai["Application Insights"]
    datadog["Datadog"]
    
    app1 --> datadog
    app2 --> datadog
    app3 --> ai
    app3 -.-> datadog
    
    ai -.- datadog
```

</div>
</template>

</v-switch>

<!--
- Telemetria nie jest niczym nowym
- APM - Application Performance Monitoring

TODO: Kooperacja wielu systemów
-->

---
hideInToc: true
transition: fade
---

# Bonus: Redneck APM

<v-click>

```csharp {*|1|5-7|9-21|*}{lines: true}
AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(SendExceptionDetails);

static void SendExceptionDetails(object sender, UnhandledExceptionEventArgs args) 
{
    Exception exception = (Exception)args.ExceptionObject;
    MailMessage error_message = new MailMessage();
    PrepareErrorMessageBody(error_message, exception);
    
    error_message.From = new MailAddress("support@xxx.eu");
    string error_recipients = ConfigurationManager.AppSettings.Get("ErrorReportRecipients");
    foreach (string recipient in error_recipients.Split(';'))
        error_message.To.Add(new MailAddress(recipient));

    SmtpClient smtp_client = new SmtpClient
    {
        Credentials = new System.Net.NetworkCredential("support@xxx.eu", "<the password>"),
        Host = "mail.xxx.eu",
        Port = 25
    };
    
    smtp_client.Send(error_message);
}

```

</v-click>

<!--
Wprowadzenie - system ERP
    - rejestracja nowych pracowników w biurach
    - organizacja zakwaterowania oraz dojazdu do pracy
    - obliczenia godzin pracy, raporty

Cecha: działał lokalnie na komputerach użytkowników, łączył się tylko z bazą danych MySQL
-->

---
hideInToc: true
---

# Bonus: Redneck APM

<div align="center">

```mermaid
graph TD;
    app["Application"]
    backend["Support mailbox"]
    fronted["Thunderbird"]
    
    app --> backend --> fronted
```

</div>
