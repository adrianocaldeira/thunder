# Notificações

`Notify` (`Thunder`) é um objeto simples para representar o resultado de uma operação —
uma ou mais mensagens associadas a um `NotifyType` — útil para devolver o resultado de um
comando de serviço para a camada de apresentação.

## NotifyType

```csharp
using Thunder;

// Success, Information, Warning, Danger
```

## Criando notificações

O construtor sem parâmetros assume `NotifyType.Success` e uma lista de mensagens vazia; os
demais construtores aceitam uma mensagem única, uma lista de mensagens, ou um `NotifyType`
explícito:

```csharp
using Thunder;

// Sucesso (Type assume NotifyType.Success)
var sucesso = new Notify("Registro salvo com sucesso.");

sucesso.Type;            // NotifyType.Success
sucesso.Messages[0];      // "Registro salvo com sucesso."

// Erro, com mais de uma mensagem
var erro = new Notify(NotifyType.Danger, new List<string>
{
    "Campo obrigatório.",
    "CPF inválido."
});

erro.Type;            // NotifyType.Danger
erro.Messages.Count;   // 2
```

## Enumerando o tipo

Como `NotifyType` é um enum comum, ele pode ser percorrido com `Enum.GetValues` — por
exemplo, para popular um `<select>` de filtro por tipo de notificação:

```csharp
foreach (NotifyType tipo in Enum.GetValues(typeof(NotifyType)))
{
    Console.WriteLine(tipo);
}
// Success
// Information
// Warning
// Danger
```
