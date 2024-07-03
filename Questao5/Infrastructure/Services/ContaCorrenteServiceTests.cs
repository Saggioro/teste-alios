using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.DTOs;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Services;
using Xunit;

public class ContaCorrenteServiceTests
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IContaCorrenteService _contaCorrenteService;

    public ContaCorrenteServiceTests()
    {
        _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        _contaCorrenteService = new ContaCorrenteService(_contaCorrenteRepository);
    }

    [Fact]
    public async Task MovimentarAsync_ShouldThrowValidationException_WhenContaCorrenteIsInvalid()
    {
        var command = new MovimentarContaCorrenteCommand
        {
            Id = "invalid-id",
            Valor = 100,
            TipoMovimento = TipoMovimento.Credito
        };

        _contaCorrenteRepository.GetByIdAsync(command.Id).Returns(Task.FromResult<ContaCorrente>(null));

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _contaCorrenteService.MovimentarAsync(command));
        Assert.Equal(ValidationErrorType.InvalidAccount, exception.ErrorType);
    }

    [Fact]
    public async Task ConsultarSaldoAsync_ShouldReturnSaldo_WhenContaCorrenteIsValid()
    {
        var conta = new ContaCorrente { Id = "valid-id", Numero = 123, Nome = "Teste", Ativo = true };
        _contaCorrenteRepository.GetByIdAsync(conta.Id).Returns(Task.FromResult(conta));
        _contaCorrenteRepository.GetMovimentosByContaIdAsync(conta.Id).Returns(Task.FromResult((IEnumerable<Movimento>)new List<Movimento>
        {
            new Movimento { Id = "1", IdContaCorrente = conta.Id, DataMovimento = DateTime.Now, TipoMovimento = TipoMovimento.Credito, Valor = 100 },
            new Movimento { Id = "2", IdContaCorrente = conta.Id, DataMovimento = DateTime.Now, TipoMovimento = TipoMovimento.Debito, Valor = 50 }
        }));

        var saldoDto = await _contaCorrenteService.ConsultarSaldoAsync(conta.Id);

        Assert.NotNull(saldoDto);
        Assert.Equal(123, saldoDto.Numero);
        Assert.Equal("Teste", saldoDto.Nome);
        Assert.Equal(50, saldoDto.Saldo);
    }
}