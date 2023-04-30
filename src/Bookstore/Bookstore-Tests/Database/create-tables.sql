CREATE SCHEMA IF NOT EXISTS bookstore AUTHORIZATION postgres;

-- Authors table migration
CREATE TABLE IF NOT EXISTS bookstore.authors
(
    id SERIAL PRIMARY KEY,
    first_name character varying COLLATE pg_catalog."default" NOT NULL,
    last_name character varying COLLATE pg_catalog."default" NOT NULL,
    country character varying COLLATE pg_catalog."default" NOT NULL
)
    WITH (
        OIDS = FALSE
    )
    TABLESPACE pg_default;

ALTER TABLE bookstore.authors
    OWNER to postgres;


-- Books table migration
CREATE TABLE IF NOT EXISTS bookstore.books
(
    id SERIAL PRIMARY KEY,
    date date NOT NULL,
    author_id bigint NOT NULL,
    title character varying COLLATE pg_catalog."default" NOT NULL,
    category character varying COLLATE pg_catalog."default" NOT NULL,
    pages int NOT NULL,
    CONSTRAINT fk_author_id FOREIGN KEY (author_id) REFERENCES bookstore.authors (id)
)
    WITH (
        OIDS = FALSE
    )
    TABLESPACE pg_default;

ALTER TABLE bookstore.books
    OWNER to postgres;
