﻿using Microsoft.AspNetCore.Mvc;
using SmartWealth.TransactionService.Services;
using SmartWealth.TransactionService.ViewModels;
using SmartWealth.TransactionService.Utilities.Exceptions;

namespace SmartWealth.TransactionService.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController(ITransactionService service) : Controller
{
    private readonly ITransactionService _service = service;

    [HttpGet("all")]
    public async Task<IActionResult> GetTransactions()
    {
        try
        {
            return Ok(await _service.GetTransactionsAsync());
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("account/{id:guid}")]
    public async Task<IActionResult> GetTransactionsByAccount(string id)
    {
        try
        {
            return Ok(await _service.GetTransactionsByAccountAsync(id));
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransaction(Guid id)
    {
        try
        {
            return Ok(await _service.GetTransactionAsync(id));
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionViewModel createdTransaction)
    {
        try
        {
            await _service.CreateTransactionAsync(createdTransaction);
            return Created();
        }
        catch (NotValidException notValidException)
        {
            return BadRequest(notValidException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("edit/{id:guid}")]
    public async Task<IActionResult> EditTransaction(Guid id, [FromBody] TransactionViewModel editedTransaction)
    {
        try
        {
            await _service.EditTransactionAsync(id, editedTransaction);
            return Created();
        }
        catch (NotValidException notValidException)
        {
            return BadRequest(notValidException.Message);
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        try
        {
            await _service.DeleteTransactionAsync(id);
            return NoContent();
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}