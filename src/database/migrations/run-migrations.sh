#!/bin/bash
set -e

export PGPASSWORD="$POSTGRES_PASSWORD"

echo "Executing migration scripts..."

DB_EXISTS=$(psql -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -d postgres -tc "SELECT 1 FROM pg_database WHERE datname = '$POSTGRES_DB'" | grep -q 1 && echo "true" || echo "false")

if [ "$DB_EXISTS" = "true" ]; then
  echo "Database exists - applying migrations..."
else
  psql -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -d postgres -c "CREATE DATABASE $POSTGRES_DB"
  echo "Database created - applying migrations..."
fi

for migration in /scripts/*.sql; do
  echo "Applying migration: $(basename $migration)"
  psql -h "$POSTGRES_HOST" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f "$migration"
done

echo "All migrations applied successfully!"