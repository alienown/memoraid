-- Rollback: Drop index and FLASHCARD_AI_GENERATIONS table.
DROP INDEX IF EXISTS flashcard_ai_generations_user_id_idx;

DROP TABLE IF EXISTS flashcard_ai_generations;