Feature: ExternalDataRetrieverServiceTests
	An API to get earthquake details and populate a db.
	
Scenario: A Get request returns 200
	Given the ExternalDataRetrieverService is started
	When I post a Get request
	Then the result should be 200
