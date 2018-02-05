Feature: CheckForTheNumberOfBlogs
	The number of blogs appearing between parentheses need to 
	corresponds to the number of blogs in each page of blogs

Scenario: Blog page is loading check
	Given I navigated to the main blog page _ first TC
	When I search for a particular element on this page
	Then this element should be there

Scenario: Check that the blog post by date table exists
	Given I navigated to the main blog page _ second TC
	When I search for the list of blogs
	Then The list of blogs is not empty

Scenario: Search the table and verify the result
	Given I navigated to the main blog page  _ third TC
	When I search the list of blogs and their number of blogs
	Then Navigating to Each blog page shows the expected number of blogs 
