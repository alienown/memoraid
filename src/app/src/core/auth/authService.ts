import { FirebaseError, initializeApp } from "firebase/app";
import {
  connectAuthEmulator,
  getAuth,
  createUserWithEmailAndPassword,
  signInWithEmailAndPassword,
  signOut,
} from "firebase/auth";

const firebaseConfig = {
  apiKey: import.meta.env.VITE_FIREBASE_API_KEY || "apiKey",
  authDomain: import.meta.env.VITE_FIREBASE_AUTH_DOMAIN,
  projectId: import.meta.env.VITE_FIREBASE_PROJECT_ID,
  storageBucket: import.meta.env.VITE_FIREBASE_STORAGE_BUCKET,
  messagingSenderId: import.meta.env.VITE_FIREBASE_MESSAGING_SENDER_ID,
  appId: import.meta.env.VITE_FIREBASE_APP_ID,
};

const app = initializeApp(firebaseConfig);

const auth = getAuth(app);

const useEmulator = import.meta.env.VITE_FIREBASE_USE_EMULATOR === "true";

if (useEmulator) {
  connectAuthEmulator(auth, import.meta.env.VITE_FIREBASE_AUTH_EMULATOR_HOST, {
    disableWarnings: true,
  });
}

export type AuthResponse = {
  isSuccess: boolean;
  error?: string;
};

export const handleAuthError = (error: unknown): AuthResponse => {
  if (!(error instanceof FirebaseError)) {
    return {
      isSuccess: false,
      error: "Something went wrong",
    };
  }

  const errorCodeToMessageDict: Record<string, string> = {
    "auth/user-not-found": "Invalid email or password",
    "auth/email-already-in-use": "Email is already in use",
    "auth/email-already-exists": "Email is already in use",
    "auth/weak-password": "Password should be at least 6 characters",
  };

  const message = errorCodeToMessageDict[error.code] || "Something went wrong";

  return {
    isSuccess: false,
    error: message,
  };
};

export const register = async (
  email: string,
  password: string
): Promise<AuthResponse> => {
  try {
    await createUserWithEmailAndPassword(auth, email, password);

    return { isSuccess: true };
  } catch (error) {
    return handleAuthError(error);
  }
};

export const login = async (
  email: string,
  password: string
): Promise<AuthResponse> => {
  try {
    await signInWithEmailAndPassword(auth, email, password);

    return { isSuccess: true };
  } catch (error) {
    return handleAuthError(error);
  }
};

export const logout = async () => {
  await signOut(auth);
};

export const onAuthStateChanged = (
  callback: (isAuthenticated: boolean) => void
) => {
  return auth.onAuthStateChanged((user) => {
    callback(!!user);
  });
};

export const getToken = async () => {
  return await auth.currentUser?.getIdToken();
};
