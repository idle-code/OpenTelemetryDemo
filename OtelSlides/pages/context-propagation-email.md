---
transition: fade
---

# Propagacja kontekstu: Email

<img src="./mail.png">

---
hideInToc: true
---
# Propagacja kontekstu: Email

<br/>

<style>
code {
    font-size: 1.5em;
}
</style>

```js
http://localhost:8081/confirm
    ?token=4d4c0c918550479591816345892b9351
    &_traceparent=00-3f710633e014b9c90b70271af31719a3-b3edfe7ffaf4fd18-01
    &_baggage=request.CounterId=test,request.Delta=1
```

<br/>

<v-click>

## 00-3f710633e014b9c90b70271af31719a3-b3edfe7ffaf4fd18-01

</v-click>
<br/>

<v-click>

W3C traceparent - https://www.w3.org/TR/trace-context/

```mermaid
---
title: "W3C traceparent"
---
block-beta
    columns 4
    version["Version"]
    traceId["Trace ID"]    
    parentId["Parent (span) ID"]
    flags["Flags"]
    
    versionVal["00"]
    traceIdVal["3f710633e014b9c90b70271af31719a3"]
    parentIdVal["b3edfe7ffaf4fd18"]
    flagsVal["01"]
```

</v-click>

---
hideInToc: true
transition: fade
---

## Propagacja kontekstu: Email - wysyłanie

<<< ../../FunctionsWorker/NotificationsFunction.cs#send-notification {*|6|*}{lines:true, maxHeight: '80%'}

---
hideInToc: true
---

## Propagacja kontekstu: Email - wysyłanie

```csharp
private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
```

<<< ../../FunctionsWorker/NotificationsFunction.cs#url-enrichement {*|9-14|9-14,17-20|*}{lines:true, maxHeight: '80%'}

<!--
- Jest to przykład manualnej instrumentacji
-->

---
hideInToc: true
---

## Propagacja kontekstu: Email - odbieranie

<<< ../../WebAPI/Telemetry/ContextFromQueryMiddleware.cs#query-context-middleware {*|7|13-17|19|3,20|20,26-30|21|23|*}{lines:true, maxHeight: '85%'}

---

<img src="./ai_confirm.png">
