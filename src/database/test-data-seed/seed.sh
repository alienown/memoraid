#!/bin/bash
set -e

export PGPASSWORD="$POSTGRES_PASSWORD"

# Execute the seed script
psql -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f /scripts/seed.sql

echo "Database seeding completed successfully!"
