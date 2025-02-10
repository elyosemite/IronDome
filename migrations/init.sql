-- Create Database
CREATE DATABASE pki_db;

\c pki_db;

-- Create Schemas
CREATE SCHEMA IF NOT EXISTS certificates;
CREATE SCHEMA IF NOT EXISTS encryption;
CREATE SCHEMA IF NOT EXISTS signatures;
CREATE SCHEMA IF NOT EXISTS observability;

-- Create Roles with Least Privilege Access
CREATE ROLE db_admin WITH LOGIN PASSWORD 'admin_secure_password' SUPERUSER;
CREATE ROLE app_rw WITH LOGIN PASSWORD 'rw_secure_password' NOSUPERUSER CREATEDB;
CREATE ROLE app_ro WITH LOGIN PASSWORD 'ro_secure_password' NOSUPERUSER;

-- Grant Permissions
GRANT CONNECT ON DATABASE pki_db TO app_rw, app_ro;
GRANT USAGE ON SCHEMA certificates, encryption, signatures, observability TO app_rw, app_ro;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA certificates, encryption, signatures TO app_rw;
GRANT SELECT ON ALL TABLES IN SCHEMA certificates, encryption, signatures, observability TO app_ro;

-- Ensure Future Tables Maintain Permissions
ALTER DEFAULT PRIVILEGES IN SCHEMA certificates, encryption, signatures
    GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO app_rw;
ALTER DEFAULT PRIVILEGES IN SCHEMA certificates, encryption, signatures, observability
    GRANT SELECT ON TABLES TO app_ro;
