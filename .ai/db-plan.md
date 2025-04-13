# 1. Tables

## USERS
- id: BIGSERIAL CONSTRAINT pk_users PRIMARY KEY
- email: VARCHAR(255) NOT NULL CONSTRAINT uq_users_email UNIQUE
- password: VARCHAR(255) NOT NULL
- created_on: TIMESTAMP NOT NULL
- created_by: VARCHAR(255) NOT NULL
- last_modified_on: TIMESTAMP NULL
- last_modified_by: VARCHAR(255) NULL

## FLASHCARDS
- id: BIGSERIAL CONSTRAINT pk_flashcards PRIMARY KEY
- user_id: INTEGER NOT NULL CONSTRAINT fk_flashcards_user_id FOREIGN KEY REFERENCES USERS(id) ON DELETE NO ACTION
- flashcard_ai_generation_id: INTEGER NULL CONSTRAINT fk_flashcards_flashcard_ai_generation_id FOREIGN KEY REFERENCES FLASHCARD_AI_GENERATIONS(id) ON DELETE NO ACTION
- front: VARCHAR(500) NOT NULL
- back: VARCHAR(200) NOT NULL
- source: VARCHAR(20) NOT NULL CHECK (source IN ('Manual', 'AIFull', 'AIEdited'))
- created_on: TIMESTAMP NOT NULL
- created_by: VARCHAR(255) NOT NULL
- last_modified_on: TIMESTAMP NULL
- last_modified_by: VARCHAR(255) NULL

## FLASHCARD_AI_GENERATIONS
- id: BIGSERIAL CONSTRAINT pk_flashcard_ai_generations PRIMARY KEY
- user_id: INTEGER NOT NULL CONSTRAINT fk_flashcard_ai_generations_user_id FOREIGN KEY REFERENCES USERS(id) ON DELETE NO ACTION
- ai_model: VARCHAR(100) NOT NULL
- source_text: VARCHAR(10000) NOT NULL
- all_flashcards_count: INTEGER NOT NULL
- accepted_unedited_flashcards_count: INTEGER NULL
- accepted_edited_flashcards_count: INTEGER NULL
- created_on: TIMESTAMP NOT NULL
- created_by: VARCHAR(255) NOT NULL
- last_modified_on: TIMESTAMP NULL
- last_modified_by: VARCHAR(255) NULL

# 2. Relationships
- One-to-Many: USERS to FLASHCARDS (each flashcard is linked to one user).
- Optional One-to-Many: FLASHCARD_AI_GENERATIONS to FLASHCARDS (flashcards may be generated from one AI generation record).
- One-to-Many: USERS to FLASHCARD_AI_GENERATIONS.

# 3. Indexes
- UNIQUE index on USERS(email) [automatically created by UNIQUE constraint].
- INDEX on FLASHCARDS(user_id) for optimizing user flashcard queries.
- INDEX on FLASHCARDS(ai_generation_id) for efficient joins.
- INDEX on FLASHCARD_AI_GENERATIONS(user_id) for optimizing generation queries.

# 4. PostgreSQL specific rules
- None

# 5. Additional Comments
- All audit columns are handled at the application level.
- Hard deletes are used (no soft/deletion logging).
- Foreign key constraints avoid cascade deletes as per design requirements.
- This schema adheres to normalization (3NF) and is optimized for PostgreSQL operations.
