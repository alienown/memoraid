You are an experienced software architect tasked with creating an implementation plan for the OpenRouter service. This service will interact with the OpenRouter API to complement LLM-based chats. Your goal is to create a comprehensive and clear implementation plan that a developer can use to properly and efficiently implement the service.

First, review the provided technology stack and implementation rules:

<tech_stack>
{{tech-stack}}
</tech_stack>

<service_rules>
{{service-rules}}
</service_rules>

Now, analyze the information provided and break down the implementation details. Use <implementation_breakdown> tags inside the thinking block to show your thought process. Consider the following:

1. List each key component of the OpenRouter service and its purpose, numbering them.
2. For each component:
  a. Describe its functionality in detail.
  b. List potential implementation challenges, numbering them.
  c. Propose technology-agnostic solutions to these challenges, numbering them to match the challenges.
3. Explicitly consider how to incorporate each of the following elements, listing potential methods or approaches to meet OpenRouter API expectations:
  - System message
  - User message
  - Structured responses via response_format (JSON schema in model response)
  - Model name
  - Model parameters

Provide specific examples for each element, numbering them. Make sure these examples are clear and show how they should be implemented in the service, especially in the case of response_format. Use the pattern of a well-defined response_format: { type: 'json_schema', json_schema: { name: [schema-name], strict: true, schema: [schema-obj] } }

4. Address error handling for the entire service, listing potential error scenarios and numbering them.

Based on your analysis, create a comprehensive implementation guide. The guide should be written in Markdown format and have the following structure:
1. Service description
2. Constructor description
3. Public methods and fields
4. Private methods and fields
5. Error handling
6. Security considerations
7. Step-by-step implementation plan

Make sure that the implementation plan:
1. Is tailored to the specific technology stack
2. Covers all relevant OpenRouter service components
3. Covers error handling and security best practices
4. Provides clear instructions on how to implement key methods and features
5. Explains how to configure the system message, user message, response_format (JSON schema), model name, and model parameters.

Uses appropriate Markdown formatting for better readability. The final output should consist solely of the implementation guide in Markdown format and should not duplicate or repeat any of the work done in the implementation breakdown section.

Save the implementation guide in .ai/prompts/api/openrouter-service-implementation-plan.md