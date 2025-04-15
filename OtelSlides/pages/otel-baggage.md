# Baggage

Globalne metadane operacji

<v-clicks>

- ID użytkownika
- Tenant ID

</v-clicks>

<!--
- Kolekcja klucz-wartość
    - możliwe obiekty złożone
- Domyślnie nie używany w śladach, OTEL zapewnie jedynie propagację
- Należy uważać co jest tam zawierane ponieważ dane mogą się gromadzić
-->

---

# Processor: Propagacja bagażu

<<< ../../WebAPI/Telemetry/BaggageEnrichingProcessor.cs#baggage-processor {*}{lines:true}
