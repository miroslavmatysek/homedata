CREATE DATABASE homedata;


\c homedata;


CREATE EXTENSION IF NOT EXISTS timescaledb;


CREATE TABLE environment_worker_place
(
    timestamp        TIMESTAMPTZ      NOT NULL,
    work_place_temp  DOUBLE PRECISION NOT NULL,
    work_place_press DOUBLE PRECISION NOT NULL,
    work_place_hum   DOUBLE PRECISION NOT NULL
);

alter table environment_worker_place
    owner to postgres;

SELECT create_hypertable('environment_worker_place', 'timestamp');


CREATE INDEX ON environment_worker_place (timestamp DESC);