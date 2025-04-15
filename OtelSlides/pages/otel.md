# opentelemetry-dotnet SDK
Oficjalna dystybucja SDK OpenTelemetry dla platformy .NET

<div>
  <a href="https://github.com/open-telemetry/opentelemetry-dotnet" target="_blank" class="slidev-icon-btn"><carbon:logo-github /></a>
  <a href="https://github.com/open-telemetry/opentelemetry-dotnet" target="_blank">github.com/open-telemetry/opentelemetry-dotnet</a>
</div>

<div>
    <a href="https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel" target="_blank" class="slidev-icon-btn"><img style="display: inline" width="16px" src="https://learn.microsoft.com/favicon.ico"></a>
    <a href="https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel" target="_blank">learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel</a>
</div>

<br/>

``` {*|1|3-7|9-10}
OpenTelemetry.Extensions.Hosting

OpenTelemetry.Instrumentation.EntityFrameworkCore
OpenTelemetry.Instrumentation.GrpcNetClient
OpenTelemetry.Instrumentation.Runtime
OpenTelemetry.Instrumentation.SqlClient
RabbitMQ.Client.OpenTelemetry

OpenTelemetry.Exporter.Console
Azure.Monitor.OpenTelemetry.AspNetCore
```

<!--
- Oficjalna dystrybucja SDK
- Do użytku w aplikacjach końcowych
- W przypadku bibliotek, powinniśmy korzystać z paczek API
-->

---
layout: section
---

# Sygnały

<!--
- Typy telemetrii
-->

---
src: ./otel-logs.md
---

---
src: ./otel-traces.md
---

---
src: ./otel-metrics.md
---
