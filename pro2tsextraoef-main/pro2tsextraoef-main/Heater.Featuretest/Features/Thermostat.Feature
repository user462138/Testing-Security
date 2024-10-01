Feature: Normal operation of the thermostat

Background:
    Given the temperature setpoint is 20°C
      And the temperature offset is 5°C

Scenario: Heater is turned on when temperature is below setpoint minus offset
    Given the heater is off
     When the temperature drops below 15°C
     Then the heater is turned on

Scenario: Heater is not turned on when temperature is above setpoint minus offset
    Given the heater is off
     When the temperature drops to 15°C
     Then the heater is not turned on

Scenario: Heater is turned off when temperature is above setpoint plus offset
    Given the heater is on
     When the temperature increases above 25°C
     Then the heater is turned off

Scenario: Heater is not turned off when temperature is below setpoint plus offset
    Given the heater is on
     When the temperature increases to 25°C
     Then the heater is not turned off
