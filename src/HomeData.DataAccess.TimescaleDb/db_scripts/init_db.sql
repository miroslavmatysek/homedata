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

CREATE TABLE environment_weather_station
(
    timestamp        TIMESTAMPTZ      NOT NULL,
    indoor_temp  DOUBLE PRECISION NOT NULL,
    indoor_hum DOUBLE PRECISION NOT NULL,
    outdoor_temp DOUBLE PRECISION NOT NULL,
    outdoor_hum DOUBLE PRECISION NOT NULL
);

alter table environment_worker_place
    owner to postgres;

alter table environment_weather_station
    owner to postgres;

SELECT create_hypertable('environment_worker_place', 'timestamp');

SELECT create_hypertable('environment_weather_station', 'timestamp');


CREATE INDEX ON environment_worker_place (timestamp DESC);

CREATE INDEX ON environment_weather_station (timestamp DESC);