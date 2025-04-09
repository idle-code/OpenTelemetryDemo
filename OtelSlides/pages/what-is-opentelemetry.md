# Składniki OpenTelemetry

<v-clicks>

- Protokół wymiany danych OTLP
- Konwencje nazewnicze
- Wskazówki odnośnie API
- SDK dla poszczególnych języków programowania
- Biblioteki obsługujące najczęściej występujące scenariusze
- Systemy instrumentacji automatycznej
- OpenTelemetry Collector - proxy do zbierania metryk

</v-clicks>

---

# OpenTelemetry Pipeline

<div align="center" style="align-content: center">
<v-switch>

<template #0>
```mermaid {scale: 0.7}
graph TD;
    app["Application"]
    apm["APM"]
    
    app --> apm
```
</template>

<template #1>
```mermaid {scale: 0.7}
graph TD;
    subgraph app["Application"]
        logs["Logs"]
        traces["Traces"]
        metrics["Metrics"]
        processors["Processors"]
        exporters["Exporters"]
        
        logs --> processors
        traces --> processors
        metrics --> processors
        processors --> exporters
    end
    apm["APM"]
    
    exporters -- "OTLP" --> apm
```
</template>

<template #2>
```mermaid {scale: 0.6}
graph TD;
    subgraph app["Application"]
        logs["Logs"]
        traces["Traces"]
        metrics["Metrics"]
        processors["Processors"]
        exporter["Exporter"]
        
        logs --> processors
        traces --> processors
        metrics --> processors
        processors --> exporter
    end
    
    exporter -- "OTLP" --> collector
    
    subgraph collector["OpenTelemetry Collector"]
        collectorProcessor["Processors"]
        collectorExporters["Exporters"]
        
        collectorProcessor --> collectorExporters
    end
    
    apm["APM"]
    
    collectorExporters -- "OTLP" --> apm
```
</template>

<template #3>
```mermaid {scale: 0.7}
graph TD;
    subgraph app["OTEL Pipeline"]
        logs["Logs"]
        traces["Traces"]
        metrics["Metrics"]
        processors["Processors"]
        exporters["Exporters"]
        
        logs --> processors
        traces --> processors
        metrics --> processors
        processors --> exporters
    end
```
</template>

</v-switch>
</div>
