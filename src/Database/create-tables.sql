\c library;

CREATE SCHEMA library AUTHORIZATION postgres;

-- Authors table migration
CREATE TABLE library.authors 
(
    id SERIAL PRIMARY KEY,
    first_name character varying COLLATE pg_catalog."default" NOT NULL,
    last_name character varying COLLATE pg_catalog."default" NOT NULL,
    country character varying COLLATE pg_catalog."default" NOT NULL,
    books int NOT NULL
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE library.authors 
    OWNER to postgres;


-- Books table migration
CREATE TABLE library.books 
(
    id SERIAL PRIMARY KEY,
    date date NOT NULL,
    author_id bigint NOT NULL,
    title character varying COLLATE pg_catalog."default" NOT NULL,
    category character varying COLLATE pg_catalog."default" NOT NULL,
    pages int NOT NULL,
    CONSTRAINT fk_author_id FOREIGN KEY (author_id) REFERENCES library.authors (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE library.books 
    OWNER to postgres;