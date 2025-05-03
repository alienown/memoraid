# OpenRouter Service Implementation Guide

## 1. Service Description
The OpenRouter service is a backend component that bridges the Memoraid application with the OpenRouter API. It handles chat completions by sending a full conversation payload (including system, user, and assistant messages) along with a specified JSON schema.

## 2. Constructor Description
- **Dependency Injection:** Inject configuration settings and HttpClient.
- **Initialization:** Load API token from ApplicationOptions.

## 3. Public Methods and Fields
- **Methods:**
  1. T CompleteAsync<T>(RequestPayload payload): Sends conversation history along with json schema to the OpenRouter API and returns the parsed response as type T.
- **Fields:**
  - ApiEndpoint: URL for the OpenRouter API.
  - HttpClient: Instance for handling HTTP requests.
  - DefaultModelName: The default model to use.

## 4. Private Methods and Fields
- **Methods:**
  1. ParseApiResponse<T>(string rawResponse): A private helper that parses the raw JSON response using the provided JSON schema from RequestPayload.
  2. BuildHeaders(): Creates necessary HTTP headers, including authentication tokens.
  3. HandleHttpError(HttpResponseMessage response): Processes non-successful HTTP responses.
- **Fields:**
  - Private constants for timeout values and other configuration settings.

## 5. RequestPayload Structure
The RequestPayload must include:
1. Model name.
2. Messages: A list of chat messages, each with a role (system, user, assistant) and content.
3. JsonSchema: The JSON schema definition to enforce response validation and parsing.
Example payload sent to open router API:
{
    model: 'openai/gpt-4',
    messages: [
      { role: 'user', content: 'What is the weather like in London?' },
    ],
    response_format: {
      type: 'json_schema',
      json_schema: {
        name: 'weather',
        strict: true,
        schema: {
          type: 'object',
          properties: {
            location: {
              type: 'string',
              description: 'City or location name',
            },
            temperature: {
              type: 'number',
              description: 'Temperature in Celsius',
            },
            conditions: {
              type: 'string',
              description: 'Weather conditions description',
            },
          },
          required: ['location', 'temperature', 'conditions'],
          additionalProperties: false,
        },
      },
    },
}

## 6. Error Handling
Error scenarios to consider:
1. HTTP Request Errors: Timeouts, network failures, non-success status codes.
2. JSON Parsing Errors: Malformed responses or schema mismatches.
3. API-Specific Errors: Errors returned by OpenRouter.
4. Validation Errors: Issues with the RequestPayload structure or content:
    - Invalid message roles (must be 'system', 'user', or 'assistant')
Implement consistent error propagation with clear messages.

## 7. Security Considerations
- Utilize api token from configuration.
- Employ TLS via HttpClient.
- Inject services with an appropriate lifetime.

## 8. Step-by-Step Implementation Plan
1. **Configuration:**
   - Make sure OpenRouter API key and base API URL are stored in ApplicationOptions and appsettings.json.
2. **HTTP Client:**
   - Register HttpClient in the DI container with a base address pointing to the OpenRouter API endpoint.
3. **Service Skeleton:**
   - Create the OpenRouterService class under `/src/api/Memoraid.WebApi/Services` with dependency injection.
4. **RequestPayload Definition:**
   - Ensure RequestPayload contains required fields: Model, Messages, and JsonSchema.
   - Correct usage of NRT (Nullable Reference Types) for RequestPayload properties.
5. **Implement Public Method:**
   - Develop CompleteAsync<T>(RequestPayload payload) to:
     a. Build HTTP headers via BuildHeaders.
     b. Use HttpClient to POST the payload.
     c. Call ParseApiResponse<T> to validate and deserialize the response.
6. **Private Methods:**
   - Implement ParseApiResponse<T>(string rawResponse) for validating and converting the JSON response into type T.
   - Maintain BuildHeaders and HandleHttpError as private methods.
7. **Testing:**
   - Write unit tests (using NUnit and Shouldly) to verify:
     a. CompleteAsync<T> correctly returns expected type T.
     b. Handling of success and error responses.
     c. Validation of RequestPayload structure.

This guide outlines the adjusted service design, ensuring the caller receives a fully parsed response of type T while maintaining a simplified public API.
