--create table utilizadores(
--	id_utilizador int not null identity(1,1) primary key, 
--	nome varchar(250) not null,
--	data_nascimento date not null,
--	idade as datediff(year, data_nascimento, getdate()),
--	morada varchar(250) not null,
--	numero_predio int not null,
--	numero_andar int not null,
--	cc int not null check(cc >= 0)
--);

--Insert into utilizadores(nome, data_nascimento, morada, numero_andar, numero_predio, cc) 
--values ('André', '1998-04-15', 'Rua Feliz', 2, 3, 4);
USE utilizadores;
select * from utilizadores;