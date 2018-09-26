using Microsoft.AspNetCore.Mvc;
using EnterprisePatterns.Api.BankAccounts.Application.Dto;
using Microsoft.AspNetCore.Http;
using EnterprisePatterns.Api.BankAccounts.Application.Assembler;
using EnterprisePatterns.Api.BankAccounts.Domain.Entity;
using EnterprisePatterns.Api.BankAccounts.Domain.Repository;
using EnterprisePatterns.Api.Common.Application.Dto;

namespace EnterprisePatterns.Api.Controllers
{
    [Route("v1/customers/{customerId}/bank-accounts")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly BankAccountCreateAssembler _bankAccountCreateAssembler;

        public BankAccountsController(IBankAccountRepository bankAccountRepository, BankAccountCreateAssembler bankAccountCreateAssembler)
        {
            _bankAccountRepository = bankAccountRepository;
            _bankAccountCreateAssembler = bankAccountCreateAssembler;
        }

        [HttpPost]
        public IActionResult Create(long customerId, [FromBody] BankAccountCreateDto bankAccountCreateDto)
        {
            bankAccountCreateDto.CustomerId = customerId;
            BankAccount bankAccount = _bankAccountCreateAssembler.toEntity(bankAccountCreateDto);
            _bankAccountRepository.Create(bankAccount);
            return StatusCode(StatusCodes.Status201Created, new ApiStringResponseDto("BankAccount Created!"));
        }
    }
}
