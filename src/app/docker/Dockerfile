FROM node:23-alpine AS build

WORKDIR /app

ARG VITE_API_URL
ARG VITE_FIREBASE_USE_EMULATOR
ARG VITE_FIREBASE_AUTH_EMULATOR_HOST
ARG VITE_FIREBASE_PROJECT_ID
ARG VITE_FIREBASE_API_KEY
ARG VITE_FIREBASE_AUTH_DOMAIN
ARG VITE_FIREBASE_STORAGE_BUCKET
ARG VITE_FIREBASE_MESSAGING_SENDER_ID
ARG VITE_FIREBASE_APP_ID

COPY package.json ./
COPY yarn.lock* ./
RUN yarn install --frozen-lockfile

COPY . .
RUN VITE_API_URL=$VITE_API_URL \
  VITE_FIREBASE_PROJECT_ID=$VITE_FIREBASE_PROJECT_ID \
  VITE_FIREBASE_API_KEY=$VITE_FIREBASE_API_KEY \
  VITE_FIREBASE_AUTH_DOMAIN=$VITE_FIREBASE_AUTH_DOMAIN \
  VITE_FIREBASE_STORAGE_BUCKET=$VITE_FIREBASE_STORAGE_BUCKET \
  VITE_FIREBASE_MESSAGING_SENDER_ID=$VITE_FIREBASE_MESSAGING_SENDER_ID \
  VITE_FIREBASE_APP_ID=$VITE_FIREBASE_APP_ID \
  VITE_FIREBASE_USE_EMULATOR=$VITE_FIREBASE_USE_EMULATOR \
  VITE_FIREBASE_AUTH_EMULATOR_HOST=$VITE_FIREBASE_AUTH_EMULATOR_HOST \
  yarn build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
