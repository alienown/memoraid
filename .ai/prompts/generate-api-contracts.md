Given the API plan #api-plan.md, generate .NET API contracts for the memoraid application.

Follow these rules:
- Generate request classes only for endpoints that accept a body. Request class should not contain any properties that are not in the request body (e.g. query or route parameters).
- Generate response classes for all endpoints that return a body.
- All classes should be public. No additional modifiers are needed.
- Create constructors for response classes. Do not create constructors for request classes.
- Each property of a response class should only have public getters and no setters. Request class properties should have public getters and setters.
- Use nullable types for all properties of request class. Use nullable types for optional properties of response class.
- Add Request and Response suffixes to class names, e.g. CreateFlashcardRequest, CreateFlashcardResponse, ChangeUserPasswordRequest, ChangeUserPasswordResponse, etc.
- Keep each request and response class in a separate file. If the request or response class has sub-classes, put them in the same file as the parent class.
- Keep a new line between group of properties and constructor. Do not separate properties with new lines.
- Do not include any comments in the generated code.
- Put contract classes in appropriate API catalogs (requests and responses). Do not create new catalogs if they already exist.
- Use IReadOnlyList<T> for collections in request and response classes

Before creating contracts, analyse loudly which endpoints require request and response classes separately. Only then generate the classes.
