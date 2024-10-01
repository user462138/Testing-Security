Feature: Failsafe in case of temperature sensor error

Background:
    Given the temperature setpoint is 20°C
      And the temperature offset is 5°C
      And the maximum number of temperature sensor failures is 3

Scenario: Heater is turned off when temperature sensor error occurs too often
    Given the heater is on
     When the temperature sensor fails more than the maximum number of times
     Then the heater is turned off

Scenario: Recover from temperature sensor error
    Given the thermostat is in safe mode because of temperature sensor failure
     When the temperature drops below 15°C
     Then the heater is turned on
