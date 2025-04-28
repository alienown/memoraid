Your task is to implement a frontend view based on the provided implementation plan and implementation rules. Your goal is to create a detailed and accurate implementation that follows the provided plan, correctly represents the component structure, integrates with the API, and supports all specified user interactions.

First review the implementation plan:

<implementation_plan>
{{implementation-plan}} <- replace with a reference to the view implementation plan (e.g. #generations-view-implementation-plan.md)
</implementation_plan>

Now review the implementation rules:

<implementation_rules>
{{frontend-rules}} <- replace with a reference to the frontend rules (e.g. @shared.mdc, @frontend.mdc, @astro.mdc, @react.mdc, @ui-shadcn-helper.mdc)
</implementation_rules>

Review the defined types:

<types>
{{types}} <- replace with a reference to the DTO definitions (e.g. @types.ts)
</types>

Implement the plan according to the following approach:

<implementation_approach>
Carry out a maximum of 3 steps of the implementation plan, briefly summarize what you have done and describe the plan for the next 3 actions - stop work at this point and wait for my feedback.
</implementation_approach>

Carefully analyze the implementation plan and rules. Pay special attention to the component structure, API integration requirements and user interactions described in the plan.

Follow these steps to implement the frontend view:

1. Component structure:

- Identify all components listed in the implementation plan.
- Create a hierarchical structure of these components.
- Make sure that the responsibilities and relationships of each component are clearly defined.

2. API integration:

- Identify all API endpoints listed in the plan.
- Implement the necessary API calls for each endpoint.
- Handle API responses and update component state accordingly.

3. User Interactions:

- List all user interactions specified in the implementation plan.
- Implement event handlers for each interaction.
- Ensure that each interaction triggers the appropriate action or state change.

4. State Management:

- Identify the required state for each component.
- Implement state management using the appropriate method (local state, custom hook, shared state).
- Ensure that state changes trigger any necessary re-rendering.

5. Styling and Layout:

- Apply the specified styling and layout as mentioned in the implementation plan.
- Ensure responsiveness if required by the plan.

6. Error Handling and Edge Cases:

- Implement error handling for API calls and user interactions.
- Consider and handle potential edge cases listed in the plan.

7. Performance Optimization:

- Implement any performance optimizations specified in the plan or policy.
- Ensure efficient rendering and minimal unnecessary re-renders.

8. Testing:

- If specified in the plan, implement unit tests for components and features.
- Thoroughly test all user interactions and API integrations.

Throughout the implementation process, strictly adhere to the provided implementation policies. These policies take precedence over any general best practices that may conflict with them.

Ensure that your implementation accurately reflects the provided implementation plan and adheres to all specified policies. Pay special attention to component structure, API integration, and handling of user interactions.