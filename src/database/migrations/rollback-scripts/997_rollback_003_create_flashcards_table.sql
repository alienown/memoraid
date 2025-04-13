-- Rollback: Drop indexes and FLASHCARDS table.
DROP INDEX IF EXISTS flashcards_flashcard_ai_generation_id_idx;

DROP INDEX IF EXISTS flashcards_user_id_idx;

DROP TABLE IF EXISTS flashcards;