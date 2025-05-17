CREATE TABLE IF NOT EXISTS flashcards (
    id BIGSERIAL CONSTRAINT flashcards_id_pkey PRIMARY KEY,
    user_id VARCHAR(200) NOT NULL,
    flashcard_ai_generation_id BIGINT NULL,
    front VARCHAR(500) NOT NULL,
    back VARCHAR(200) NOT NULL,
    source VARCHAR(20) NOT NULL,
    created_on TIMESTAMP NOT NULL,
    created_by VARCHAR(200) NOT NULL,
    last_modified_on TIMESTAMP NULL,
    last_modified_by VARCHAR(200) NULL,
    CONSTRAINT flashcards_source_check CHECK (source IN ('Manual', 'AIFull', 'AIEdited')),
    CONSTRAINT flashcards_flashcard_ai_generation_id_fkey FOREIGN KEY (flashcard_ai_generation_id) REFERENCES flashcard_ai_generations (id) ON DELETE NO ACTION
);

CREATE INDEX IF NOT EXISTS flashcards_user_id_idx ON flashcards (user_id);