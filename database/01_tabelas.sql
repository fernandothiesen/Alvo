CREATE TABLE usuario (
    id_usuario SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    senha_hash TEXT NOT NULL,
    ativo BOOLEAN DEFAULT TRUE,
    data_criacao TIMESTAMP DEFAULT NOW(),
    ultimo_login TIMESTAMP
);

CREATE TABLE role (
    id_role SERIAL PRIMARY KEY,
    nome_role TEXT NOT NULL,
    descricao TEXT
);

CREATE TABLE permissao (
    id_permissao SERIAL PRIMARY KEY,
    nome_permissao TEXT NOT NULL,
    descricao TEXT
);
CREATE TABLE pais (
    id_pais SERIAL PRIMARY KEY,
    nome_pais TEXT NOT NULL,
    codigo_iso TEXT NOT NULL
);

CREATE TABLE estado (
    id_estado SERIAL PRIMARY KEY,
    id_pais INTEGER NOT NULL REFERENCES pais(id_pais),
    nome_estado TEXT NOT NULL,
    sigla_estado TEXT NOT NULL
);

CREATE TABLE cidade (
    id_cidade SERIAL PRIMARY KEY,
    id_estado INTEGER NOT NULL REFERENCES estado(id_estado),
    nome_cidade TEXT NOT NULL
);
CREATE TABLE cliente (
    id_cliente SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    ativo BOOLEAN DEFAULT TRUE,
    data_cadastro TIMESTAMP DEFAULT NOW(),
    id_cidade INTEGER REFERENCES cidade(id_cidade)
);

CREATE TABLE fornecedor (
    id_fornecedor SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    ativo BOOLEAN DEFAULT TRUE,
    data_cadastro TIMESTAMP DEFAULT NOW(),
    id_cidade INTEGER REFERENCES cidade(id_cidade)
);
CREATE TABLE servico (
    id_servico SERIAL PRIMARY KEY,
    nome_servico TEXT NOT NULL,
    descricao TEXT
);
CREATE TABLE evento (
    id_evento SERIAL PRIMARY KEY,
    codigo_evento TEXT UNIQUE NOT NULL,
    titulo TEXT NOT NULL,
    descricao TEXT,
    data_inicio DATE,
    data_fim DATE,
    local TEXT,
    mes_referencia TEXT,
    limite_entrega_material DATE,
    status TEXT,
    observacoes TEXT,
    id_cidade INTEGER REFERENCES cidade(id_cidade),
    id_responsavel INTEGER REFERENCES usuario(id_usuario)
);

CREATE TABLE demanda (
    id_demanda SERIAL PRIMARY KEY,
    id_evento INTEGER NOT NULL REFERENCES evento(id_evento),
    titulo TEXT NOT NULL,
    descricao TEXT,
    prioridade TEXT,
    status TEXT,
    data_criacao TIMESTAMP DEFAULT NOW(),
    data_conclusao TIMESTAMP
);
CREATE TABLE documento (
    id_documento SERIAL PRIMARY KEY,
    tipo_documento TEXT NOT NULL,
    valor_documento TEXT NOT NULL,
    ativo BOOLEAN DEFAULT TRUE,
    data_validacao DATE
);

CREATE TABLE contato (
    id_contato SERIAL PRIMARY KEY,
    tipo_contato TEXT NOT NULL,
    valor_contato TEXT NOT NULL,
    observacao TEXT,
    ativo BOOLEAN DEFAULT TRUE
);
CREATE TABLE forma_pagamento (
    id_forma_pagamento SERIAL PRIMARY KEY,
    tipo_forma_pagamento TEXT NOT NULL,
    descricao TEXT
);

CREATE TABLE conta_bancaria (
    id_conta SERIAL PRIMARY KEY,
    banco TEXT NOT NULL,
    agencia TEXT NOT NULL,
    numero_conta TEXT NOT NULL,
    tipo_conta TEXT NOT NULL,
    titular TEXT NOT NULL,
    ativo BOOLEAN DEFAULT TRUE
);

