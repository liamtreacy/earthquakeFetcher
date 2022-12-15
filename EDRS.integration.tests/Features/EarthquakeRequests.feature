Feature: Earthquake requests tests

Scenario: Get request returns something
	Given I am a client
	Given the stored data is cleared
	When I make a GET request to '/api/earthquakes'
	Then the response status code is '200'
	Then the response json should not be  empty
	
Scenario: Data is deleted on request
	Given I am a client
	Given the stored data is cleared
	Then the response status code is '200'
	#When I make a GET request to '/api/earthquakes/largestmag'
	#Then the response status code is '404'
	
	
Scenario: Get largest mag fails if no earthquakes retrieved
	Given I am a client
	Given the stored data is cleared
	When I make a GET request to '/api/earthquakes/largestmag'
	Then the response status code is '404'