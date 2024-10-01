# README

## Opdracht

Maak een thermostaat applicatie die gebaseerd op de huidige temperatuur en een temperatuur setpoint een verwarmingselement aan/af zet. Gebruik ook een offset zodat het verwarmingselement niet constant aan en af springt als de temperatuur rond het setpoint schommelt. Als het ophalen van de temperatuur te vaak faalt (vb 3x na elkaar) willen we voorkomen dat de heater constant aan blijft staan, dus naar safe mode waar de heater uitgezet wordt.

## Class diagram

```plantuml
class Thermostat {
    - double setpoint
    - double offset
    --
    + void Work()
}

note left of Thermostat {
    Application creates a Thermostat with a
    TemperatureSensor and HeatingElement. It
    also calls Thermostat::work() in a loop.
}

note right of Thermostat::work {
    This is where the algorithm is implemented.
    If current temperature below setpoint minus
    offset, turn on heater. If current temperature
    above setpoint plus offset, turn off heater.
}

interface ITemperatureSensor {
    + {abstract} double temperature()
}

note right of ITemperatureSensor {
    Depend on abstractions instead of concrete
    classes. So add a temperature sensor interface.
    This can then also be used for mocking in the
    unittests.
}

interface IHeatingElement {
    + {abstract} void Enable()
    + {abstract} void Disable()
}

note left of IHeatingElement {
    Depend on abstractions instead of concrete
    classes. So add a heating element interface.
    This can thne also be used for mocking in the
    unittests.
}

class TemperatureSensor {
    - String url
}

note right of TemperatureSensor {
    Concrete temperature sensor. Since we don't
    have an actual temperature sensor we can get
    the temperature for Antwerp from the Internet.
    See exercise from other class.
}

class HeatingElement {
    + bool IsEnabled()
}

note left of HeatingElement {
    Concrete heating element. Since we don't
    have an actual heating element we can pretend
    by having a boolean that keeps track of whether
    the heating element is enabled or not.
}

Thermostat o-down- ITemperatureSensor
Thermostat o-down- IHeatingElement
TemperatureSensor -up-|> ITemperatureSensor
HeatingElement -up-|> IHeatingElement
```

## Test cases

* Als huidige temperatuur kleiner is dan setpoint - offset --> heater aan
* Als huidige temperatuur gelijk is aan setpoint - offset --> moet niets gebeuren
* Als huidige temperatuur groter is dan setpoint + offset --> heater uit
* Als huidige temperatuur gelijk is aan setpoint + offset --> moet niets gebeuren
* Als temperatuur ophalen faalt --> exception vangen, moet verder niets met heater gebeuren
* Als temperatuur ophalen te vaak faalt --> ga naar safe mode --> heater uitzetten
* Als temperatuur ophalen terug slaagt na te vaak falen --> ga uit safe mode --> heater terug aan als temperatuur te laag is
