# Sistema de Gestão de Eventos

Este repositório contém o início do desenvolvimento de um sistema de gestão de eventos, incluindo cadastro de clientes, fornecedores, eventos, demandas e controle financeiro.  
O objetivo é criar uma plataforma organizada, escalável e segura para auxiliar na administração de eventos corporativos.

---

##  Status do Projeto

- Banco de dados criado no Supabase (ambiente de testes)
- Modelagem lógica definida e implementada
- Repositório GitHub configurado
- Próximo passo: iniciar desenvolvimento da aplicação

---

##  Tecnologias Planejadas

- **Supabase (PostgreSQL)** — banco de dados, autenticação e storage  
- **.NET / c#** — backend 
- **Html/CSS/JS** — frontend (a definir)  
- **GitHub** — versionamento e documentação  
- **Futuro:** CI/CD com GitHub Actions

---

##  Banco de Dados

O banco foi modelado seguindo boas práticas de normalização (3FN/BCNF).  
Principais entidades:

- `usuario`
- `cliente`
- `fornecedor`
- `evento`
- `demanda`
- `financeiro`
- `documento`
- `contato`
- tabelas associativas N:N

O DDL completo está disponível em:  
`database/01_tabelas.sql` (crie este arquivo e coloque o script lá)

---

##  Objetivo Geral

Criar uma plataforma completa para:

- Gerenciar eventos
- Controlar clientes e fornecedores
- Registrar demandas internas
- Administrar movimentações financeiras
- Gerar relatórios e indicadores

---

##  Roadmap Inicial

### Fase 1 — Estrutura
- Criar banco no Supabase ✔
- Criar repositório GitHub ✔
- Definir stack inicial

### Fase 2 — Desenvolvimento
- Implementar autenticação
- Criar CRUD de cliente
- Criar CRUD de fornecedor
- Criar CRUD de evento

### Fase 3 — Funcionalidades Avançadas
- Demandas
- Financeiro
- Relatórios

### Fase 4 — Segurança
- Roles e permissões
- RLS no Supabase

### Fase 5 — Deploy
- CI/CD
- Ambiente de produção

---

