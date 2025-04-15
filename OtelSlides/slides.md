---
download: true
# You can also start simply with 'default'
theme: seriph
background: ./cover.jpg
# some information about your slides (markdown enabled)
title: OpenTelemetry w .NET
info: |
  ## Slidev Starter Template
  Presentation slides for developers.

  Learn more at [Sli.dev](https://sli.dev)
# apply unocss classes to the current slide
#class: text-center
# https://sli.dev/features/drawing
drawings:
  persist: false
# slide transition: https://sli.dev/guide/animations.html#slide-transitions
transition: slide-left

# enable MDC Syntax: https://sli.dev/features/mdc
#mdc: true
# open graph
# seoMeta:
#  ogImage: https://cover.sli.dev
---

# <img style="display: inline; width: 50%" src="./opentelemetry-horizontal-color.svg"><br/>w .NET

**Jak to w zasadzie działa?**

---
transition: fade-out
layout: two-cols
layoutClass: gap-16
hideInToc: true
---

# Intro
<br/>

<div>
  <a href="https://github.com/idle-code/OpenTelemetryDemo" target="_blank" class="slidev-icon-btn"><carbon:logo-github /></a>
  <a href="https://github.com/idle-code/OpenTelemetryDemo" target="_blank">github.com/idle-code/OpenTelemetryDemo</a>
</div>

<div>

<img style="margin-left: auto; margin-right: auto; width: 50%" src="./gh-qr-code.svg" />

</div>

::right::

<Toc text-sm minDepth="1" maxDepth="3" />


---
src: ./pages/what-is-observability.md
---

---
src: ./pages/what-is-telemetry.md
---

---
src: ./pages/why-opentelemetry.md
---

---
src: ./pages/what-is-opentelemetry.md
---

---
src: ./pages/demo-project.md
---

---
src: ./pages/otel.md
---

---
src: ./pages/context-propagation.md
---

---
src: ./pages/processor-filtering.md
---

---
src: ./pages/otel-baggage.md
---
