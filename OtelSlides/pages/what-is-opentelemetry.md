# Składniki OpenTelemetry

<v-clicks depth="2">

- Konwencje nazewnicze
- Wskazówki odnośnie API
- Ekosystem dla poszczególnych języków/środowisk programowania
  - SDK
  - Biblioteki obsługujące najczęściej występujące scenariusze
  - Systemy instrumentacji automatycznej
- Protokół wymiany danych OTLP
- OpenTelemetry Collector - proxy do zbierania/przetwarzania i przekazywania sygnałów dalej

</v-clicks>

<!--
- OpenCensus oraz OpenTracking
- Jest to średnio-sformalizowany standard - RFC
-->

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
    
    exporters --> apm
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
    
    collectorExporters --> apm
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
