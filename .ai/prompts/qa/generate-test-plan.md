You are a senior QA engineer. Based on the provided <codebase></codebase> and <tech-stack></tech-stack> generate a high-quality, professional test plan tailored to the project. Follow an industry-standard QA planning process.

<codebase>
{{codebase}}
</codebase>

<tech-stack>
{{tech_stack}}
</tech-stack>

Address the following elements in detail:

1. **Test Objectives**  
   Clearly define the goals of testing in the context of the project. What quality attributes (e.g. reliability, performance, usability) are most critical?

2. **Scope of Testing**  
   Identify what will be tested and what will be explicitly out of scope, based on the system boundaries and project priorities.

3. **Assumptions and Constraints**  
   Document assumptions about the project context, team, timelines, or tools. Highlight any limitations introduced by the architecture or development environment.

4. **Test Strategy**  
   Describe the high-level approach to testing. Base this on the system design, deployment model, and development practices. Include rationale behind the approach (e.g., risk-based, shift-left, exploratory).

5. **Test Design and Coverage Approach**  
   Outline how test cases will be derived (e.g., requirements-based, code-path analysis, model-based testing). Align test design techniques with the complexity of the system and desired coverage goals.

6. **Environment and Infrastructure**  
   Specify environments required (dev, staging, pre-prod), test data needs, and how they align with the overall system architecture. Consider containerization, cloud dependencies, and infrastructure-as-code if relevant.

7. **Test Tools and Frameworks**  
   Select appropriate tools for test automation, test management, mocking, data generation, and reporting — consistent with the given tech stack.

8. **Entry and Exit Criteria**  
   Define measurable conditions for starting and completing test phases. Include readiness criteria for builds, environments, and test assets.

9. **Risk Assessment**  
   Identify key technical and process risks. Include mitigation strategies, especially for fragile or high-impact areas in the codebase.

10. **Test Schedule and Resource Planning**  
    Provide a realistic timeline and resource plan, including effort estimates and any dependencies on development or infrastructure teams.

Keep in mind that we are in a MVP stage, so focus on delivering a test plan that is pragmatic, easy to implement, and achievable within the constraints of the project. Prioritize high-impact areas and ensure that the plan is adaptable to changes in scope or requirements as the project evolves.

Deliver a plan that demonstrates strong analytical thinking, technical depth, and alignment with modern software engineering practices. Avoid generic recommendations; tailor everything to the specific characteristics of the project.

Save the test plan in a markdown file /.ai/test-plan.md. Do not include your thought process in the final file. The output should be a clean, professional document ready for review by the development and product teams.