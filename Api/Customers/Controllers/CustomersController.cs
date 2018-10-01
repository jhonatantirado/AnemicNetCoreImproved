using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using EnterprisePatterns.Api.Common.Controllers;
using EnterprisePatterns.Api.Movies.Domain.Repository;
using EnterprisePatterns.Api.Customers.Domain.Repository;
using EnterprisePatterns.Api.Common.Application;
using EnterprisePatterns.Api.Common.Application.Dto;
using EnterprisePatterns.Api.Common.Domain.ValueObject;
using EnterprisePatterns.Api.Customers.Dto;
using EnterprisePatterns.Api.Movies.Dto;
using EnterprisePatterns.Api.Customers;
using EnterprisePatterns.Api.Movies.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EnterprisePatterns.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly IMovieRepository _movieRepository;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(IUnitOfWork unitOfWork, IMovieRepository movieRepository, ICustomerRepository customerRepository)
        {
             _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            Customer customer = _customerRepository.Read(id);
            if (customer == null)
                return NotFound();
            
            var dto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name.Value,
                Email = customer.Email.Value,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.Type.ToString(),
                StatusExpirationDate = customer.Status.ExpirationDate,
                PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
                {
                    Price = x.Price,
                    ExpirationDate = x.ExpirationDate,
                    PurchaseDate = x.PurchaseDate,
                    Movie = new MovieDto
                    {
                        Id = x.Movie.Id,
                        Name = x.Movie.Name
                    }
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet]
        public IActionResult GetList()
        {
            bool uowStatus = false;
            try{
                uowStatus = _unitOfWork.BeginTransaction();
                IReadOnlyList<Customer> customers = _customerRepository.GetList();
                _unitOfWork.Commit(uowStatus);
                List<CustomerInListDto> dtos = customers.Select(x => new CustomerInListDto
                {
                    Id = x.Id,
                    Name = x.Name.Value,
                    Email = x.Email.Value,
                    MoneySpent = x.MoneySpent,
                    Status = x.Status.Type.ToString(),
                    StatusExpirationDate = x.Status.ExpirationDate
                }).ToList();
                return Ok(dtos);
            } catch (Exception ex){
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiStringResponseDto("Internal Server Error"));
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto item)
        {
            Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
            Result<Email> emailOrError = Email.Create(item.Email);

            Result result = Result.Combine(customerNameOrError, emailOrError);
            if (result.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto(result.Error));

            if (_customerRepository.GetByEmail(emailOrError.Value) != null)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto("Email is already in use: " + item.Email));

            var customer = new Customer(customerNameOrError.Value, emailOrError.Value);
            _customerRepository.Create(customer);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
        {
            Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
            if (customerNameOrError.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto(customerNameOrError.Error));

            Customer customer = _customerRepository.Read(id);
            if (customer == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto("Invalid customer id: " + id));

            customer.Name = customerNameOrError.Value;

            return Ok();
        }

        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            Movie movie = _movieRepository.Read(movieId);
            if (movie == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto("Invalid movie id: " + movieId));

            Customer customer = _customerRepository.Read(id);
            if (customer == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto("Invalid customer id: " + id));

            if (customer.HasPurchasedMovie(movie))
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto("The movie is already purchased: " + movie.Name));

            customer.PurchaseMovie(movie);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            Customer customer = _customerRepository.Read(id);
            if (customer == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto("Invalid customer id: " + id));

            Result promotionCheck = customer.CanPromote();
            if (promotionCheck.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest, new ApiStringResponseDto(promotionCheck.Error));

            customer.Promote();

            return Ok();
        }
    }
}
