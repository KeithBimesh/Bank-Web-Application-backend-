using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankDataWebAPI.Data;
using BankDataWebAPI.Models;

namespace BankDataWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositData data)
        {
            if(data.Amount <= 0)
            {
                return BadRequest(new
                {
                    error = "INVALID_AMOUNT",
                    message = "The deposit amount must be greater than zero",
                    details = (object)null
                });
            }
            bool success = DBManager.Deposit(data.AccountID, data.Amount);

            if(!success)
            {
                return UnprocessableEntity(new
                {
                    error = "DEPOSIT_FAILED",
                    message = "The deposit was failed",
                    details = (object)null
                });
            }
            return Ok(new
            {
                message = "Deposit successful"
            });
        }
        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawData data)
        {
            if(data.Amount <= 0)
            {
                return BadRequest(new
                {
                    error = "INVALID_AMOUNT",
                    message = "The withdraw amount must be greater than zero",
                    details = (object)null
                });
            }
            bool success = DBManager.Withdraw(data.AccountID, data.Amount);

            if(!success)
            {
                return UnprocessableEntity(new
                {
                    error = "WITHDRAWAL_FAILED",
                    message = "The withdrawal was failed",
                    details = (object)null
                });
            }
            return Ok(new
            {
                message = "Withdrawal successful"
            });
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferData data)
        {
            if(data.Amount <= 0)
            {
                return BadRequest(new
                {
                    error = "INVALID_AMOUNT",
                    message = "The transfer amount must be greater than zero",
                    details = (object)null
                });
            }
            if(data.SenderAccountID == data.RecieverAccountID)
            {
                return BadRequest(new
                {
                    error = "SAME_ACCOUNT",
                    message = "Sender's account and reciever's account must be different",
                    details = (object)null
                });
            }
            bool success = DBManager.Transfer(
                data.SenderAccountID,
                data.RecieverAccountID,
                data.Amount);

            if(!success)
            {
                return UnprocessableEntity(new
                {
                    error = "TRANSFER_FAILED",
                    message = "The transfer was failed",
                    details = (object)null
                });
            }
            return Ok(new
            {
                message = "Transfer successful"
            });
        }
        [HttpGet("account/{accountID}")]
        public IActionResult GetTransactions(int accountID)
        {
            List<Transaction> transactions = DBManager.GetTransactionsByAccountID(accountID);

            return Ok(transactions);
        }
    }
}
