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
    app["Application"]
    subgraph apm["APM"]
        backend["Storage backend"]
        fronted["Visualization frontend"]
    end
    
    app --> apm
```
</div>
</template>

<template #4>
<div align="center">
```mermaid
graph TD;
    app["Application"]
    ai["Application Insights"]
    
    app --> ai
```
</div>
</template>

</v-switch>

<!--
- APM - Application Performance Monitoring
-->

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
