CREATE TABLE operaciones (
    id SERIAL PRIMARY KEY,
    name TEXT UNIQUE NOT NULL
);

CREATE TABLE grupo_material (
    id SERIAL PRIMARY KEY,
    name TEXT UNIQUE NOT NULL
);

CREATE TABLE grupo_operaciones (
    id_grupo INT REFERENCES grupo_material(id),
    id_operacion INT REFERENCES operaciones(id),
    PRIMARY KEY (id_grupo, id_operacion)
);

CREATE TABLE piezas (
    id_pieza TEXT PRIMARY KEY,
    item_number TEXT,
    item_type TEXT,
    item_version TEXT,
    item_description TEXT,
    fecha TIMESTAMP,
    operacion TEXT,
    status TEXT
);

CREATE TABLE last_requested (
    id_grupo INT REFERENCES grupo_material(id),
    rango TEXT,
    last_time TIMESTAMP,
    PRIMARY KEY (id_grupo, rango)
);

CREATE INDEX idx_piezas_status ON piezas(status);
CREATE INDEX idx_piezas_fecha ON piezas(fecha);
