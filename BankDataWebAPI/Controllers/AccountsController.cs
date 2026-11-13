using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankDataWebAPI.Data;
using BankDataWebAPI.Models;

namespace BankDataWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateAccount([FromBody] Accounts account)
        {
            if(account.UserID <= 0 ||
               string.IsNullOrWhiteSpace(account.AccountNumber) ||
               string.IsNullOrWhiteSpace(account.AccountType) ||
               account.Balance < 0)
            {
                return BadRequest(new
                {
                    error = "VALIDATION_ERROR",
                    message = "Provide valid account details",
                    details = (object)null
                });
            }
            account.IsActive = true;
            bool success = DBManager.InsertAccount(account);

            if(!success)
            {
                return Conflict(new
                {
                    error = "ACCOUNT_CREATION_FAILED",
                    message = "The account can't be created",
                    details = (object)null
                });
            }
            return StatusCode(201, new
            {
                message = "Account created successfully",
                accountNumber = account.AccountNumber
            });
        }
        [HttpGet("{accountNumber}")]
        public IActionResult GetAccount(string accountNumber)
        {
            Accounts account = DBManager.GetAccountByNumber(accountNumber);

            if(account == null)
            {
                return NotFound(new
                {
                    error = "ACCOUNT_NOT_FOUND",
                    message = "The account was not found",
                    details = (object)null
                });
            }
            return Ok(account);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] Accounts account)
        {
            if(id <= 0 ||
               string.IsNullOrWhiteSpace(account.AccountType) ||
               account.Balance < 0)
            {
                return BadRequest(new
                {
                    error = "VALIDATION_ERROR",
                    message = "Provide valid account details",
                    details = (object)null
                });
            }
            account.ID = id;
            bool success = DBManager.UpdateAccount(account);

            if(!success)
            {
                return NotFound(new
                {
                    error = "ACOUNT_NOT_FOUND",
                    message = "The account can't be updated",
                    details = (object)null
                });
            }
            return Ok(new
            {
                message = "Account updated successfully"
            });
        }
        [HttpDelete("{id}")]
        public IActionResult DeactivateAccount(int id)
        {
            bool success = DBManager.DeactivateAccount(id);
            if(!success)
            {
                return NotFound(new
                {
                    error = "ACOUNT_NOT_FOUND",
                    message = "The account can't be deactivated",
                    details = (object)null
                });
            }
            return Ok(new
            {
                message = "Account deactivated succesfully"
            });
        }
    }
}
