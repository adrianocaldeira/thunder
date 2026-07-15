# Validações e formatações brasileiras

O pacote **Thunder** traz validação e formatação dos documentos e contatos brasileiros mais
comuns como métodos de extensão de `string` (`Thunder.Extensions`).

## CPF

```csharp
using Thunder;
using Thunder.Extensions;

"529.982.247-25".IsCpf();            // true (com máscara)
"52998224725".IsCpf();               // true (sem máscara)
"111.111.111-11".IsCpf();            // false (dígitos repetidos)

"52998224725".Format(FormatType.Cpf); // "529.982.247-25"
```

## CNPJ (numérico e alfanumérico)

A partir de julho/2026 a Receita Federal emite CNPJ **alfanumérico**. O Thunder valida e
formata os dois formatos com o mesmo método — o cálculo dos dígitos verificadores usa
módulo 11 sobre os valores ASCII, e o CNPJ numérico continua funcionando como antes.

```csharp
// Numérico
"11.222.333/0001-81".IsCnpj();          // true
"11222333000181".Format(FormatType.Cnpj); // "11.222.333/0001-81"

// Alfanumérico (novo formato)
"12ABC345000188".IsCnpj();               // true
"12abc345000188".IsCnpj();               // true (minúsculas são normalizadas)
"12ABC345000188".Format(FormatType.Cnpj); // "12.ABC.345/0001-88"
```

Via DataAnnotations:

```csharp
using Thunder.ComponentModel.DataAnnotations;

public class Empresa
{
    [Document(Type = DocumentType.Cnpj)]
    public string Cnpj { get; set; }
}
```

## E-mail, CEP e telefone

```csharp
"contato@exemplo.com.br".IsEmail();   // true
"01001-000".IsZipCode();              // true
"01001000".Format(FormatType.ZipCode);// "01001-000"
"(11) 91234-5678".IsPhone();          // true
"11912345678".Format(FormatType.Phone); // "(11) 91234-5678"
```
