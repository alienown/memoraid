As a senior frontend developer, your job is to create a detailed plan for implementing a new view in a web application. This plan should be comprehensive and clear enough for another frontend developer to implement the view correctly and efficiently.

First, review the following information:

1. Product Requirements Document (PRD):
<prd>
{{prd}} <- replace with a reference to the @prd.md file
</prd>

2. View Description:
<view_description>
{{view-description}} <- paste the description of the implemented view from ui-plan.md
</view_description>

3. User Stories:
<user_stories>
{{user-stories}} <- paste the user stories from @prd.md that will be addressed by the view
</user_stories>

4. Endpoint Description:
<endpoint_description>
{{endpoint-description}} <- paste the descriptions of the endpoints from api-plan.md that the view will use
</endpoint_description>

5. Backend API HTTP Client & Type definitions:
<endpoint_implementation>
{{http-client-and-type-definitions}} <- replace with a reference to the http client and type definitions
</endpoint_implementation>

6. Tech Stack:
<tech_stack>
{{tech-stack}} <- replace with a reference to the @tech-stack.md file
</tech_stack>

Before creating the final implementation plan, do some analysis and planning inside the <implementation_breakdown> tags in your thinking block. This section can be quite long because it's important to be thorough.

In your implementation breakdown, do the following:

1. For each input section (PRD, User Stories, Endpoint Description, Backend API HTTP Client & Type Definitions, Tech Stack):
  - Summarize key points
  - List any requirements or constraints
  - Highlight any potential challenges or important issues
2. Extract and list key requirements from the PRD
3. List all the major components needed, along with a brief description of their description, types needed, supported events, and validation conditions
4. Create a high-level component tree diagram
5. Identify the required DTOs and custom ViewModel types for each view component. Explain these new types in detail, breaking down their fields and related types.
6. Identify potential state variables and custom hooks, explaining their purpose and how they are used
7. List required API calls and their corresponding frontend actions
8. Map each user story to specific implementation details, components, or functions
9. List user interactions and their expected outcomes
10. List the conditions required by the API and how to verify them at the component level
11. Identify potential error scenarios and suggest how to handle them
12. List potential challenges in implementing this view and suggest possible solutions

After conducting the analysis, deliver an implementation plan in Markdown format with the following sections:

1. Overview: A brief summary of the view and its purpose.
2. View routing: Defining the path along which the view should be accessible.
3. Component structure: Outline the main components and their hierarchy.
4. Component Details: For each component, describe:
  - Description of the component, its purpose, and what it consists of
  - Main HTML elements and child components that build the component
  - Events handled
  - Validation conditions (detailed conditions, according to the API)
  - Types (DTOs and ViewModels) required by the component
  - Props that the component accepts from the parent (component interface)
5. Types: Detailed description of the types required to implement the view, including a detailed breakdown of any new types or view models by fields and types.
6. State Management: Detailed description of how state is managed in the view, whether a custom hook is required.
7. API Integration: Explain how to integrate with the provided endpoint. Precisely indicate the types of request and response.
8. User Interactions: Detailed description of user interactions and how they are handled.
9. Conditions and Validation: Describe what conditions are validated by the interface, which components they apply to, and how they affect the state of the interface
10. Error Handling: Describe how to handle potential errors or edge cases.
11. Implementation Steps: Step-by-step guide to implementing the view.

Make sure your plan is consistent with the PRD, user stories, and takes into account the provided technology stack.

The final results should be in English and written in a file named .ai/ui/{view-name}-view-implementation-plan.md. Do not include any analysis or planning in the final result.

Here is an example of what the output file should look like (content is replaceable):

```markdown
# View Implementation Plan [View Name]

## 1. Overview
[A short description of the view and its purpose]

## 2. View Routing
[The path where the view should be available]

## 3. Component Structure
[Outline of the main components and their hierarchy]

## 4. Component Details
### [Component Name 1]
- Component Description [description]
- Main Elements: [description]
- Supported Interactions: [list]
- Supported Validation: [list, detailed]
- Types: [list]
- Props: [list]

### [Component Name 2]
[...]

## 5. Types
[Detailed description of required types]

## 6. State Management
[Description of state management in the view]

## 7. API Integration
[Explaining the integration with the provided endpoint, indicating the types of request and response]

## 8. User Interactions
[Detailed description of user interactions]

## 9. Conditions and Validation
[Detailed description of conditions and their validation]

## 10. Handling errors
[Description of handling potential errors]

## 11. Implementation steps
1. [Step 1]
2. [Step 2]
3. [...]
```

Start analyzing and planning now. Your final output should consist solely of an implementation plan in English in markdown format, which you will save in the file .ai/prompts/ui/{view-name}-view-implementation-plan.md and should not duplicate or repeat any work done in the implementation split.