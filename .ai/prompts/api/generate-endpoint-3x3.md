Your task is to implement a REST API endpoint based on the provided implementation plan. Your goal is to create a solid and well-organized implementation that includes proper validation, error handling, and follows all the logical steps described in the plan.

First, review the provided implementation plan carefully:

<implementation_plan>
{{endpoint-implementation-plan}} <- add a reference to the endpoint implementation plan (e.g. #generations-endpoint-implementation-plan.md)
</implementation_plan>

<types>
{{types}} <- add references to type definitions (e.g. #types)
</types>

<implementation_rules>
{{backend-rules}} <- add references to backend rules (e.g. #shared.mdc, #backend.mdc, #astro.mdc)
</implementation_rules>

<implementation_approach>
Perform a maximum of 3 steps of the implementation plan, briefly summarize what you have done and describe the plan for the next 3 actions - stop work at this point and wait for my feedback.
</implementation_approach>

Now, follow these steps to implement a REST API endpoint:

1. Review your implementation plan:
- Define the HTTP method (GET, POST, PUT, DELETE, etc.) for the endpoint.
- Determine the structure of the endpoint URL
- List all expected input parameters
- Understand the required business logic and data processing steps
- Note any special validation or error handling requirements.

2. Start the implementation:
- Start by defining the endpoint function with a valid HTTP method decorator.
- Configure function parameters based on expected input
- Implement input validation for all parameters
- Follow the logical steps outlined in the implementation plan
- Implement error handling for each step of the process
- Ensure proper data processing and transformation as required
- Prepare the response data structure

3. Validation and error handling:
- Implement strict input validation for all parameters
- Use appropriate HTTP status codes for different scenarios (e.g. 400 for syntactic errors, 422 for business rules violations, 404 for not found, 500 for server errors).
- Provide clear and informative error messages in the response.
- Handle potential exceptions that may occur during processing.

4. Testing considerations:
- Consider edge cases and potential issues that should be tested.
- Ensure that the implementation covers all scenarios listed in the plan.

5. Documentation:
- Add clear comments to explain complex logic or important decisions
- Include documentation for the main function and any helper functions.

Once the implementation is complete, make sure it includes all necessary imports, function definitions, and any additional helper functions or classes required for the implementation.

If you have any assumptions to make or questions about the implementation plan, state them before writing the code.

Be sure to follow REST API design best practices, adhere to programming language style guidelines, and ensure that the code is clean, readable, and well-organized.