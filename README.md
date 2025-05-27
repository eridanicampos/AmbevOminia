Projeto AmbevTech

Visão Geral

Este projeto é uma API web desenvolvida para a AmbevTech, utilizando um stack tecnológico robusto e moderno, visando eficiência, escalabilidade e facilidade de manutenção. A solução foi projetada com princípios de Domain-Driven Design (DDD) e segue boas práticas como SOLID, MediatR para o padrão CQRS, e o uso de arquitetura dirigida a eventos através de um modelo publisher-subscriber.

Tecnologias Utilizadas

.NET 8: O projeto foi construído em .NET 8, proporcionando as funcionalidades mais recentes e melhorias do ecossistema .NET.

Entity Framework Core: Abordagem Code First para gerenciamento do banco de dados.

AutoMapper: Utilizado para mapeamento entre objetos de transferência de dados e entidades de domínio.

FluentValidation: Para validação das requisições recebidas.

Bogus: Para gerar dados de teste.

XUnit: Framework de testes unitários utilizado para desenvolvimento orientado a testes.

FluentAssertions: Utilizado para escrever asserções de teste legíveis e de fácil manutenção.

NSubstitute: Biblioteca de mocking usada para criar substituições de interfaces durante os testes.

Serilog: Biblioteca de logging utilizada para rastrear as atividades da aplicação.

Docker: O projeto pode ser containerizado para implantação usando Docker.

Swagger: Integrado para documentação e testes da API.

Arquitetura e Boas Práticas

Domain-Driven Design (DDD): A solução é construída seguindo os princípios de DDD, o que ajuda a criar um modelo de domínio rico que representa os cenários reais do negócio.

Princípios SOLID: O projeto segue estritamente os princípios SOLID para garantir que o código seja manutenível, escalável e fácil de estender.

CQRS com MediatR: O padrão Command Query Responsibility Segregation (CQRS) é implementado usando MediatR, o que ajuda a desacoplar comandos e consultas, tornando a arquitetura mais flexível e de fácil manutenção.

Configuração de CORS: Cross-Origin Resource Sharing (CORS) está configurado para permitir requisições de diferentes domínios, aumentando a usabilidade da API em múltiplos ambientes.

Arquitetura Dirigida a Eventos: Utilizamos um publisher de eventos para desacoplar diferentes partes do sistema, permitindo melhor escalabilidade e separação de responsabilidades.

Projeto Desacoplado: O projeto foi desenvolvido de forma a garantir o desacoplamento das funcionalidades, facilitando a manutenção e evolução do sistema.

Testes

Testes Unitários: O projeto inclui testes unitários extensivos usando XUnit. FluentAssertions é utilizado para melhorar a legibilidade das asserções dos testes, e NSubstitute é utilizado para mockar dependências durante os testes.

Bogus: Bogus é utilizado para gerar dados fictícios durante os testes, tornando-os mais robustos e realistas.

Como Executar o Projeto

Clone o repositório do GitHub:

git clone https://github.com/eridanicampos/AmbevOminia.git

Certifique-se de ter a versão mais recente do SDK .NET 8 instalada.

Execute docker-compose up para iniciar a aplicação em um container Docker.

Acesse o Swagger em http://localhost:5000/swagger para interagir com a API.