CREATE TABLE financeiro (
    id_financeiro SERIAL PRIMARY KEY,
    tipo TEXT NOT NULL,
    valor NUMERIC(12,2) NOT NULL,
    data_movimentacao DATE NOT NULL,
    descricao TEXT,

    id_evento INTEGER REFERENCES evento(id_evento),
    id_demanda INTEGER REFERENCES demanda(id_demanda),
    id_fornecedor INTEGER REFERENCES fornecedor(id_fornecedor),
    id_cliente INTEGER REFERENCES cliente(id_cliente),

    id_forma_pagamento INTEGER NOT NULL REFERENCES forma_pagamento(id_forma_pagamento),
    id_conta INTEGER REFERENCES conta_bancaria(id_conta)
);
CREATE TABLE atividade_sistema (
    id_atividade SERIAL PRIMARY KEY,
    id_usuario INTEGER NOT NULL REFERENCES usuario(id_usuario),
    tipo_atividade TEXT NOT NULL,
    descricao TEXT,
    data_hora TIMESTAMP DEFAULT NOW()
);
CREATE TABLE usuario_role (
    id_usuario INTEGER REFERENCES usuario(id_usuario),
    id_role INTEGER REFERENCES role(id_role),
    PRIMARY KEY (id_usuario, id_role)
);

CREATE TABLE role_permissao (
    id_role INTEGER REFERENCES role(id_role),
    id_permissao INTEGER REFERENCES permissao(id_permissao),
    PRIMARY KEY (id_role, id_permissao)
);

CREATE TABLE usuario_permissao (
    id_usuario INTEGER REFERENCES usuario(id_usuario),
    id_permissao INTEGER REFERENCES permissao(id_permissao),
    PRIMARY KEY (id_usuario, id_permissao)
);

CREATE TABLE evento_cliente (
    id_evento INTEGER REFERENCES evento(id_evento),
    id_cliente INTEGER REFERENCES cliente(id_cliente),
    PRIMARY KEY (id_evento, id_cliente)
);

CREATE TABLE evento_fornecedor (
    id_evento INTEGER REFERENCES evento(id_evento),
    id_fornecedor INTEGER REFERENCES fornecedor(id_fornecedor),
    PRIMARY KEY (id_evento, id_fornecedor)
);

CREATE TABLE demanda_cliente (
    id_demanda INTEGER REFERENCES demanda(id_demanda),
    id_cliente INTEGER REFERENCES cliente(id_cliente),
    PRIMARY KEY (id_demanda, id_cliente)
);

CREATE TABLE demanda_fornecedor (
    id_demanda INTEGER REFERENCES demanda(id_demanda),
    id_fornecedor INTEGER REFERENCES fornecedor(id_fornecedor),
    PRIMARY KEY (id_demanda, id_fornecedor)
);

CREATE TABLE cliente_contato (
    id_cliente INTEGER REFERENCES cliente(id_cliente),
    id_contato INTEGER REFERENCES contato(id_contato),
    PRIMARY KEY (id_cliente, id_contato)
);

CREATE TABLE fornecedor_contato (
    id_fornecedor INTEGER REFERENCES fornecedor(id_fornecedor),
    id_contato INTEGER REFERENCES contato(id_contato),
    PRIMARY KEY (id_fornecedor, id_contato)
);

CREATE TABLE usuario_contato (
    id_usuario INTEGER REFERENCES usuario(id_usuario),
    id_contato INTEGER REFERENCES contato(id_contato),
    PRIMARY KEY (id_usuario, id_contato)
);

CREATE TABLE cliente_documento (
    id_cliente INTEGER REFERENCES cliente(id_cliente),
    id_documento INTEGER REFERENCES documento(id_documento),
    PRIMARY KEY (id_cliente, id_documento)
);

CREATE TABLE fornecedor_documento (
    id_fornecedor INTEGER REFERENCES fornecedor(id_fornecedor),
    id_documento INTEGER REFERENCES documento(id_documento),
    PRIMARY KEY (id_fornecedor, id_documento)
);

CREATE TABLE fornecedor_conta_bancaria (
    id_fornecedor INTEGER REFERENCES fornecedor(id_fornecedor),
    id_conta INTEGER REFERENCES conta_bancaria(id_conta),
    PRIMARY KEY (id_fornecedor, id_conta)
);

CREATE TABLE fornecedor_servico (
    id_fornecedor INTEGER REFERENCES fornecedor(id_fornecedor),
    id_servico INTEGER REFERENCES servico(id_servico),
    PRIMARY KEY (id_fornecedor, id_servico)
);
