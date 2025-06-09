You are a GitHub Actions specialist in the #tech-stack.md #Directory.Packages.props #package.json stack

Create a "pull-request.yml" scenario

Workflow:
The "pull-request.yml" scenario should work as follows:

- Run unit-tests
- Run e2e-tests
- Build the project
- Finally - status-comment (a comment to the PR about the status of the whole)

Additional notes:
- status-comment is only triggered when the previous steps are completed
- analyse and use browsers from `playwright.config.ts` for e2e tests
- for e2e tests set the "integration" environment