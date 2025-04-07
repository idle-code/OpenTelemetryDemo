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
    apm["APM - Application Performance Monitoring"]
    
    app --> apm
```

</div>
</template>

<template #3>
<div align="center">

```mermaid
graph TD;
    app1["Application"]
    app2["Application"]
    ai["Application Insights"]
    prometheus["Prometheus"]
    loki["Loki"]
    grafana["Grafana"]
    
    app1 --> ai
    
    app2 --> prometheus
    app2 --> loki
    prometheus --> grafana
    loki --> grafana
```

</div>
</template>

</v-switch>

---

### Bonus slide: Redneck APM

```csharp
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
        Port = 25 //587
    };
    
    smtp_client.Send(error_message);
}

```
