DO $$

DECLARE user_id BIGINT;
DECLARE flashcard_ai_generation_id BIGINT;
DECLARE created_by CHAR(11) := 'Seed script';

BEGIN
    -- 1. Insert user
    INSERT INTO users (email, password, created_on, created_by)
    VALUES ('test@example.com', '123', CURRENT_TIMESTAMP, created_by)
    RETURNING id INTO user_id;

    -- 2. Insert AI generation
    INSERT INTO flashcard_ai_generations (
        user_id, 
        ai_model, 
        source_text, 
        all_flashcards_count, 
        accepted_unedited_flashcards_count, 
        accepted_edited_flashcards_count, 
        created_on, 
        created_by
    )
    VALUES (
        user_id, 
        'Mock', 
        'This is a sample text for AI flashcard generation. The mitochondria is the powerhouse of the cell. Water boils at 100 degrees Celsius. The Earth revolves around the Sun.', 
        5, 
        1, 
        1, 
        CURRENT_TIMESTAMP, 
        created_by
    )
    RETURNING id INTO flashcard_ai_generation_id;

    -- 3. Insert flashcards
    -- Manual flashcard
    INSERT INTO flashcards (
        user_id, 
        flashcard_ai_generation_id, 
        front, 
        back, 
        source, 
        created_on, 
        created_by
    )
    VALUES (
        user_id, 
        NULL, 
        'What is the capital of France?', 
        'Paris', 
        'Manual', 
        CURRENT_TIMESTAMP, 
        created_by
    );

    -- AI Full flashcard (unedited)
    INSERT INTO flashcards (
        user_id, 
        flashcard_ai_generation_id, 
        front, 
        back, 
        source, 
        created_on, 
        created_by
    )
    VALUES (
        user_id, 
        flashcard_ai_generation_id, 
        'What is the main function of mitochondria in a cell?', 
        'The powerhouse of the cell', 
        'AIFull', 
        CURRENT_TIMESTAMP, 
        created_by
    );

    -- AI Edited flashcard
    INSERT INTO flashcards (
        user_id, 
        flashcard_ai_generation_id, 
        front, 
        back, 
        source, 
        created_on, 
        created_by
    )
    VALUES (
        user_id, 
        flashcard_ai_generation_id, 
        'At what temperature does water boil at standard pressure?', 
        '100 degrees Celsius', 
        'AIEdited', 
        CURRENT_TIMESTAMP, 
        created_by
    );
END $$;