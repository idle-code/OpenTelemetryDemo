---
transition: fade
---

# Projekt demo

<iframe width="100%" height="80%" src="http://localhost:8080/"></iframe>

---
hideInToc: true
transition: fade
---

# Projekt demo

<v-switch>

<template #0>
<div align="center">

```mermaid
graph LR;
    user@{ shape: start }
    app["Application"]
    db[(Database)]
    mail@{ shape: notch-rect, label: "E-Mail" }
    
    user --> app
    app --> db
    app --> mail
```

</div>
</template>

<template #1>
<div align="center">

```mermaid
graph LR;
    user@{ shape: start }
    ui["Angular frontend"]
    db[(Database)]
    mail@{ shape: notch-rect, label: "E-Mail" }
    api["Web API"]
    
    user --> ui --> api
    api --> db
    api --> mail
```

</div>
</template>

<template #2>
<div align="center">

```mermaid
graph LR;
    user@{ shape: start }
    ui["Angular frontend"]
    db[(Database)]
    function["Azure function"]
    mail@{ shape: notch-rect, label: "E-Mail" }
    api["Web API"]
    
    user --> ui --> api
    api --> db
    api --> function --> mail
```

</div>
</template>

<template #3>
<div align="center">

```mermaid
graph LR;
    user@{ shape: start }
    ui["Angular frontend"]
    api["Web API"]
    db[(Database)]
    rabbitmq[/RabbitMQ queue/]
    function["Azure function"]
    mail@{ shape: notch-rect, label: "E-Mail" }
    
    user --> ui --> api
    api --> db
    api --> rabbitmq
    rabbitmq --> function --> mail
```

</div>
</template>

<template #4>
<div align="center">

```mermaid
graph LR;
    user@{ shape: start }
    ui["Angular frontend"]
    db[(Database)]
    rabbitmq[/RabbitMQ queue/]
    function["Azure function"]
    mail@{ shape: notch-rect, label: "E-Mail" }
    
    subgraph api["Web API"]
        mediator["MediatR"]
        increment["IncrementNamedCounter"]
        
        mediator --> increment
    end
    
    user --> ui --> api
    api --> db
    api --> rabbitmq
    rabbitmq --> function --> mail
```

</div>
</template>

</v-switch>
