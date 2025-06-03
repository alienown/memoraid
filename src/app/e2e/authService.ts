import { initializeApp } from "firebase/app";
import {
  connectAuthEmulator,
  getAuth,
  signInWithEmailAndPassword,
} from "firebase/auth";

const firebaseConfig = {
  apiKey: process.env.VITE_FIREBASE_API_KEY || "apiKey",
  authDomain: process.env.VITE_FIREBASE_AUTH_DOMAIN,
  projectId: process.env.VITE_FIREBASE_PROJECT_ID,
  storageBucket: process.env.VITE_FIREBASE_STORAGE_BUCKET,
  messagingSenderId: process.env.VITE_FIREBASE_MESSAGING_SENDER_ID,
  appId: process.env.VITE_FIREBASE_APP_ID,
};

const app = initializeApp(firebaseConfig);

const auth = getAuth(app);

const useEmulator = process.env.FIREBASE_USE_EMULATOR === "true";

if (useEmulator) {
  connectAuthEmulator(auth, process.env.FIREBASE_AUTH_EMULATOR_HOST ?? "", {
    disableWarnings: true,
  });
}

export const login = async () => {
  const email = process.env.TEST_USER_EMAIL || "";
  const password = process.env.TEST_USER_PASSWORD || "";

  await signInWithEmailAndPassword(auth, email, password);
};

export const getToken = async () => {
  return await auth.currentUser?.getIdToken();
};
