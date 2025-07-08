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
    }  
}
