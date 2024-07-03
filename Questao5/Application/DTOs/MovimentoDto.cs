namespace Questao5.Application.DTOs
{
    public class MovimentoDto
    {
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; }
        public string ChaveIdempotencia { get; set; }
    }
}
