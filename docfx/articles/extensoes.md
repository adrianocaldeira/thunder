# Extensões de tipos

O pacote **Thunder** adiciona métodos de extensão de uso cotidiano para `string`, `DateTime`
e `object`, em `Thunder.Extensions`.

## Texto

```csharp
using Thunder.Extensions;

"Desenvolvimento de software".Truncate(12); // "Desenvolvime"

"Lorem ipsum dolor sit amet".Reduce(20, "..."); // "Lorem ipsum dolor..." (20 caracteres no total)

"Café com Açúcar e Pão".RemoveAccent(); // "Cafe com Acucar e Pao"

"Café & Livraria São Paulo".ToSeo(); // "cafe-livraria-sao-paulo"

"Linha 1\nLinha 2".NlToBr(); // "Linha 1<br />Linha 2"

"   ".IsEmpty(); // true (também true para null)
"ok".IsEmpty();  // false
```

`With` encapsula `string.Format`, e `Join` concatena um array com separador:

```csharp
"Olá, {0}! Você tem {1} anos.".With("Maria", 30); // "Olá, Maria! Você tem 30 anos."

new object[] { "a", "b", "c" }.Join(", "); // "a, b, c"
```

## Datas

```csharp
using Thunder.Extensions;

var data = new DateTime(2026, 7, 15);

data.FirstDayOfMonth(); // 2026-07-01
data.LastDayOfMonth();  // 2026-07-31
data.AddWeeks(2);       // 2026-07-29
data.IsWeekend();       // false (é uma quarta-feira)
data.DaysBetween(new DateTime(2026, 8, 1)); // 17
```

Também há `FirstDayOfYear`/`LastDayOfYear`, `PreviousDay`/`NextDay` (com sobrecargas por
`DayOfWeek`), `YearsBetween`/`MonthsBetween`/`WeeksBetween` e `Quarter`, entre outros.

## Objetos

```csharp
using Thunder.Extensions;

"42".Cast<int>();  // 42
"".Cast<int>();     // 0 (string vazia cai no guard que retorna default(T) antes de tentar converter)
```

`Trim<T>` percorre as propriedades públicas do tipo `T` e aplica `string.Trim()` em todas
as que forem `string`, útil para higienizar um objeto recebido de um formulário antes de
persistir:

```csharp
var pedido = new Pedido { Observacao = "  entregar até sexta  " };
pedido.Trim();
// pedido.Observacao == "entregar até sexta"
```
