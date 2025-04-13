-- Migration: Create FLASHCARDS table with proper foreign keys and check constraints.
CREATE TABLE IF NOT EXISTS flashcards (
    id BIGSERIAL CONSTRAINT flashcards_id_pkey PRIMARY KEY,
    user_id BIGINT NOT NULL,
    flashcard_ai_generation_id BIGINT NULL,
    front VARCHAR(500) NOT NULL,
    back VARCHAR(200) NOT NULL,
    source VARCHAR(20) NOT NULL,
    created_on TIMESTAMP NOT NULL,
    created_by VARCHAR(255) NOT NULL,
    last_modified_on TIMESTAMP NULL,
    last_modified_by VARCHAR(255) NULL,
    CONSTRAINT flashcards_source_check CHECK (source IN ('Manual', 'AI', 'AIEdited')),
    CONSTRAINT flashcards_user_id_fkey FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE NO ACTION,
    CONSTRAINT flashcards_flashcard_ai_generation_id_fkey FOREIGN KEY (flashcard_ai_generation_id) REFERENCES flashcard_ai_generations (id) ON DELETE NO ACTION
);

-- Create indexes to optimize query performance.
CREATE INDEX IF NOT EXISTS flashcards_user_id_idx ON flashcards (user_id);

CREATE INDEX IF NOT EXISTS flashcards_flashcard_ai_generation_id_idx ON flashcards (flashcard_ai_generation_id);