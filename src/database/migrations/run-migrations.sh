#!/bin/bash
set -e

# Export PGPASSWORD environment variable to avoid manual password entry
export PGPASSWORD="$POSTGRES_PASSWORD"

# Function to check if postgres is ready
postgres_ready() {
  pg_isready -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -q
}

# Wait for postgres to become ready
until postgres_ready; do
  echo "PostgreSQL is unavailable - sleeping..."
  sleep 2
done

echo "PostgreSQL is up - executing migrations script..."

# Check if database exists and create it if it doesn't
DB_EXISTS=$(psql -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -tc "SELECT 1 FROM pg_database WHERE datname = '$POSTGRES_DB'" | grep -q 1 && echo "true" || echo "false")

if [ "$DB_EXISTS" = "true" ]; then
  echo "Database exists - applying migrations..."
else
  psql -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -c "CREATE DATABASE $POSTGRES_DB"
  echo "Database created - applying migrations..."
fi

# Apply migrations in order
for migration in /scripts/*.sql; do
  echo "Applying migration: $(basename $migration)"
  psql -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f "$migration"
done

echo "All migrations applied successfully!"
