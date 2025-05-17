# Authentication Refactor Implementation Plan for Memoraid Frontend

## Overview

This document outlines a plan to refactor the authentication mechanism in the Memoraid frontend application. The goal is to centralize authentication logic, improve maintainability, and create a more robust architecture while adhering to project constraints.

## Current Implementation

The current authentication system in Memoraid uses:
- Firebase Authentication for user operations
- JWT tokens stored in localStorage
- React Context API for global auth state
- Protected Routes with manual redirects
- Axios interceptors for handling 401 responses

## Identified Problems

1. **Scattered Authentication Logic**:
   - Firebase authentication functions are defined in `firebase.ts`
   - Token management is in `tokenStorage.ts`
   - Authentication state is managed in `AuthProvider.tsx`

2. **Inconsistent Error Handling**:
   - Duplicated error handling in Login and Registration components
   - Different approaches between Firebase auth errors and API call errors

3. **No Clear Authentication Flow**:
   - Multiple entry points for authentication (Login and Registration)
   - No centralized mechanism for auth events

4. **Tight Coupling with Firebase**:
   - Direct Firebase calls across components
   - Difficult to switch auth providers if needed

## Implementation Details

### 1. Create a Centralized Authentication Service

Create an authentication service that will serve as the single entry point for all authentication operations.

```typescript
// src/services/auth/authService.ts
export interface AuthService {
  login(email: string, password: string): Promise<AuthResult>;
  register(email: string, password: string): Promise<AuthResult>;
  logout(): Promise<void>;
  isAuthenticated(): boolean;
  getCurrentToken(): string | null;
  onAuthStateChange(callback: (isAuthenticated: boolean) => void): () => void;
}
```

The Firebase implementation would encapsulate all Firebase-specific logic:

```typescript
// src/services/auth/firebaseAuthService.ts
export class FirebaseAuthService implements AuthService {
  // Implementation that uses Firebase auth and the tokenStorage
}
```

### 2. Enhance Token Management

Keep using localStorage but encapsulate all token-related operations in a dedicated service:

```typescript
// src/services/auth/tokenService.ts
export interface TokenService {
  setToken(token: string): void;
  getToken(): string | null;
  removeToken(): void;
  hasToken(): boolean;
}

export class LocalStorageTokenService implements TokenService {
  // Implementation using localStorage (similar to current tokenStorage)
}
```

### 3. Create a Robust Authentication Context

Refactor the `AuthProvider` to use the new authentication service. `AuthContext` should only hold a isAuthenticated flag.

```typescript
// src/services/auth/AuthProvider.tsx
export function AuthProvider({ children }: AuthProviderProps) {
  // Use the authentication service instead of direct Firebase calls and tokenStorage
  const authService = useAuthService();
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(
    authService.isAuthenticated()
  );
  
  // Subscribe to auth state changes
  useEffect(() => {
    return authService.onAuthStateChange((newState) => {
      setIsAuthenticated(newState);
    });
  }, [authService]);
  
  // Rest of implementation
}
```

### 4. Create Custom Authentication Hooks

Create hooks that encapsulate common auth-related operations:

```typescript
// src/lib/auth/useAuth.ts
export function useAuth() {
  const authService = useAuthService();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const login = async (email: string, password: string) => {
    setIsLoading(true);
    setError(null);
    try {
      await authService.login(email, password);
    } catch (error) {
      const errorMessage = handleAuthError(error);
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setIsLoading(false);
    }
  };
  
  // Similar implementations for register, logout
  
  return { login, register, logout, isLoading, error };
}
```

### 5. Update HTTP Client Interceptors

Update the Axios interceptors to use the authentication service:

```typescript
// src/lib/axiosInterceptor.ts
export function setupInterceptors(authService: AuthService) {
  axiosInstance.interceptors.request.use((config) => {
    const token = authService.getCurrentToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });
  
  axiosInstance.interceptors.response.use(
    (response) => response,
    async (error) => {
      if (error.response?.status === 401) {
        await authService.logout();
      }
      return Promise.reject(error);
    }
  );
}
```

## Implementation Steps

1. **Create Base Files and Interfaces**: Create the interface files for the auth service and token service.

2. **Implement Token Service**: Create the LocalStorageTokenService implementation to replace direct tokenStorage usage.

3. **Implement Auth Service**: Create the FirebaseAuthService implementation that uses the TokenService and Firebase.

4. **Update Auth Context**: Refactor the AuthProvider to use the auth service and subscribe to auth events.

5. **Create Authentication Hooks**: Implement useAuth and other needed hooks.

6. **Update Axios Interceptors**: Modify the existing interceptors to use the auth service.

7. **Refactor Components**: Update Login, Registration, and other components to use the new auth hooks.

## Directory Structure Changes

Current:
```
/src/lib/auth/
  - AuthContext.tsx
  - AuthProvider.tsx
  - useAuth.ts
/src/lib/
  - tokenStorage.ts
  - firebase.ts (contains auth functions)
  - axiosInterceptor.ts
```

Proposed:
```
/src/services/auth/
  - authService.ts (interface)
  - firebaseAuthService.ts (implementation)
  - tokenService.ts (interface)
  - localStorageTokenService.ts (implementation)
  - useAuth.ts
  - AuthContext.tsx
  - AuthProvider.tsx
/src/lib/
  - axiosInterceptor.ts (updated)
  - firebase.ts (removed)
  - tokenStorage.ts (removed)
/src/lib/auth (removed)
  - AuthHandlerWrapper.tsx (removed)
  - AuthContext.tsx (removed)
  - AuthProvider.tsx (removed)
  - useAuth.ts (removed)
```

## Benefits

1. **Centralized Authentication Logic**: All authentication logic is in the auth service
2. **Decoupled Components**: Components use hooks instead of direct Firebase calls
3. **Abstracted Authentication Provider**: Easier switching of providers in the future
4. **Improved Error Handling**: Consistent error handling through the auth service
5. **Better Testability**: Services and hooks can be mocked for testing
