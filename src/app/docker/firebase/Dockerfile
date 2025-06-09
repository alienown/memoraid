FROM andreysenov/firebase-tools

USER root

RUN apt-get update && apt-get install -y curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

USER node