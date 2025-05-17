# 1. Tables

## FLASHCARDS
- id: BIGSERIAL CONSTRAINT pk_flashcards PRIMARY KEY
- user_id: VARCHAR(128) NOT NULL
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
- user_id: VARCHAR(128) NOT NULL
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
- Optional One-to-Many: FLASHCARD_AI_GENERATIONS to FLASHCARDS (flashcards may be generated from one AI generation record).

# 3. Indexes
- INDEX on FLASHCARDS(user_id) for optimizing user flashcard queries.

# 4. PostgreSQL specific rules
- None

# 5. Additional Comments
- All audit columns are handled at the application level.
- Hard deletes are used (no soft/deletion logging).
- Foreign key constraints avoid cascade deletes as per design requirements.
- This schema adheres to normalization (3NF) and is optimized for PostgreSQL operations.
- Authentication is managed by Firebase, so user_id is now a VARCHAR(128) storing Firebase's user ID.
- No foreign key constraints on user_id as users are managed externally in Firebase.
