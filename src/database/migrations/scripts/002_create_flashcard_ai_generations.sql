-- Migration: Create FLASHCARD_AI_GENERATIONS table with its columns and foreign key to users.
CREATE TABLE IF NOT EXISTS flashcard_ai_generations (
    id BIGSERIAL CONSTRAINT flashcard_ai_generations_id_pkey PRIMARY KEY,
    user_id INTEGER NOT NULL,
    ai_model VARCHAR(100) NOT NULL,
    source_text VARCHAR(10000) NOT NULL,
    all_flashcards_count INTEGER NOT NULL,
    accepted_unedited_flashcards_count INTEGER NULL,
    accepted_edited_flashcards_count INTEGER NULL,
    created_on TIMESTAMP NOT NULL,
    created_by VARCHAR(255) NOT NULL,
    last_modified_on TIMESTAMP NULL,
    last_modified_by VARCHAR(255) NULL,
    CONSTRAINT flashcard_ai_generations_user_id_fkey FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE NO ACTION
);

-- Create index for optimizing user queries.
CREATE INDEX IF NOT EXISTS flashcard_ai_generations_user_id_idx ON flashcard_ai_generations (user_id);