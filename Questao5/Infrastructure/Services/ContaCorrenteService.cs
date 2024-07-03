using Questao5.Application.Commands.Requests;
using Questao5.Application.DTOs;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Services
{
    public class ContaCorrenteService : IContaCorrenteService
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public ContaCorrenteService(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<string> MovimentarAsync(MovimentarContaCorrenteCommand command)
        {
            var conta = await _contaCorrenteRepository.GetByIdAsync(command.Id);
            if (conta == null)
                throw new ValidationException(ValidationErrorType.InvalidAccount);

            if (!conta.Ativo)
                throw new ValidationException(ValidationErrorType.InactiveAccount);

            if (command.Valor <= 0)
                throw new ValidationException(ValidationErrorType.InvalidValue);

            if (command.TipoMovimento != TipoMovimento.Credito && command.TipoMovimento != TipoMovimento.Debito)
                throw new ValidationException(ValidationErrorType.InvalidType);

            var movimento = new Movimento
            {
                Id = Guid.NewGuid().ToString(),
                IdContaCorrente = command.Id,
                DataMovimento = DateTime.Now,
                TipoMovimento = command.TipoMovimento,
                Valor = command.Valor
            };

            await _contaCorrenteRepository.AddMovimentoAsync(movimento);
            return movimento.Id;
        }

        public async Task<SaldoDto> ConsultarSaldoAsync(string idContaCorrente)
        {
            var conta = await _contaCorrenteRepository.GetByIdAsync(idContaCorrente);
            if (conta == null)
                throw new ValidationException(ValidationErrorType.InvalidAccount);

            if (!conta.Ativo)
                throw new ValidationException(ValidationErrorType.InactiveAccount);

            var movimentos = await _contaCorrenteRepository.GetMovimentosByContaIdAsync(idContaCorrente);
            var saldo = movimentos
                .Where(m => m.TipoMovimento == TipoMovimento.Credito).Sum(m => m.Valor) -
                movimentos.Where(m => m.TipoMovimento == TipoMovimento.Debito).Sum(m => m.Valor);

            return new SaldoDto
            {
                Numero = conta.Numero,
                Nome = conta.Nome,
                DataConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }
}