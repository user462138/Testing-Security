Feature: Normal operation of the subscriber

Background: 
    Given the subscriber is initialised

Scenario: the results of the tests are both not sufficient 
     When both the results are not sufficient
     Then a mail is sent to the student