CREATE DATABASE "library"
    WITH
    OWNER = "postgres"
    TEMPLATE = template0
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C'
    TABLESPACE = 'pg_default'
    CONNECTION LIMIT = -1;

ALTER DATABASE "library" SET timezone TO 'UTC';