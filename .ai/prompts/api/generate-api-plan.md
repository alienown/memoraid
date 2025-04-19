<db-plan>
{{db-plan}} <- replace with a reference to #db-plan.md
<db-plan>

<prd>
{{prd}} <- replace with a reference to #prd.md
</prd>

<tech-stack>
{{tech-stack}} <- replace with a reference to #tech-stack.md
</tech-stack>

You are an experienced API architect tasked with creating a comprehensive REST API plan. Your plan will be based on the given database schema, product requirements document (PRD), and technology stack provided above. Carefully review the input data and perform the following steps:

1. Analyze the database schema:

- Identify the main entities (tables)
- Note the relationships between entities
- Consider any indexes that may affect the API design
- Note the validation conditions specified in the schema.

2. Analyze the PRD:

- Identify key features and functionality
- Note specific data operation requirements (fetch, create, update, delete)
- Identify business logic requirements that fall outside of CRUD operations

3. Consider the technology stack:

- Ensure the API plan is aligned with the specified technologies.
- Consider how those technologies might impact the API design

4. Create a comprehensive REST API plan:

- Define core resources based on database entities and PRD requirements
- Design CRUD endpoints for each resource
- Design endpoints for the business logic described in the PRD
- Include pagination, filtering, and sorting for list endpoints.
- Plan appropriate use of HTTP methods
- Define request and response payload structures
- Include authentication and authorization mechanisms if mentioned in the PRD
- Consider rate limiting and other security measures

Before delivering your final plan, work inside the <api_analysis> tags in your thinking block to break down your thought process and make sure you have covered all the necessary aspects. In this section:

1. List the major entities from the database schema. Number each entity and quote the appropriate part of the schema.
2. List the key business logic functions from the PRD. Number each function and quote the appropriate part of the PRD.
3. Map the functions from the PRD to potential API endpoints. For each function, consider at least two possible endpoint designs and explain which one you chose and why.
4. Consider and list any security and performance requirements. For each requirement, quote the part of the input documents that supports them.
5. Clearly map business logic from PRD to API endpoints.
6. Include validation conditions from database schema in API plan.

This section can be quite long.

The final API plan should be formatted in markdown and contain the following sections:

```markdown

# REST API Plan

## 1. Resources

- List each primary resource and its corresponding database table

## 2. Endpoints

For each resource, provide:

- HTTP method
- URL path
- Short description
- Query parameters (if applicable)
- JSON request payload structure (if applicable)
- JSON response payload structure
- Success codes and messages
- Error codes and messages

## 3. Authentication and authorization

- Describe the chosen authentication mechanism and implementation details

## 4. Validation and business logic

- List validation conditions for each resource
- Describe how the business logic is implemented in the API

```

Make sure your plan is comprehensive, well-structured, and addresses all aspects of the input materials. If you have to make assumptions because of unclear input, state them clearly in your analysis.

The final output should consist solely of the API plan in markdown English that you save in .ai/api-plan.md and should not duplicate or repeat any of the work done in the think block.