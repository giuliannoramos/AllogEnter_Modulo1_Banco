-- Questão 1
CREATE TABLE cidade (
	id SERIAL PRIMARY KEY,
	nome VARCHAR(60),
	uf VARCHAR(2)
);

CREATE TABLE cliente (
	id SERIAL PRIMARY KEY,
	id_cidade INT,
	nome VARCHAR(60),
	endereco VARCHAR(100),
	telefone VARCHAR(18),
	CONSTRAINT fk_cidade_cliente FOREIGN KEY (id_cidade) REFERENCES cidade(id)
);

-- Questão 2
INSERT INTO cidade (nome, uf) VALUES ('Brusque', 'SC');

INSERT INTO cidade (nome, uf) VALUES 
	('Itajaí', 'SC'),
	('Curitiba', 'PR'),
	('São Paulo', 'SP'),
	('Santos', 'SP');

SELECT * FROM cidade;

-- Questão 3
INSERT INTO cliente (id_cidade, nome, endereco, telefone) VALUES
(1, 'Teste 1', 'Rua Teste 1', '(11) 1111-1111'),
(2, 'Teste 2', 'Rua Teste 2', '(22) 2222-2222'),
(2, 'Teste 3', 'Rua Teste 3', '(33) 3333-3333'),
(2, 'Teste 4', 'Rua Teste 4', '(44) 4444-4444'),
(4, 'Teste 5', 'Rua Teste 5', '(55) 5555-5555'),
(3, 'Teste 6', 'Rua Teste 6', '(66) 6666-6666'),
(3, 'Teste 7', 'Rua Teste 7', '(77) 7777-7777');

SELECT * FROM cliente;

-- Questão 4
-- Atualizar o nome da cidade de “Brusque” para “Bruxque”;
UPDATE cidade SET nome = 'Bruxque' WHERE id = 1;

SELECT * FROM cidade;

-- Atualizar o nome das cidades da UF ‘SC’ colocando o prefixo “SC-”:
UPDATE cidade SET nome = 'SC-' || nome WHERE uf = 'SC';

SELECT * FROM cidade;

-- Questão 5
-- Excluir a cidade de “Santos”;
DELETE FROM cidade WHERE id = 5;

SELECT * FROM cidade;
-- Excluir o cliente de id = 2.
DELETE FROM cliente WHERE id = 2;

SELECT * FROM cliente;

-- Excluir as cidades com UF igual a SC e SP.
DELETE FROM cidade WHERE uf = 'SC' OR uf = 'SP'; 
-- Erro de violação da FK impossibilitando a exclusão de cidades, pois existem filhos relacionados (cliente)

-- Questão 6
-- Mostre o nome das cidades em ordem alfabética.
SELECT nome FROM cidade ORDER BY nome;

-- Mostre o Nome do Cliente e Nome da Cidade dos clientes cadastrados do estado ‘SC’.
SELECT cl.nome AS cliente, ci.nome AS cidade FROM cliente cl
INNER JOIN cidade ci ON cl.id_cidade = ci.id
WHERE uf = 'SC';

-- Mostre apenas os clientes que começam com a letra B.
-- Atualizado os nomes para listar resultados
UPDATE cliente SET nome = 'Beatriz' WHERE id = 4;
UPDATE cliente SET nome = 'Bruno' WHERE id = 6;

SELECT * FROM cliente WHERE nome ILIKE 'b%'; -- ILIKE maisculo minusculo

-- Mostre a quantidade de clientes que existem por cidade. Colocar o nome da cidade e a quantidade.
SELECT ci.nome AS cidade, COUNT(cl.id) AS quantidade FROM cliente cl
INNER JOIN cidade ci ON cl.id_cidade = ci.id
GROUP BY ci.id;

-- Mostre a quantidade de clientes da UF = ‘SC’.
SELECT COUNT(cl.id) AS quantidade FROM cliente cl
INNER JOIN cidade ci ON cl.id_cidade = ci.id
WHERE ci.uf = 'SC';
-- OU
SELECT COUNT(id) FROM cliente
WHERE id_cidade IN (SELECT id FROM cidade WHERE uf = 'SC');

-- Mostre o nome dos clientes concatenando com o nome da cidade e UF (exemplo: Lucas-Brusque-SC).
SELECT cl.nome || '-' || ci.nome || '-' || ci.uf AS concat FROM cliente cl
INNER JOIN cidade ci ON cl.id_cidade = ci.id;
-- Mostre o nome dos clientes de Santa Catarina e do Paraná, em dois comandos separadamente, e junte-os utilizando o comando UNION.
SELECT cl.nome FROM cliente cl
INNER JOIN cidade ci ON cl.id_cidade = ci.id
WHERE uf = 'SC' 
UNION 
SELECT cl.nome FROM cliente cl
INNER JOIN cidade ci ON cl.id_cidade = ci.id
WHERE uf = 'PR';