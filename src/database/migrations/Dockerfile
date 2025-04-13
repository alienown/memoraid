FROM alpine:latest

# Install PostgreSQL client and bash
RUN apk add --no-cache postgresql-client bash

# Copy necessary files
COPY ./run-migrations.sh /run-migrations.sh
COPY ./scripts /scripts

# Make the script executable
RUN chmod +x /run-migrations.sh

# Set the script as the entrypoint
ENTRYPOINT ["/run-migrations.sh"]
