CREATE TABLE IF NOT EXISTS flashcard_ai_generations (
    id BIGSERIAL CONSTRAINT flashcard_ai_generations_id_pkey PRIMARY KEY,
    user_id VARCHAR(200) NOT NULL,
    ai_model VARCHAR(100) NOT NULL,
    source_text VARCHAR(10000) NOT NULL,
    all_flashcards_count INTEGER NOT NULL,
    accepted_unedited_flashcards_count INTEGER NULL,
    accepted_edited_flashcards_count INTEGER NULL,
    created_on TIMESTAMP NOT NULL,
    created_by VARCHAR(200) NOT NULL,
    last_modified_on TIMESTAMP NULL,
    last_modified_by VARCHAR(200) NULL
);