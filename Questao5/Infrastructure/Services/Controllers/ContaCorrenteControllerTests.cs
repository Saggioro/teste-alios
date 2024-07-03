using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.DTOs;
using Questao5.Controllers;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Services;
using Xunit;

public class ContaCorrenteControllerTests
{
    private readonly IMediator _mediator;
    private readonly ContaCorrenteController _controller;
    private readonly ILogger<ContaCorrenteController> _logger;
    public ContaCorrenteControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<ContaCorrenteController>>();
        _controller = new ContaCorrenteController(_mediator, _logger);
    }

    [Fact]
    public async Task Movimentar_ShouldReturnBadRequest_WhenValidationExceptionIsThrown()
    {
        // Arrange
        var command = new MovimentarContaCorrenteCommand
        {
            Id = "invalid-id",
            Valor = 100,
            TipoMovimento = TipoMovimento.Credito
        };

        _mediator.Send(command).Returns(Task.FromException<string>(new ValidationException(ValidationErrorType.InvalidAccount)));

        // Act
        var result = await _controller.Movimentar(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("InvalidAccount", ((dynamic)badRequestResult.Value).Mensagem);
    }

    [Fact]
    public async Task ConsultarSaldo_ShouldReturnOk_WhenContaCorrenteIsValid()
    {
        // Arrange
        var query = new ConsultarSaldoQuery { IdContaCorrente = "valid-id" };
        var saldoDto = new SaldoDto
        {
            Numero = 123,
            Nome = "Teste",
            DataConsulta = DateTime.Now,
            Saldo = 100
        };

        _mediator.Send(query).Returns(Task.FromResult(saldoDto));

        // Act
        var result = await _controller.ConsultarSaldo("valid-id");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var returnedSaldo = Assert.IsType<SaldoDto>(okResult.Value);
        Assert.Equal(saldoDto.Numero, returnedSaldo.Numero);
        Assert.Equal(saldoDto.Nome, returnedSaldo.Nome);
        Assert.Equal(saldoDto.DataConsulta, returnedSaldo.DataConsulta);
        Assert.Equal(saldoDto.Saldo, returnedSaldo.Saldo);
    }
}
