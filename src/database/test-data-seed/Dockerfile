FROM alpine:latest

# Install PostgreSQL client and bash
RUN apk add --no-cache postgresql-client bash

# Copy necessary files
COPY ./scripts /scripts
COPY ./seed.sh /seed.sh

# Make the script executable
RUN chmod +x /seed.sh

ENTRYPOINT ["/seed.sh"]
