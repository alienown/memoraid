-- Migration: Create USERS table with primary key, UNIQUE email constraint and audit columns.
CREATE TABLE IF NOT EXISTS users (
    id BIGSERIAL CONSTRAINT users_id_pkey PRIMARY KEY,
    email VARCHAR(255) NOT NULL CONSTRAINT users_email_key UNIQUE,
    password VARCHAR(255) NOT NULL,
    created_on TIMESTAMP NOT NULL,
    created_by VARCHAR(255) NOT NULL,
    last_modified_on TIMESTAMP NULL,
    last_modified_by VARCHAR(255) NULL
);