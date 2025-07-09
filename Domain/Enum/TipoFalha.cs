using System.ComponentModel;

namespace ContaCorrente.Domain.Enum
{
    /// <summary>
    /// Enumerador do Tipo Falha
    /// </summary>
    public enum TipoFalha
    {
        [Description("CPF inválido")]
        Invalid_Document = 1,

        [Description("Usuário não autorizado")]
        User_Unauthorized = 2,

        [Description("Conta inválida")]
        Invalid_Account = 3,

        [Description("Conta inativa")]
        Inactive_Account = 4,

        [Description("Valor Invalido")]
        Invalid_Value = 5,

        [Description("Tipo Invalido")]
        Invalid_Type = 6,
    }  
}
