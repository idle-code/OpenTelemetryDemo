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
