# opentelemetry-dotnet SDK
Oficjalna dystybucja SDK OpenTelemetry dla platformy .NET

<div>
  <a href="https://github.com/open-telemetry/opentelemetry-dotnet" target="_blank" class="slidev-icon-btn"><carbon:logo-github /></a>
  <a href="https://github.com/open-telemetry/opentelemetry-dotnet" target="_blank">github.com/open-telemetry/opentelemetry-dotnet</a>
</div>

<br/>

``` {*|1-2|4-8|10}
OpenTelemetry.Extensions.Hosting
Azure.Monitor.OpenTelemetry.AspNetCore

OpenTelemetry.Instrumentation.EntityFrameworkCore
OpenTelemetry.Instrumentation.GrpcNetClient
OpenTelemetry.Instrumentation.Runtime
OpenTelemetry.Instrumentation.SqlClient
RabbitMQ.Client.OpenTelemetry

OpenTelemetry.Exporter.Console
```

<!--
- Oficjalna dystrybucja SDK
- Do użytku w aplikacjach końcowych
- W przypadku bibliotek, powinniśmy korzystać z API
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

---
src: ./otel-baggage.md
---
