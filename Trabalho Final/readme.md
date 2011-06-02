# PROMPT11-01-VMEssentials.vilhena - Projecto Final #

# Autor #

Gonçalo Vilhena

## Arquitectura ##

### Componentes ###

 * HttpReflector
  * Console Application responsável pelo carregamento dos vários componentes

 * Controler
  * Entidade responsável por comunicar e interligar os vários componentes.
  * Implementação ReflectorController da interface IController
  * Constituido por um IRouter e um IUIBinder
  * Recebe pedidos do UIBinder através de uma Callback registada, encaminha o pedido para o Router e devolve a resposta ao UIBinder

 * UIBinder

 * Router
  * Entidade responsável por armazenar pares de Rotas e Tipos
  * Implementação ReflectorRouter da interface IRouter
  * Constituido por uma IRouteContainer, colecção de armazenamento de rotas
  * Recebe um pedido encaminha a rota ao IRouterCollection e devolve um IHandler
  * Nesta implementalção o IRouteContainer é uma implementação com Dicionário (RouteList)
  * O RouteList é um container de pares Rotas -> Type, onde o type é o tipo do objecto que deve ser instanciado e devolvido ao controller
  * Recebe do RouteContainer um RouteResult, o qual é constituido por um Mapa <pattern, valor> encontrado pelo pattern e o Objecto guardado no RouteContainter, neste caso o tipo do Handler

 * RouteContainer
  * Container Genérico de armazenamento de rotas
  * Implementação da RouteList a interface IRouteContainer
  * Regista rotas com patterns com um Type associado, neste caso o tipo do IHandler a ser executado
  * Retorna um RouteResult

 * Handlers
  * Entidades que contêm as acções necessárias executar sobre o AssemblyModel (responsável por retornar os tipos carregados)
  * Impementa a interface IHandler, que é constituida por um método Run que retorna uma IView
  * 

4 semanas, 10h / semana (total de 40h)
 
### Exemplo de um Pedido ###

Este módulo tem por objectivo dotar os participantes com a capacidade de compreensão e utilização de técnicas actuais de programação em C# 3.0, 3.5 e 4.0, conseguindo identificar problemas a que estas se adequam.

## Compromissos ##

 * Router implementado com uma implementação de IRouterCollection em Dicionário, existe uma outra implementação em Tree mas não está a funcionar correctamente, como o Router utiliza uma IRouterCollection é basta alterar uma linha de código para passar a utilizar uma implementação de IRouterCollection em Tree
 * 


 