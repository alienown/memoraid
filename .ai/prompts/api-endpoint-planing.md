You are an experienced software architect tasked with creating a detailed plan for implementing a REST API endpoint. Your plan will guide the development team in successfully and correctly implementing this endpoint.

Before we start, please review the following information:

1. Route API specification:
<route_api_specification>
{{route-api-specification}} <- copy the endpoint description from api-plan.md
</route_api_specification>

2. Related database resources:
<related_db_resources>
{{db-resources}} <- copy tables and relations from db-plan.md
</related_db_resources>

3. Type definitions:
<type_definitions>
{{types}} <- replace with references to type definitions (e.g. @types)
</type_definitions>

3. Tech stack:
<tech_stack>
{{tech-stack}} <- replace with references to @tech-stack.md
</tech_stack>

4. Implementation rules:
<implementation_rules>
1. Reference types in request classes should stay optional even though they are required in the endpoint specification.
2. Use FluentValidation for validating incoming requests according to the endpoint API specification.
3. API responses should be of type Response<T> where T is the response class. Response<T> is a generic class that holds the data and errors.
4. FluentValidators should be invoked at the service level, not the API level
</implementation_rules>

Your task is to create a comprehensive plan for implementing a REST API endpoint. Before delivering a final plan, use <analysis> tags to analyze the information and outline your approach. In this analysis, make sure to:

1. Summarize the key points of the API specification.
2. List the required and optional parameters from the API specification.
3. List the necessary request and response classes.
4. Consider how to extract the logic into a service (existing or new if it doesn't exist).
5. Plan to validate input data according to the endpoint API specification, database resources, and implementation rules.
6. Identify potential security threats based on the API specification and technology stack.
7. Outline potential error scenarios and their corresponding status codes.

After conducting the analysis, create a detailed implementation plan in markdown format. The plan should contain the following sections:

1. Endpoint overview
2. Request details
3. Response details
4. Data flow
5. Security considerations
6. Error handling
7. Performance
8. Implementation steps

Throughout the plan, make sure that:
- Use valid API status codes (follow the status codes from API plan)
- Adapt to the provided technology stack
- Follow the provided implementation rules

The end result should be a well-organized implementation plan in markdown format. Here is an example of what the output should look like:

```markdown
# API Endpoint Implementation Plan: [Endpoint Name]

## 1. Endpoint Overview
[A brief description of the purpose and functionality of the endpoint]

## 2. Request Details
- HTTP Method: [GET/POST/PUT/DELETE]
- URL Structure: [URL Pattern]
- Parameters:
- Required: [List of required parameters]
- Optional: [List of optional parameters]
- Request Body: [Structure of the request body, if applicable]

## 3. Types Used
[DTOs and Command Models necessary for the implementation]

## 3. Response Details
[Expected response structure and status codes]

## 4. Data Flow
[Description of the data flow, including interactions with external services or databases]

## 5. Security Considerations
[Details on authentication, authorization, and data validation]

## 6. Error Handling
[List of potential errors and how to handle them]

## 7. Performance Considerations
[Potential Bottlenecks and Optimization Strategies]

## 8. Implementation Steps
1. [Step 1]
2. [Step 2]
3. [Step 3]
...
```

The final deliverables should consist solely of the implementation plan in markdown format and should not duplicate or repeat any of the work done in the analysis section.

Be sure to save your implementation plan as .ai/view-implementation-plan.md. Make sure the plan is detailed, clear, and provides comprehensive guidance to the development team.