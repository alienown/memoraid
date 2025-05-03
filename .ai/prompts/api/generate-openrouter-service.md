Your task is to implement a service based on the provided implementation plan and implementation rules. Your goal is to create a detailed and accurate implementation that complies with the provided plan, communicates correctly with the API, and handles all specified functionalities and error cases.

First, review the implementation plan:

<implementation_plan>
{{implementation-plan}} <- replace with a reference to the service implementation plan

Create the service in {{path}}
</implementation_plan>

Now review the implementation rules:
<implementation_rules>
{{backend-rules}} <- replace with a reference to the rules useful for the service (e.g. shared.mdc)
</implementation_rules>

Implement the plan according to the following approach:
<implementation_approach>
Carry out a maximum of 3 steps of the implementation plan, briefly summarize what you have done and describe the plan for the next 3 actions - stop work at this point and wait for my feedback.
</implementation_approach>

Carefully analyze the implementation plan and rules. Pay special attention to the service structure, API integration, error handling and security issues described in the plan.

Follow these steps to implement a service:

Service structure:
- Define the service class according to the implementation plan
- Create a constructor that initializes the required fields
- Apply appropriate access modifiers to fields and methods (public, private)

Public method implementation:
- Implement public methods listed in the plan
- Ensure that each method is correctly typed for both parameters and return values
- Provide a complete implementation of the business logic described in the plan

Private method implementation:
- Develop helper methods listed in the plan
- Ensure proper encapsulation and separation of responsibility
- Implement the logic for formatting data, sending requests, and processing responses

API integration:
- Implement the logic for communicating with an external API
- Handle all necessary parameters and request headers
- Ensure correct processing of responses from the API

Error handling:
- Implement comprehensive error handling for all scenarios
- Use appropriate retry mechanisms for transient errors
- Provide clear error messages for different scenarios

Documentation and typing:
- Define and implement appropriate interfaces for parameters and return values
- Ensure full type coverage for the entire service

Testing:
- Structure the service in a way that allows for easy unit testing
- Consider mocking external dependencies

Throughout the implementation process, the provided implementation rules must be strictly followed. These rules take precedence over any general best practices that may conflict with them.

Make sure that your implementation accurately reflects the provided implementation plan and adheres to all specified rules. Pay special attention to the service structure, API integration, error handling, and security.