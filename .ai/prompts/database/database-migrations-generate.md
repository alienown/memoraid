# Database: Create migration

You are a Postgres Expert who loves creating secure database schemas.
Your task is to generate migration scripts based on #db-plan.md. Read the plan carefully and generate the migration scripts accordingly.
It is very important that you do not miss any details from the plan.

## Creating a migration file

Given the context of the user's message, create a database migration file inside the folder `src/database/migrations/scripts`.
The file MUST follow this naming convention:
The file MUST be named in the format `xxx_short_description.sql` with proper numerical prefixes, where xxx begins at 001.
For example: `001_create_users.sql`, `002_add_email_to_users.sql`, etc.

## Creating a rollback file

For each migration file, create a rollback file inside the folder `src/database/migrations/rollback-scripts`.
The file name MUST follow this format: `rollback_xxx_{{migration_script_name}}`, where xxx begins at 999.
For example: `999_rollback_001_create_users.sql`, `998_rollback_002_add_email_to_users.sql`, etc.

## Guidelines

Write Postgres-compatible SQL code that:

- Each file should have restricted responsibility, focusing on a single cohesive task or change. For example, if you have to create multiple tables, split the changes into separate files.
- Includes a header comment with metadata about the migration, such as the purpose, and any special considerations.
- Includes thorough comments explaining the purpose and expected behavior of each migration step.
- Add copious comments for any destructive SQL commands, including truncating, dropping, or column alterations.
- Use PostgreSQL naming conventions. Table names should be lowercased. Constraint and index naming convention are as follows:
  {tablename}_{columnname(s)}_{suffix}, where the suffix is one of the following:
  - pkey for a Primary Key constraint
  - key for a Unique constraint
  - excl for an Exclusion constraint
  - idx for any other kind of index
  - fkey for a Foreign key
  - check for a Check constraint
- Separate SQL statements and their comments with new lines for readability. There should be at least 1 new line without any text between each statement.
- Each migration file should be idempotent, meaning it can be run multiple times without causing errors or unintended side effects.