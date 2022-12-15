Feature: Earthquake requests tests

Scenario: Get request returns something
	Given I am a client
	When I make a GET request to '/api/earthquakes'
	Then the response status code is '200'
	Then the response json should not be  empty