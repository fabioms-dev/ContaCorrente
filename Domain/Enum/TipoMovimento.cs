using System.ComponentModel;

namespace ContaCorrente.Domain.Enum
{
    /// <summary>
    /// Tipo de movimento enum
    /// </summary>
    public enum TipoMovimento
    {
        [Description("Credito")]
        Credito = 'C',

        [Description("Debito")]
        Debito = 'D',
    }
}
